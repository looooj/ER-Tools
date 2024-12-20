using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpdateParam
{
    public class UpdateBuddyStone
    {


        public static void Exec(ParamProject paramProject,UpdateCommand updateCommand)
        {
            ProcBuddyStone(paramProject,updateCommand);
            ProcRemoveConsume(paramProject,updateCommand);
        }

        public static void ProcRemoveConsume(ParamProject? paramProject, UpdateCommand updateCommand) {

            //200000 Black Knife Tiche
            //263010 Jarwight Puppet +10

            //92,消費MP,consumeMP,-1,
            //93,消費HP,consumeHP,660,

            
            if (paramProject == null)
                return;

            SoulsParam.Param? param = paramProject.FindParam(ParamNames.EquipParamGoods);

            if (param == null)
            {
                return;
            }

            UpdateLogger.InfoTime("UpdateBuddyStone ProcRemoveConsume");

            for (int i = 0; i < param.Rows.Count; i++)
            {

                SoulsParam.Param.Row row = param.Rows[i];

                if ( (row.ID >= 200000 && row.ID <= 300000)
                   || (row.ID >= 2200000 && row.ID <= 2220010) )
                {

                    updateCommand.AddItem(row, "consumeMP", "-1");
                    updateCommand.AddItem(row, "consumeHP", "-1");
                }
            }

        }
        public static void ProcBuddyStone(ParamProject? paramProject,UpdateCommand updateCommand)
        {

            if (paramProject == null)
                return;

            SoulsParam.Param? param = paramProject.FindParam(ParamNames.BuddyStoneParam);

            if (param == null)
            {
                return;
            }
            UpdateLogger.InfoTime("UpdateBuddyStone ProcBuddyStone");

            for (int i = 0; i < param.Rows.Count; i++)
            {

                SoulsParam.Param.Row row = param.Rows[i];
                if (row.ID < 2)
                    continue;


                updateCommand.AddItem(row, "eliminateTargetEntityId", "0");
                updateCommand.AddItem(row, "summonedEventFlagId", "0");
                updateCommand.AddItem(row, "activateRange", "9999");
                updateCommand.AddItem(row, "overwriteActivateRegionEntityId", "0");
            }
        }
    }
}
