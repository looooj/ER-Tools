using ERParamUtils;
using Org.BouncyCastle.Utilities;
using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using VdfFile;

namespace ERParamEditor
{
    public class Tests
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();



        public static void TestName() {

            string[] names = { "a", "abc", "abc123","123" };

            var regx = new Regex("^[a-z]{3,}[0-9]?");
            foreach(string name in names)
            if (!regx.Match(name).Success)
            {
                MessageBox.Show("Not Match " + name);
            }

        }

        class TestProgress : IProgress<float>
        {
            public void Report(float value)
            {
                //throw new NotImplementedException();
            }
        }

        public static void TestDCX() {

            string dir = @"D:\docs\game\er\unpack-\unpack-files\msg\engus";
            //@"D:\docs\game\er\unpack-\unpack-files\msg\zhocn";

            string targetDir = @"D:\docs\game\er\unpack-\unpack-files-text";

            var files = Directory.GetFiles(dir, "*.dcx");
            foreach (var file in files) {
                try
                {
                    BinderFileUtils.ExtractDCX(file, targetDir);
                }
                catch (Exception ex) {

                    logger.Error(ex, file);
                }
            }

        }

        public static void TestDCX2() {

            string dir = @"D:\docs\game\er\unpack-\unpack-files\map\m10\m10_00_00_00";
          

            string targetDir = @"D:\docs\game\er\unpack-\unpack-files-text";

            var files = Directory.GetFiles(dir, "*.*");
            foreach (var file in files)
            {

                try
                {
                    BinderFileUtils.ExtractDCX(file, targetDir);
                }
                catch (Exception ex)
                {

                    logger.Error(ex, file);
                }
            }

        }

        public static void TestVdf() {

            string path = @"D:\Program Files (x86)\Steam\steamapps\libraryfolders.vdf";

            var foldersObj = new VdfLibraryfolders();
            foldersObj.Parse(path);

            var appObj = new VdfAppManifest();
            string path1 = @"D:\Program Files (x86)\Steam\steamapps\appmanifest_1245620.acf";
            appObj.Parse(path1);

            var dir = appObj.InstallDir;          

        }

        public static void TestFind() {

            //List<FindEquipLocation> result = new();
            //FindEquipUtils.Find(8384, 0, result);
        }


        public static void TestComp() {

            //projects\merchant
            //ParamProjectManager.CompareProject("org", "merchant");

            ParamProjectCompare.CompareProject("org", "rand",new List<string>());

        }




        public static SortedDictionary<int, string> GetMsgIdName(XmlElement root) {

            SortedDictionary<int, string> dict = new();
            var msgs = root.GetElementsByTagName("msg");

            for (int i = 0; i < msgs.Count; i++) {

                XmlElement item =(XmlElement)msgs[i];
                XmlElement idNode = (XmlElement)item.SelectSingleNode("ID");
                XmlElement textNode = (XmlElement)item.SelectSingleNode("Text");
                int id = int.Parse(idNode.InnerText);
                string text = textNode.InnerText;
                text = text.Trim();
                if (text.ToUpper().Contains("[ERROR]") || text.Length < 1)
                    continue;
                dict.Add(id, text);
            }

            return dict;
        }

        public static void TestGenChNames() {

            //assets\msg\item.msgbnd\msg\engUS\AccessoryName.fmg.xml
            //assets\msg\item.msgbnd\msg\zhoCN
            string[] names = { "AccessoryName","GoodsName",
                "ProtectorName","WeaponName","NpcName","PlaceName","GemName" };

            string baseDir = GlobalConfig.AssetsDir+ @"\msg\item.msgbnd\msg";
            string outDir = GlobalConfig.AssetsDir + @"\msg\item-msg-text";
            foreach (string name in names) {

                string engFileName = string.Format("{0}\\engUS\\{1}.fmg.xml",baseDir,name);
                string zhoFileName = string.Format("{0}\\zhoCN\\{1}.fmg.xml", baseDir, name);

                XmlDocument engDoc = new();
                XmlDocument zhoDoc = new();
                engDoc.Load(engFileName);
                zhoDoc.Load(zhoFileName);

                var engDict = GetMsgIdName(engDoc.DocumentElement);
                var choDict = GetMsgIdName(zhoDoc.DocumentElement);
                var keys = engDict.Keys;

                
                List<string> outLines = new();
                foreach (int id in keys)
                {
                    string ename = engDict[id];
                    if (!choDict.TryGetValue(id, out string cname)) {
                        continue;
                    }

                    outLines.Add(string.Format("{0};{1};{2}",id,ename,cname));

                }

                string outName = outDir + @"\" + name + ".txt";
                Directory.CreateDirectory(outDir);
                File.WriteAllLines(outName, outLines,Encoding.UTF8);
            }

        }

        public static void Run() {

            TestGenChNames();
        }
    }
}
