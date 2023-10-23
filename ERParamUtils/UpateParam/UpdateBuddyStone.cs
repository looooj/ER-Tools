using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
{
    public class UpdateBuddyStone
    {


        public static void Exec(ParamProject paramProject)
        {
            ProcBuddyStone(paramProject);
            ProcRemoveConsume(paramProject);
        }

        public static void ProcRemoveConsume(ParamProject? paramProject) {

            //200000 Black Knife Tiche
            //263010 Jarwight Puppet +10

            //92,消費MP,consumeMP,-1,
            //93,消費HP,consumeHP,660,

            
            if (paramProject == null)
                return;

            SoulsParam.Param param = paramProject.FindParam(ParamNames.EquipParamGoods);

            if (param == null)
            {
                return;
            }

            UpdateLogger.Info("UpdateBuddyStone ProcRemoveConsume");

            for (int i = 0; i < param.Rows.Count; i++)
            {

                SoulsParam.Param.Row row = param.Rows[i];
                if (row.ID < 200000 || row.ID > 300000)
                    continue;

                ParamRowUtils.SetCellValue(row, "consumeMP", -1);
                ParamRowUtils.SetCellValue(row, "consumeHP", -1);
            }

        }
        public static void ProcBuddyStone(ParamProject? paramProject)
        {

            if (paramProject == null)
                return;

            SoulsParam.Param param = paramProject.FindParam(ParamNames.BuddyStoneParam);

            if (param == null)
            {
                return;
            }
            UpdateLogger.Info("UpdateBuddyStone ProcBuddyStone");

            for (int i = 0; i < param.Rows.Count; i++)
            {

                SoulsParam.Param.Row row = param.Rows[i];
                if (row.ID < 2)
                    continue;
                /*
#new value BuddyStoneParam  10000100 Stormveil Castle eliminateTargetEntityId 10005100->0
eliminateTargetEntityId;0
#new value BuddyStoneParam  10000100 Stormveil Castle summonedEventFlagId 10002100->0
summonedEventFlagId;0
#new value BuddyStoneParam  10000100 Stormveil Castle activateRange 999->9999
activateRange;9999
#new value BuddyStoneParam  10000100 Stormveil Castle overwriteActivateRegionEntityId 10002150->0                 */
                ParamRowUtils.SetCellValue(row, "eliminateTargetEntityId", 0);
                ParamRowUtils.SetCellValue(row, "summonedEventFlagId", 0);
                ParamRowUtils.SetCellValue(row, "activateRange", 9999);
                ParamRowUtils.SetCellValue(row, "overwriteActivateRegionEntityId", 0);
            }
        }
    }
}
