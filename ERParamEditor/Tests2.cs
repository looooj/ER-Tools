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

        static void FindRemnant(List<string> items,ParamProject proj,string paramName) {

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

            FindRemnant(items, proj,ParamNames.ItemLotParamEnemy);
            FindRemnant(items, proj, ParamNames.ItemLotParamMap);
            string path = proj.GetUpdateDir() + "\\FindRemnant.txt";
            string r = string.Join("\n", items);

            File.WriteAllText(path, r);

        }
    }

}
