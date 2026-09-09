using ERParamUtils.UpateParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpdateParam
{
    public class UpdateSmithingStone
    {
        public static void Exec(ParamProject paramProject, UpdateCommand updateCommand)
        {
            if (!ModConfig.CanSetupSomber())
            {
                return;
            }

            var param = paramProject.FindParam(ParamNames.EquipMtrlSetParam);
            if (param == null)
                return;

            var rows = param.Rows;
            foreach (var row in rows)
            {

                int id = ParamRowUtils.GetCellInt(row, "materialId01", 0);
                int cat = ParamRowUtils.GetCellInt(row, "materialCate01", 0);
                int num = ParamRowUtils.GetCellInt(row, "itemNum01", 0);
                if (cat == 4 && id >= 10100 && id < 10110 && num > 1)
                {
                    updateCommand.AddItem(row, "itemNum01", 1);
                }

            }

        }
        /*
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
        }*/

    }
}

