using ERParamUtils;
using ERParamUtils.UpateParam;
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



        public static void TestName()
        {

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

        public static void TestDCX()
        {

            string dir = @"D:\docs\game\er\unpack-\unpack-files\msg\engus";
            //@"D:\docs\game\er\unpack-\unpack-files\msg\zhocn";

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

        public static void ExtractMsg()
        {

            string[] langs = { "engus", "zhocn" };
            foreach (string lang in langs)
            {
                //string dir = @"D:\docs\game\er\unpack-\unpack-files\msg\" + lang;
                string dir = "F:\\docs\\games\\ER\\Game\\msg\\" + lang;
                //string targetDir = @"D:\docs\game\er\unpack-\unpack-files-text";
                string targetDir = @"D:\myprojects\game-tools\ER-Tools\tmp\msg-text-cer";
                Directory.CreateDirectory(targetDir);
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

        public static void TestEvent()
        {


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

        public static void TestDCX2()
        {

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

        public static void TestVdf()
        {

            string path = @"D:\Program Files (x86)\Steam\steamapps\libraryfolders.vdf";

            var foldersObj = new VdfLibraryfolders();
            foldersObj.Parse(path);

            var appObj = new VdfAppManifest();
            string path1 = @"D:\Program Files (x86)\Steam\steamapps\appmanifest_1245620.acf";
            appObj.Parse(path1);

            var dir = appObj.InstallDir;

        }

        public static void TestFind()
        {

            //List<FindEquipLocation> result = new();
            //FindEquipUtils.Find(8384, 0, result);
        }


        public static void TestComp()
        {

            //projects\merchant
            //ParamProjectManager.CompareProject("org", "merchant");

            ParamProjectCompare.CompareProject("org", "rand", new List<string>());

        }




        public static SortedDictionary<int, string> GetMsgIdName(XmlElement root)
        {

            SortedDictionary<int, string> dict = new();
            var msgs = root.GetElementsByTagName("msg");

            for (int i = 0; i < msgs.Count; i++)
            {

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


        static string clearLineEnd(string s)
        {
            s = s.Trim();
            s = s.Replace("\x0d\x0a", " ");
            s = s.Replace("\x0d", " ");
            s = s.Replace("\x0a", " ");
            return s;
        }
        public static void GenChNames(string baseDir, string outDir, string[] names, string subName)
        {

            //assets\msg\item.msgbnd\msg\engUS\AccessoryName.fmg.xml
            //assets\msg\item.msgbnd\msg\zhoCN
            /*
            string[] names = { "AccessoryName","GoodsName",
                "ProtectorName","WeaponName","NpcName","PlaceName","GemName" };

            string baseDir = GlobalConfig.AssetsDir + @"\msg\item.msgbnd\msg";
            string outDir = GlobalConfig.AssetsDir + @"\msg\item-msg-text";
            */
            //string tmp = "D:\\myprojects\\game-tools\\ER-Tools\\tmp\\msg-text\\item_dlc01.msgbnd\\msg\\engUS\\AccessoryName.fmg.xml";
            foreach (string name in names)
            {

                string engFileName = string.Format("{0}\\{1}.msgbnd\\msg\\engUS\\{2}.fmg.xml", baseDir, subName, name);
                string zhoFileName = string.Format("{0}\\{1}.msgbnd\\msg\\zhoCN\\{2}.fmg.xml", baseDir, subName, name);

                if (!File.Exists(engFileName))
                {
                    continue;
                }
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
                    if (!choDict.TryGetValue(id, out string cname))
                    {
                        continue;
                    }

                    ename = clearLineEnd(ename);
                    cname = clearLineEnd(cname);

                    if (ename.Contains(";") || cname.Contains(";"))
                        continue;
                    if (ename.Length < 3 || cname.Length < 1 || ename.Length > 50)
                        continue;

                    outLines.Add(string.Format("{0};{1};{2}", id, ename, cname));

                }

                string outName = outDir + @"\" + subName + name + ".txt";
                Directory.CreateDirectory(outDir);
                File.WriteAllLines(outName, outLines, Encoding.UTF8);
            }

        }


        public static void TestMaps()
        {

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

        static void checkShopLineupParamRecipe()
        {

            var proj = GlobalConfig.GetCurrentProject();
            if (proj == null)
                return;

            var param = proj.FindParam(ParamNames.ShopLineupParamRecipe);
            if (param == null)
                return;

            List<string> items = new();
            foreach (var row in param.Rows)
            {

                var v = ParamRowUtils.GetCellInt(row, "eventFlag_forStock", 0);

                if (v <= 0)
                {
                    continue;
                }

                var equipId = ParamRowUtils.GetCellInt(row, "equipId", 0);
                if (equipId <= 0)
                    continue;
                var equipType = ParamRowUtils.GetCellInt(row, "equipType", 0);

                var line = string.Format("{0},{1},{2},{3}",
                    row.ID, v, equipId, equipType);
                items.Add(line);

            }


            File.WriteAllLines("forStockList.txt", items);
        }

        static void gen_menu_text()
        {
            //D:\myprojects\game - tools\ER - Tools\docs\item - menu - text
            string[] names = { "GR_MenuText", "ActionButtonText", "BloodMsg" };
            string outDir = "D:\\myprojects\\game-tools\\ER-Tools\\docs\\item-menu-text";
            string baseDir = "D:\\docs\\game\\er\\unpack-\\unpack-files-text\\menu.msgbnd\\msg";
            GenChNames(baseDir, outDir, names, "");
            //checkShopLineupParamRecipe();
        }



        static void GenDlcText(string baseDir, string outDir, string[] names, string[] subNames)
        {


            //string[] names = { "AccessoryName","GoodsName",
            //    "ProtectorName","WeaponName","NpcName","PlaceName","GemName" };

            List<string> tmpNames = new List<string>();
            foreach (string name in names)
            {
                tmpNames.Add(name);
                tmpNames.Add(name + "_dlc01");
                //tmpNames.Add(name + "_dlc02");
            }

            //string[] subNames = { "item", "item_dlc01", "item_dlc02"};

            //string baseDir = @"D:\myprojects\game-tools\ER-Tools\tmp\msg-text";
            //string outDir = @"D:\myprojects\game-tools\ER-Tools\docs\dlc-item-text";
            Directory.CreateDirectory(outDir);
            for (int i = 0; i < subNames.Length; i++)
            {
                GenChNames(baseDir, outDir, tmpNames.ToArray(), subNames[i]);
            }
        }


        static void GenItemText() {

            string[] names = { "AccessoryName","GoodsName",
                "ProtectorName","WeaponName","NpcName","PlaceName","GemName" };
            string[] subNames = { "item", "item_dlc01" };

            GenDlcText(@"D:\myprojects\game-tools\ER-Tools\tmp\msg-text-cer", 
                @"D:\myprojects\game-tools\ER-Tools\docs\cer-item-text",
               names, subNames);

        }

        static void GenMenuText()
        {
            string[] names = { "GR_MenuText", "ActionButtonText", "BloodMsg", "EventTextForTalk" };
            string[] subNames = { "menu","menu_dlc01" };

            GenDlcText(@"D:\myprojects\game-tools\ER-Tools\tmp\msg-text-cer", 
                @"D:\myprojects\game-tools\ER-Tools\docs\cer-menu-text",
               names, subNames);
        }

        static void WriteParamNames()
        {

            //
            string fn = "D:\\myprojects\\game-tools\\ER-Tools\\docs\\default-param-names.txt";

            bool nameFilter = GlobalConfig.UseParamNameFilter;
            var paramList = GlobalConfig.GetCurrentProject().GetParamNameList(nameFilter);
            File.WriteAllLines(fn, paramList);
        }

        static void findSoul()
        {
            var proj = GlobalConfig.GetCurrentProject();
            if (proj == null)
                return;

            var param = proj.FindParam(ParamNames.NpcParam);
            if (param == null)
                return;

            List<string> items = new();
            foreach (var row in param.Rows)
            {

                var v = ParamRowUtils.GetCellInt(row, "getSoul", 0);

                if (v < 1000)
                {
                    continue;
                }
                var isBoss = ParamRowUtils.GetCellInt(row, "isSoulGetByBoss", 0);//
                var line = string.Format("{0},{1},{2},{3}",
                    row.ID, row.Name, v, isBoss);
                items.Add(line);

            }
            File.WriteAllLines(proj.GetDir() + "\\findSoul.txt", items);

        }

        static void GenAutoLot(string[] eqIdFiles, string[] lotIdFile)
        {

            //string[] eqIdFiles = { "bell-bearing-a.txt", "smithing-stone.txt", "rune.txt", "gg.txt","tear.txt","pt.txt","prepare01.txt" };
            string baseDir = @"D:\myprojects\game-tools\ER-Tools\docs\tips\";
            foreach (var eqIdFile in eqIdFiles)
            {
                string fn = baseDir + eqIdFile;
                //string[] lotIdFile = { "auto-id-01.txt" };
                
                UpdateHelper.ExecLotAutoId(fn, lotIdFile);
            }

        }

        static void GenAutoLot()
        {

            string[] eqIdFiles1 = { "bell-bearing-a.txt", "smithing-stone.txt", "rune.txt", "gg.txt", "tear.txt", "pt.txt", "prepare02.txt", "wh.txt","tmp.txt" };
            string[] lotIdFile1 = { "auto-id-01.txt" };

            string[] eqIdFiles2 = { "test-lot.txt" };
            string[] lotIdFile2 = { "auto-id-02.txt" };


            GenAutoLot(eqIdFiles1, lotIdFile1);

        }
        //docs\item-msg-text\AccessoryName.txt
        static void GenAccessoryId()
        {

            var proj = GlobalConfig.GetCurrentProject();
            if (proj == null)
                return;

            var param = proj.FindParam(ParamNames.EquipParamAccessory);
            if (param == null)
                return;

            List<string> items = new();
            foreach (var row in param.Rows)
            {
                if (row.ID >= 1000 && row.ID <= 8240)
                {

                    var refId = ParamRowUtils.GetCellString(row, "refId", "");
                    if (refId == null || refId == "")
                        continue;

                    items.Add(refId);
                }
            }

            var r = string.Join(",", items);
            r = "@@var@@AccessoryId=" + r;
            string path = GlobalConfig.GetCurrentProject().GetUpdateDir() + "\\AccessoryId.txt";
            File.WriteAllText(path, r);
        }

        

        public static void Run()
        {

            //GenAccessoryId();
            //GenAutoLot();
            //GenItemText();
            GenMenuText();
            //Tools.CleanUpdateLog(5);
            //Tests2.FindRemnant();
            //ExtractMsg();
        }
    }
}
