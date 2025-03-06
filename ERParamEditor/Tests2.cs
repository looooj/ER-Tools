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
        public static void FindAlbinaurics() {

            var proj = GlobalConfig.GetCurrentProject();
            if (proj == null)
                return;

            var param = proj.FindParam(ParamNames.NpcParam);
            if (param == null)
                return;

            List<string> items = new List<string>();
            var rows = param.Rows;
            foreach (var row in rows) {
                if (row.Name == null) 
                    continue;

                if (!row.Name.Contains("Albinauric")) {
                    continue;
                }
                var v = ParamRowUtils.GetCellInt(row, "getSoul", 0);

                if (v > 1000) {
                    items.Add(row.ID + "," + row.Name + "," + v);
                }

            }

            string path = proj.GetUpdateDir() + "\\Albinauric-Soul.txt";
            string r = string.Join("\n", items);

            File.WriteAllText(path, r);

        }
    }
}
