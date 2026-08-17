using ERParamUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamEditor
{
    public class Tests2
    {
        //                    UpdateCommandItem.Create(ParamNames.NpcParam, 45610068, "getSoul", "10000000"));
        // Albinaurics
        public static void FindAlbinaurics()
        {

            var proj = GlobalConfig.GetCurrentProject();
            if (proj == null)
                return;

            var param = proj.FindParam(ParamNames.NpcParam);
            if (param == null)
                return;

            List<string> items = new List<string>();
            var rows = param.Rows;
            foreach (var row in rows)
            {
                if (row.Name == null)
                    continue;

                if (!row.Name.Contains("Albinauric"))
                {
                    continue;
                }
                var v = ParamRowUtils.GetCellInt(row, "getSoul", 0);

                if (v > 1000)
                {
                    items.Add(row.ID + "," + row.Name + "," + v);
                }

            }

            string path = proj.GetUpdateDir() + "\\Albinauric-Soul.txt";
            string r = string.Join("\n", items);

            File.WriteAllText(path, r);

        }

        static void FindRemnant(List<string> items, ParamProject proj, string paramName)
        {

            items.Add("====" + paramName + "====");
            var param = proj.FindParam(paramName);
            if (param == null)
                return;
            var rows = param.Rows;
            foreach (var row in rows)
            {

                var key = "lotItemId01";
                var v = ParamRowUtils.GetCellInt(row, key, 0);
                if (v >= 20900 && v < 20999)
                {
                    //List<string> items = new List<string>();

                    items.Add(row.ID + "," + row.Name + "," + v);


                }


            }
        }
        public static void FindRemnant()
        {

            var proj = GlobalConfig.GetCurrentProject();
            if (proj == null)
                return;
            List<string> items = new List<string>();

            FindRemnant(items, proj, ParamNames.ItemLotParamEnemy);
            FindRemnant(items, proj, ParamNames.ItemLotParamMap);
            string path = proj.GetUpdateDir() + "\\FindRemnant.txt";
            string r = string.Join("\n", items);

            File.WriteAllText(path, r);

        }


        public static Dictionary<int, string> LoadGraceText(string baseDir, string[] names)
        {

            Dictionary<int, string> ret = new Dictionary<int, string>();

            foreach (string name in names)
            {

                string fn = baseDir + "\\" + name;

                var lines = File.ReadAllLines(fn);

                foreach (string line in lines)
                {

                    var t = line.Trim();

                    var items = t.Split(";");

                    if (items.Length > 1)
                    {
                        var id = int.Parse(items[0]);
                        if (id >= 100000 )
                            ret.TryAdd(id, t);
                    }
                }

            }
            return ret;

        }

        public static void ExportParamGrace()
        {

            var proj = GlobalConfig.GetCurrentProject();
            if (proj == null)
                return;

            var param = proj.FindParam(ParamNames.BonfireWarpParam);
            if (param == null)
                return;



            string baseDir = "C:\\EldenRingMods\\ER-Tools\\docs\\cer-item-text";
            string[] names = { "item_dlc02PlaceName.txt", "item_dlc02PlaceName_dlc01.txt" };
            var d = LoadGraceText(baseDir, names);


            List<string> items = new List<string>();
            List<string> items2 = new List<string>();
            List<string> items3 = new List<string>();

            var rows = param.Rows;
            foreach (var row in rows)
            {

                if (row.Name == null || row.Name.Length < 6)
                {
                    continue;
                }

                var rowName = row.Name;
                var textId = ParamRowUtils.GetCellInt(row, "textId1", 0);
                var line = string.Format("{0};{1};{2}", textId, rowName, row.ID);
                items.Add(line);

                if (d.ContainsKey(textId))
                {
                    items2.Add(d[textId] + ";" + row.ID);
                }
                else {
                    items3.Add(line);
                }


            }

            string path = proj.GetUpdateDir() + "\\ParamGrace.txt";
            string r = string.Join("\n", items);
            File.WriteAllText(path, r);

            path = proj.GetUpdateDir() + "\\ParamGraceText.txt";
            r = string.Join("\n", items2);
            File.WriteAllText(path, r);

            path = proj.GetUpdateDir() + "\\ParamGraceText-N.txt";
            r = string.Join("\n", items3);
            File.WriteAllText(path, r);


        }


        static void getMaxMinValue(Dictionary<string, int> dict, string key, int v)
        {

            string maxKey = key + "_max";
            string minKey = key + "_min";

            if (dict.ContainsKey(maxKey))
            {
                int v1 = dict[maxKey];
                if (v1 < v)
                {
                    dict[maxKey] = v;
                }
            }
            else
            {
                dict[maxKey] = v;
            }

            if (dict.ContainsKey(minKey))
            {
                int v1 = dict[minKey];
                if (v1 > v)
                {
                    dict[minKey] = v;
                }
            }
            else
            {
                dict[minKey] = v;
            }
        }

        public static void FindGuardLevel()
        {

            var proj = GlobalConfig.GetCurrentProject();
            if (proj == null)
                return;
            var param = proj.FindParam(ParamNames.NpcParam);

            if (param == null)
                return;
            var rows = param.Rows;
            var dict = new Dictionary<string, int>();
            int v = 0;
            foreach (var row in rows)
            {
                v = ParamRowUtils.GetCellInt(row, "lockDist", 0);
                getMaxMinValue(dict, "lockDist", v);
                v = ParamRowUtils.GetCellInt(row, "guardLevel", 0);
                getMaxMinValue(dict, "guardLevel", v);

            }

            var lines = new List<string>();
            foreach (var key in dict.Keys)
            {
                lines.Add(key + "=" + dict[key]);
            }

            string path = proj.GetUpdateDir() + "\\npc-max-min.txt";
            string r = string.Join("\n", lines);

            File.WriteAllText(path, r);

        }
    }

}
