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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ERParamEditor
{
    public class Tests
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();



        public static void TestName() {

            string[] names = { "a", "abc", "abc123", "123" };

            var regx = new Regex("^[a-z]{3,}[0-9]?");
            foreach (string name in names)
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
                    SoulsFileUtils.ExtractDCX(file, targetDir);
                }
                catch (Exception ex) {

                    logger.Error(ex, file);
                }
            }

        }

        public static void ExtractMsg() {

            string[] langs = { "engus", "zhocn" };
            foreach (string lang in langs)
            {
                string dir = @"D:\docs\game\er\unpack-\unpack-files\msg\" + lang;
                string targetDir = @"D:\docs\game\er\unpack-\unpack-files-text";
                var files = Directory.GetFiles(dir, "*.dcx");
                foreach (var file in files)
                {
                    try
                    {
                        SoulsFileUtils.ExtractDCX(file, targetDir);
                    }
                    catch (Exception ex)
                    {

                        logger.Error(ex, file);
                    }
                }
            }
        }

        public static void TestEvent() {


            string dir = @"D:\docs\game\er\unpack-\unpack-files\event";
            string targetDir = @"D:\docs\game\er\unpack-\unpack-files-text\event";
            var files = Directory.GetFiles(dir, "*.dcx");
            int testCount = 0;
            foreach (var file in files)
            {
                try
                {
                    testCount++;
                    if (testCount > 10)
                        break;
                    SoulsFileUtils.ExtractDCX(file, targetDir);
                }
                catch (Exception ex)
                {

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
                    SoulsFileUtils.ExtractDCX(file, targetDir);
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

            ParamProjectCompare.CompareProject("org", "rand", new List<string>());

        }




        public static SortedDictionary<int, string> GetMsgIdName(XmlElement root) {

            SortedDictionary<int, string> dict = new();
            var msgs = root.GetElementsByTagName("msg");

            for (int i = 0; i < msgs.Count; i++) {

                XmlElement item = (XmlElement)msgs[i];
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


        static string clearLineEnd(string s) {
            s = s.Trim();
            s = s.Replace("\x0d\x0a", " ");
            s = s.Replace("\x0d", " ");
            s = s.Replace("\x0a", " ");
            return s;
        }
        public static void TestGenChNames(string baseDir, string outDir, string[] names) {

            //assets\msg\item.msgbnd\msg\engUS\AccessoryName.fmg.xml
            //assets\msg\item.msgbnd\msg\zhoCN
            /*
            string[] names = { "AccessoryName","GoodsName",
                "ProtectorName","WeaponName","NpcName","PlaceName","GemName" };

            string baseDir = GlobalConfig.AssetsDir + @"\msg\item.msgbnd\msg";
            string outDir = GlobalConfig.AssetsDir + @"\msg\item-msg-text";
            */
            foreach (string name in names) {

                string engFileName = string.Format("{0}\\engUS\\{1}.fmg.xml", baseDir, name);
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

                    ename = clearLineEnd(ename);
                    cname = clearLineEnd(cname);

                    if (ename.Contains(";") || cname.Contains(";"))
                        continue;
                    if (ename.Length < 3 || cname.Length < 1 || ename.Length > 50 )
                        continue;

                    outLines.Add(string.Format("{0};{1};{2}", id, ename, cname));

                }

                string outName = outDir + @"\" + name + ".txt";
                Directory.CreateDirectory(outDir);
                File.WriteAllLines(outName, outLines, Encoding.UTF8);
            }

        }


        public static void TestMaps() {

            /*
             			FMG fMG = game.ItemFMGs["NpcName"];
			merchantNames = new HashSet<int>(from e in fMG.Entries
				where e.Text != null && e.Text.Contains("Merchant")
				select e.ID);
			foreach (PARAM.Row row10 in game.Params["WorldMapPointParam"].Rows)
			{
				if ((int)row10["textId1"].Value > 0)
				{
					row10["eventFlagId"].Value = (uint)mapUnlockFlag;
				}
				rewriteMerchantIcons(row10);
			}
			foreach (PARAM.Row row11 in game.Params["BonfireWarpParam"].Rows)
			{
				rewriteMerchantIcons(row11);
			}
            

            		void rewriteMerchantIcons(PARAM.Row row)
		{
			for (int m = 1; m <= 8; m++)
			{
				int num8 = (int)row[$"textId{m}"].Value;
				if (merchantNames.Contains(num8))
				{
					row[$"textEnableFlagId{m}"].Value = (uint)mapUnlockFlag;
					int value5 = 0;
					if (result != null && result.MerchantGiftFlags.TryGetValue(num8, out value5) && value5 != 400049)
					{
						row[$"textDisableFlagId{m}"].Value = (uint)value5;
					}
				}
			}
		}

             */

        }

        static void checkShopLineupParamRecipe() {

            var proj = GlobalConfig.GetCurrentProject();
            if (proj == null)
                return;

            var param = proj.FindParam(ParamNames.ShopLineupParamRecipe);
            if (param == null)
                return;

            List<string> items = new();
            foreach (var row in param.Rows) {

                var v = ParamRowUtils.GetCellInt(row, "eventFlag_forStock",0);

                if (v <= 0) {
                    continue;
                }

                var equipId = ParamRowUtils.GetCellInt(row, "equipId", 0);
                if (equipId <= 0)
                    continue;
                var equipType = ParamRowUtils.GetCellInt(row, "equipType", 0);

                var line = string.Format("{0},{1},{2},{3}", 
                    row.ID,v,equipId,equipType);
                items.Add(line);
                
            }


            File.WriteAllLines("forStockList.txt", items);
        }
        public static void Run() {

            //D:\myprojects\game - tools\ER - Tools\docs\item - menu - text
            string[] names = { "GR_MenuText", "ActionButtonText", "BloodMsg" };
            string outDir = "D:\\myprojects\\game-tools\\ER-Tools\\docs\\item-menu-text";
            string baseDir = "D:\\docs\\game\\er\\unpack-\\unpack-files-text\\menu.msgbnd\\msg";
            TestGenChNames(baseDir, outDir, names);
            //checkShopLineupParamRecipe();
        }
    }
}
