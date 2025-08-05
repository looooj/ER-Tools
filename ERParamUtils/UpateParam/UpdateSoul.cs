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

            int times =  updateCommand.GetOption(UpdateParamOptionNames.TimesGetSoul);
            if (times <= 1)
                return;

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
                    //Albinauric;白金之子
                    if (row.Name.Contains("Albinauric"))
                    {
                        if ( times < 10 )
                            v = v * (times * 10);
                        else 
                            v = v * times; 
                    }
                    else {
                        v = v * times;
                    }
                    updateCommand.AddItem(ParamNames.NpcParam, row.ID, key, v);
                    
                }
            }
        }

    }
}
