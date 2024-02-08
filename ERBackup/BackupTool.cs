using ERParamUtils;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERBackup
{

    public class BackupItem
    {
        string time;
        string name;
        string fullFilename;

        public string Time { get => time; set => time = value; }
        public string Name { get => name; set => name = value; }
        public string FullFilename { get => fullFilename; set => fullFilename = value; }

        public static bool Parse(string line, ref BackupItem item)
        {
            string[] ss = line.Split(';');
            item.Time = ss[0];
            item.Name = ss[1];
            item.FullFilename = ss[2];
            return true;
        }
    }
    class BackupTool
    {

        string ownerId = "";
        const string autoTag = "__auto__";


        public string GetSaveDir()
        {

            string userDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            string d = string.Format("{0}\\AppData\\Roaming\\EldenRing\\{1}", userDir, ownerId);

            return d;
        }

        public string GetBakDir()
        {

            string d = GlobalConfig.BaseDir + @"\baks";
            Directory.CreateDirectory(d);
            return d;
        }

        public void ExecAutoBak()
        {
            ExecBak(autoTag);
        }

        List<string> GetBakNames()
        {

            List<string> bakNames = new();
            string[] files = Directory.GetFiles(GetSaveDir());
            foreach (var file in files)
            {

                if (file.Contains("ER0000"))
                {
                    bakNames.Add(Path.GetFileName(file));
                }
            }
            return bakNames;
        }

        public void ExecBak(string tagName)
        {
            tagName = tagName.Trim();
            if (tagName.Length < 1)
                tagName = "t";

            string timeStr = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            string fn = string.Format("er_{0}_{1}.zip", tagName, timeStr);
            fn = PathNameUtils.ConvertName(fn);
            string zname = GetBakDir() + @"\" + fn;

            ZipArchive zip = ZipFile.Open(zname, ZipArchiveMode.Create);

            var bakNames = GetBakNames();
            foreach (string bakName in bakNames)
            {

                string t = GetSaveDir() + @"\" + bakName;
                ZipArchiveEntry entry = zip.CreateEntryFromFile(t, bakName);

            }
            zip.Dispose();
        }

        void Save()
        {

        }

        public void CleanAll()
        {
            string[] files = Directory.GetFiles(GetBakDir());
            foreach (string n in files)
            {
                if (n.EndsWith(".zip"))
                {
                    File.Delete(n);
                }
            }
        }

        public List<string> GetZipList(bool includeAutoBackup = false)
        {

            List<string> zipList = new();
            string[] files = Directory.GetFiles(GetBakDir());
            foreach (string n in files)
            {
                if (!includeAutoBackup && n.Contains(autoTag))
                    continue;
                if (n.EndsWith(".zip"))
                    zipList.Add(n);
            }
            return zipList;
        }

        public void ExecRestore(string zipName, bool testFlag = false)
        {


            ZipArchive zip = ZipFile.Open(zipName, ZipArchiveMode.Read);

            string destDir = GetSaveDir();
            if (testFlag)
                destDir = GlobalConfig.BaseDir + @"\tmp";

            zip.ExtractToDirectory(destDir, true);
            zip.Dispose();
        }

        internal void Init()
        {
            var m = SteamAppPath.GetEldenRingAppManifest();
            ownerId = m.LastOwner;

        }
    }
}
