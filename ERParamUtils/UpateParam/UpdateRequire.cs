using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpdateParam
{
    public class ParamUpdateRequire
    {

        public static void Exec(ParamProject paramProject, UpdateCommand updateCommand) {

            ProcWeapon(paramProject,updateCommand);
            ProcMagic(paramProject,updateCommand);

        }

        public static void ProcWeapon(ParamProject? paramProject,UpdateCommand updateCommand) {

            if (paramProject == null)
                return;

            SoulsParam.Param? param = paramProject.FindParam(ParamNames.EquipParamWeapon);

            if (param == null) {
                return;
            }

            for (int i = 0; i < param.Rows.Count; i++) {

                SoulsParam.Param.Row row = param.Rows[i];


                string[] keys = { "properStrength", "properAgility",
                    "properMagic", "properFaith","properLuck"};
                foreach (var key in keys)
                {
                    UpdateCommandItem item = UpdateCommandItem.Create(row, key, "0"); 
                    updateCommand.AddItem(item);
                }
            }


        }

        public static void ProcMagic(ParamProject paramProject, UpdateCommand updateCommand)
        {
            if (paramProject == null)
                return;

            SoulsParam.Param? param = paramProject.FindParam(ParamNames.MagicParam);

            if (param == null)
            {
                return;
            }

            for (int i = 0; i < param.Rows.Count; i++)
            {
                SoulsParam.Param.Row row = param.Rows[i];
                if (!SpecEquipConfig.IsMagic(row.ID,EquipType.Good)) {
                    continue;
                }
                string[] keys = { "requirementLuck", "requirementIntellect", "requirementFaith" };
                foreach (var key in keys)
                {
                    UpdateCommandItem item = UpdateCommandItem.Create(row, key, "1");
                    updateCommand.AddItem(item);
                }
            }
        }

    }

    class ParamRemoveWeight {



        static void RemoveWeight(SoulsParam.Param? param, UpdateCommand updateCommand) {

            if (param == null)
            {
                return;
            }

            UpdateLogger.InfoTime("RemoveWeight {0}",param.Name);

            for (int i = 0; i < param.Rows.Count; i++)
            {

                SoulsParam.Param.Row row = param.Rows[i];
                UpdateCommandItem item = UpdateCommandItem.Create(row, "weight", "1");
                updateCommand.AddItem(item);
                //ParamRowUtils.SetCellValue(row, "weight", 1);
                //ParamRowUtils.SetCellValue(row, "sellValue", 5);
            }
        }

        public static void ProcProtector(ParamProject? paramProject, UpdateCommand updateCommand)
        {

            if (paramProject == null)
                return;

            SoulsParam.Param? param = paramProject.FindParam(ParamNames.EquipParamProtector);

            //UpdateLogger.Info("RemoveWeight ProcProtector");

            RemoveWeight(param,updateCommand);
        }

        public static void ProcWeapon(ParamProject? paramProject, UpdateCommand updateCommand)
        {

            if (paramProject == null)
                return;

            SoulsParam.Param? param = paramProject.FindParam(ParamNames.EquipParamWeapon);

            //UpdateLogger.Info("RemoveWeight ProcWeapon");
            RemoveWeight(param,updateCommand);

        }

        internal static void Exec(ParamProject project, UpdateCommand updateCommand)
        {
            ProcWeapon(project,updateCommand);
            ProcProtector(project,updateCommand);
        }
    }



}
