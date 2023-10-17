using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
{
    public class UpdateRequire
    {

        public static void Exec(ParamProject paramProject) {

            ProcWeapon(paramProject);
            ProcMagic(paramProject);

        }

        public static void ProcWeapon(ParamProject? paramProject) {

            if (paramProject == null)
                return;

            FSParam.Param param = paramProject.FindParam(ParamNames.EquipParamWeapon);

            if (param == null) {
                return;
            }

            for (int i = 0; i < param.Rows.Count; i++) {

                FSParam.Param.Row row = param.Rows[i];


                //properStrength,24,
                //properAgility,8,
                //properMagic,0,
                //properFaith,0,                 

                ParamRowUtils.SetCellValue(row, "properStrength", 0);
                ParamRowUtils.SetCellValue(row, "properAgility", 0);
                ParamRowUtils.SetCellValue(row, "properMagic", 0);
                ParamRowUtils.SetCellValue(row, "properFaith", 0);

            }


        }

        //requirementIntellect,0,
        //requirementFaith,46,
        public static void ProcMagic(ParamProject paramProject)
        {
            if (paramProject == null)
                return;

            FSParam.Param param = paramProject.FindParam(ParamNames.EquipParamWeapon);

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

    class RemoveWeight {


        public static void ProcProtector(ParamProject? paramProject)
        {

            if (paramProject == null)
                return;

            FSParam.Param param = paramProject.FindParam(ParamNames.EquipParamProtector);

            if (param == null)
            {
                return;
            }

            UpdateLogger.Info("RemoveWeight ProcProtector");

            for (int i = 0; i < param.Rows.Count; i++)
            {

                FSParam.Param.Row row = param.Rows[i];
                ParamRowUtils.SetCellValue(row, "weight", 1);

            }
        }

        public static void ProcWeapon(ParamProject? paramProject)
        {

            if (paramProject == null)
                return;

            FSParam.Param param = paramProject.FindParam(ParamNames.EquipParamWeapon);

            if (param == null)
            {
                return;
            }
            UpdateLogger.Info("RemoveWeight ProcWeapon");

            for (int i = 0; i < param.Rows.Count; i++)
            {

                FSParam.Param.Row row = param.Rows[i];
                ParamRowUtils.SetCellValue(row, "weight", 1);

            }
        }

        internal static void Exec(ParamProject project)
        {
            ProcWeapon(project);
            ProcProtector(project);
        }
    }

}
