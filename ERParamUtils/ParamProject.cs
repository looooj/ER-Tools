using ERParamUtils;
using SoulsFormats;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static SoulsFormats.MQB;

namespace ERParamUtils
{
    // ref DSMapStudio

    public class ParamProject
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        string Name ="";
        string ModRegulationPath="";
        string CreateTime="";

        BND4 currentBinder=null;
        GameType gameType = GameType.EldenRing;

        private Dictionary<string, SoulsParam.Param> _params = new() ;
        //private Dictionary<string, ParamWrapper> _paramWrappers = new();
        private ulong _paramVersion;

        private Dictionary<string, PARAMDEF> _paramdefs = new();

        private Dictionary<string, Dictionary<ulong, PARAMDEF>> _patchParamdefs = new();


        public ParamProject(string name) {
            Name = name;
        }

        public void Create(string modRegulationPath) {
            ModRegulationPath = modRegulationPath;
            CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public string GetName()
        {
            return Name;
        }

        public string GetDir() {

            return GlobalConfig.GetProjectDir(Name);
        } 

        public string GetRegulationPath() {

            return GetDir() + @"\regulation.bin";
        }

        public string GetOrginalRegulationPath() {
            return GetRegulationPath() + ".org";
        }

        public string GetModRegulationPath()
        {
            return ModRegulationPath;
        }


        public string GetUpdateDir()
        {
            return GetDir() + @"\update";
        }

        public string GetUpdateFile(string filename)
        {
            return  GetUpdateDir() + @"\" + filename;
        }


        private List<(string, PARAMDEF)> LoadParamdefs()
        {
            _paramdefs = new Dictionary<string, PARAMDEF>();
            _patchParamdefs = new Dictionary<string, Dictionary<ulong, PARAMDEF>>();
            var dir = ParamdexConfig.Get().GetParamdefDir();
            var files = Directory.GetFiles(dir, "*.xml");
            List<(string, PARAMDEF)> defPairs = new List<(string, PARAMDEF)>();
            foreach (var f in files)
            {
                var pdef = PARAMDEF.XmlDeserialize(f);
                _paramdefs.Add(pdef.ParamType, pdef);
                defPairs.Add((f, pdef));
            }

            // Load patch paramdefs
            var patches = ParamdexConfig.Get().GetParamdefPatches();
            foreach (var patch in patches)
            {
                var pdir = ParamdexConfig.Get().GetParamdefPatchDir(patch);
                var pfiles = Directory.GetFiles(pdir, "*.xml");
                foreach (var f in pfiles)
                {
                    var pdef = PARAMDEF.XmlDeserialize(f);
                    defPairs.Add((f, pdef));
                    if (!_patchParamdefs.ContainsKey(pdef.ParamType))
                    {
                        _patchParamdefs[pdef.ParamType] = new Dictionary<ulong, PARAMDEF>();
                    }
                    _patchParamdefs[pdef.ParamType].Add(patch, pdef);
                }
            }

            return defPairs;
        }



        public void LoadParams() {

            _params.Clear();
            //_paramWrappers.Clear();

            string path = GetRegulationPath();
            string orgPath = GetOrginalRegulationPath();
            string modPath = GetModRegulationPath();

            if (!File.Exists(path)) {

                if (!File.Exists(orgPath))
                {

                    File.Copy(modPath, orgPath, true);
                    string timeId = DateTime.Now.ToString("_yyyyMMdd_HHmmss");
                    File.Copy(orgPath, orgPath + timeId);
                    File.Copy(orgPath, path);
                }
                else {
                    File.Copy(orgPath, path);
                }
            }

            LoadParamdefs();
            currentBinder = SFUtil.DecryptERRegulation(path);
            LoadParamFromBinder(currentBinder,out _paramVersion, true);
            ImpRowNames();


        }

        private void LoadParamFromBinder(IBinder parambnd, out ulong version, bool checkVersion = false) //, ref Dictionary<string, FSParam.Param> paramBank, out ulong version, bool checkVersion = false)
        {
            _params.Clear();
            //_paramWrappers.Clear();

            bool success = ulong.TryParse(parambnd.Version, out version);
            if (checkVersion && !success)
            {
                throw new Exception($@"Failed to get regulation version. Params might be corrupt.");
            }
            Dictionary<string, SoulsParam.Param> paramBank = _params;

            List<string> paramNames = new();
            foreach (var f in parambnd.Files)
            {
                //paramNames.Add(f.Name);

                if (!f.Name.ToUpper().EndsWith(".PARAM"))
                {
                    continue;
                }

                string paramName = Path.GetFileNameWithoutExtension(f.Name);


                paramNames.Add(paramName);

                if (!ParamNameFilter.IncludesParam(paramName))
                {
                    continue;
                }

                if (paramBank.ContainsKey(paramName))
                {
                    continue;
                }

                
                if (f.Name.EndsWith("LoadBalancerParam.param") && ParamdexConfig.Get().Type != GameType.EldenRing)
                {
                    continue;
                }

                SoulsParam.Param p = SoulsParam.Param.Read(f.Bytes);
                p.Name = paramName;
                if (!_paramdefs.ContainsKey(p.ParamType) && !_patchParamdefs.ContainsKey(p.ParamType))
                {
                    continue;
                }

                // Try to fixup Elden Ring ChrModelParam for ER 1.06 because many have been saving botched params and
                // it's an easy fixup
                // todo 
                /*
                if (gameType == GameType.EldenRing &&
                    p.ParamType == "CHR_MODEL_PARAM_ST" &&
                    _paramVersion == 10601000)
                {
                    p.FixupERChrModelParam();
                }
                */
                // Lookup the correct paramdef based on the version
                PARAMDEF? def = null;
                if (_patchParamdefs.ContainsKey(p.ParamType))
                {
                    var keys = _patchParamdefs[p.ParamType].Keys.OrderByDescending(e => e);
                    foreach (var k in keys)
                    {
                        if (version >= k)
                        {
                            def = _patchParamdefs[p.ParamType][k];
                            break;
                        }
                    }
                }

                // If no patched paramdef was found for this regulation version, fallback to vanilla defs
                if (def == null)
                    def = _paramdefs[p.ParamType];

                try
                {
                    if (p.CheckParamdef(def)) { 

                    }
                    p.Name = paramName;
                    p.ApplyParamdef(def);
                    p.MakeCellIndex();
                    paramBank.Add(paramName, p);
                }
                catch (Exception)
                {
                    //var name = f.Name.Split("\\").Last();
                    //TaskManager.warningList.TryAdd($"{name} DefFail", $"Could not apply ParamDef for {name}");
                }
            }

            string tmpFilenames = GlobalConfig.GetProjectDir(Name) + @"\param-list.txt";
            File.WriteAllLines(tmpFilenames, paramNames);
        }



        public void ImpRowNames()
        {

            foreach (string paramName in _params.Keys)
            {
       
    
                var names = RowNamesManager.LoadNames(paramName);
                if (names.Count < 1)
                    continue;

                SoulsParam.Param param = _params[paramName];

                foreach (SoulsParam.Param.Row row in param.Rows)
                {
                    if (names.ContainsKey(row.ID) && (row.Name == null || row.Name == ""))
                    {
                        row.Name = names[row.ID];
                    }
                }
            }

        }


        public SoulsParam.Param? FindParam(string paramName) {


            if (_params.ContainsKey(paramName) ) { 
               
                return _params[paramName];
            }
            return null;
        }

        public List<string> GetParamNameList(bool filter) {

            List<string> paramList = new();

            foreach(var k in _params.Keys) {

                if (ParamNameFilter.IncludesParam(k) || !filter) {
                    paramList.Add(k);
                }
            }

            return paramList;
        }

        public Dictionary<string,SoulsParam.Param> GetParamDict() {

            return _params;
        }
        

        public void SaveParams() {

            if (currentBinder == null)
                return;

            foreach (BinderFile file in currentBinder.Files)
            {
                string paramName = Path.GetFileNameWithoutExtension(file.Name);
                if (!_params.ContainsKey(paramName)) {
                    continue;
                }
                var p = _params[paramName];
                if (!p.Changed)
                    continue;
                logger.Info("===save change {0}", paramName);
                file.Bytes = _params[paramName].Write();
            }

            string savePath = GetRegulationPath();

            logger.Info("===save {0}", savePath);

            SFUtil.EncryptERRegulation(savePath, currentBinder as BND4);
        }

        public void Restore() {

            string source = GetOrginalRegulationPath();
            string target = GetRegulationPath();

            if (File.Exists(source)) {

                logger.Info("===restore {0} -> {1}", source, target);

                File.Copy(source, target,true);
                LoadParams();
            }
            
        }

        public void Publish() {

            string source = GetRegulationPath();
            string target = GetModRegulationPath();

            logger.Info("===publish {0} -> {1}",
                source, target);

            File.Copy(source,target,true);
        }

        public static readonly string ConfigName="project.txt";

        public void SaveConfig() {


            DictConfig config = new();

            string path = GlobalConfig.GetProjectDir(Name) + @"\" + ConfigName;

            config.SetString("ModRegulationPath", GetModRegulationPath());
            config.SetString("CreateTime", CreateTime);
            config.Save(path);

        }

        public void LoadConfig() {

            DictConfig config = new DictConfig();
            string path = GlobalConfig.GetProjectDir(Name) + @"\" + ConfigName;
            config.Load(path);

            ModRegulationPath = config.GetString("ModRegulationPath","?");
            CreateTime = config.GetString("CreateTime", "?");
        }

        public void InitDirs() {

            //Directory.CreateDirectory(GetUpdateDir());
        } 

        public void CheckConfig() { 
        

        }

        internal void Init()
        {
            

            string updateDir = GetUpdateDir();
            Directory.CreateDirectory(updateDir);

            string templateDir = GlobalConfig.GetTemplateDir();

            var files = Directory.GetFiles(templateDir, "*.txt");
            foreach (var file in files) {

                string newFile = updateDir + @"\" + Path.GetFileName(file);
                if (File.Exists(newFile)) {
                    continue;
                }
                File.Copy(file, newFile);
            }

        }
    }
}