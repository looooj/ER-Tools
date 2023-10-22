using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
{
    public class ParamUpdateRequire
    {

        public static void Exec(ParamProject paramProject) {

            ProcWeapon(paramProject);
            ProcMagic(paramProject);

        }

        public static void ProcWeapon(ParamProject? paramProject) {

            if (paramProject == null)
                return;

            FSParam.Param? param = paramProject.FindParam(ParamNames.EquipParamWeapon);

            if (param == null) {
                return;
            }

            for (int i = 0; i < param.Rows.Count; i++) {

                FSParam.Param.Row row = param.Rows[i];     

                ParamRowUtils.SetCellValue(row, "properStrength", 0);
                ParamRowUtils.SetCellValue(row, "properAgility", 0);
                ParamRowUtils.SetCellValue(row, "properMagic", 0);
                ParamRowUtils.SetCellValue(row, "properFaith", 0);
            }


        }

        public static void ProcMagic(ParamProject paramProject)
        {
            if (paramProject == null)
                return;

            FSParam.Param? param = paramProject.FindParam(ParamNames.EquipParamWeapon);

            if (param == null)
            {
                return;
            }

            for (int i = 0; i < param.Rows.Count; i++)
            {
                FSParam.Param.Row row = param.Rows[i];

                ParamRowUtils.SetCellValue(row, "requirementIntellect", 0);
                ParamRowUtils.SetCellValue(row, "requirementFaith", 0);     
            }
        }

    }

    class ParamRemoveWeight {



        static void RemoveWeight(FSParam.Param? param) {

            if (param == null)
            {
                return;
            }

            UpdateLogger.Info("RemoveWeight ProcProtector");

            for (int i = 0; i < param.Rows.Count; i++)
            {

                FSParam.Param.Row row = param.Rows[i];
                ParamRowUtils.SetCellValue(row, "weight", 1);
                ParamRowUtils.SetCellValue(row, "sellValue", 5);
            }
        }

        public static void ProcProtector(ParamProject? paramProject)
        {

            if (paramProject == null)
                return;

            FSParam.Param? param = paramProject.FindParam(ParamNames.EquipParamProtector);

            UpdateLogger.Info("RemoveWeight ProcProtector");

            RemoveWeight(param);
        }

        public static void ProcWeapon(ParamProject? paramProject)
        {

            if (paramProject == null)
                return;

            FSParam.Param? param = paramProject.FindParam(ParamNames.EquipParamWeapon);

            UpdateLogger.Info("RemoveWeight ProcWeapon");
            RemoveWeight(param);

        }

        internal static void Exec(ParamProject project)
        {
            ProcWeapon(project);
            ProcProtector(project);
        }
    }



}
