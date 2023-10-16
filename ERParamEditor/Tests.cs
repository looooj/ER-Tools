using ERParamUtils;
using Org.BouncyCastle.Utilities;
using SoulsFormats;
using System;
using System.Collections.Generic;
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

            var files = Directory.GetFiles(dir, "*.*");
            foreach (var file in files) {

                BinderFileUtils.ExtractDCX(file, targetDir );
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

            List<FindEquipLocation> result = new();
            FindEquipUtils.Find(8384, 0, result);
        }


        public static void Run() {

            TestFind();
        }
    }
}
