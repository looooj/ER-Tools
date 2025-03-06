using ERParamUtils.UpdateParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
{
    public class UpdateSoul
    {

        public static void Proc(ParamProject? paramProject, UpdateCommand updateCommand) {

            if (paramProject == null) {
                return;
            }

            var param = paramProject.FindParam(ParamNames.NpcParam);
            if (param == null)
                return;

            List<string> items = new List<string>();
            var rows = param.Rows;
            string key = "getSoul";
            foreach (var row in rows)
            {
                if (row.Name == null)
                    continue;


                var v = ParamRowUtils.GetCellInt(row, key, 0);

                if (v > 0)
                {

                    if (row.Name.Contains("Albinauric"))
                    {
                        v = v * 10;
                    }
                    else {
                        v = v * 2;
                    }
                    updateCommand.AddItem(ParamNames.NpcParam, row.ID, key, v);
                    
                }
            }
        }

    }
}
