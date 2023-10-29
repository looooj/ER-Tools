using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
{
    public class UpdateSmithingStone
    {

        public static void Proc(ParamProject? paramProject, UpdateCommand updateCommand)
        {

            if (paramProject == null)
                return;

            SoulsParam.Param? param = paramProject.FindParam(ParamNames.EquipMtrlSetParam);

            if (param == null)
            {
                return;
            }

            for (int i = 1; i <= 24; i++)
            {
                var row = ParamRowUtils.FindRow(param, i);
                if (row == null)
                    continue;

                updateCommand.AddItem(row, "itemNum01", "1");
            }
        }

    }
}

