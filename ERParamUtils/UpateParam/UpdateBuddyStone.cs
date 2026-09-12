using ERParamUtils.UpateParam;
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
            //for cer
            DisableDebuff(paramProject, updateCommand);

            if (updateCommand.HaveOption(UpdateParamOptionNames.DefaultEnhance) ||
                updateCommand.HaveOption(UpdateParamOptionNames.EnhanceBuddy)
                )
            {

                ProcRemoveConsume(paramProject, updateCommand);
                ProcNpcParam(paramProject, updateCommand);
            }
           

            if (!updateCommand.HaveOption(UpdateParamOptionNames.EnhanceBuddy)) {
                UpdateLogger.InfoTime("===UpdateBuddyStone Skip");
                return;
            }
            if (!ModConfig.EnhanceBuddy()) {
                UpdateLogger.InfoTime("===UpdateBuddyStone Skip ModConfig");
                return;
            }
            UpdateLogger.InfoTime("===UpdateBuddyStone.Exec");

            ProcBuddyStone(paramProject,updateCommand);
        }

        private static void ProcRemoveConsume(ParamProject? paramProject, UpdateCommand updateCommand) {

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
            UpdateLogger.Begin(param.Name);
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
        //Spirit Summon
        private static void ProcNpcParam(ParamProject? paramProject, UpdateCommand updateCommand)
        {
            if (paramProject == null)
                return;

            SoulsParam.Param? param = paramProject.FindParam(ParamNames.NpcParam);

            if (param == null)
            {
                return;
            }
            UpdateLogger.InfoTime("UpdateBuddyStone ProcNpcParam");

            var rows = param.Rows;

            foreach (var row in rows) { 
                if ( row.Name == null )
                     continue; 
                if (row.Name.Contains("Spirit Summon")) {

                    var hp = ParamRowUtils.GetCellInt(row, "hp", 0);
                    var times = 2;
                    if (hp > 0) {
                        if (hp < 500)
                            times = 4;
                        if (hp < 1000)
                            times = 3;
                        hp = hp * times;
                        updateCommand.AddItem(row, "hp", hp);
                    }
                }
            }
        }

        //For Mod CER
        private static void DisableDebuff(ParamProject? paramProject, UpdateCommand updateCommand) {

            if (paramProject == null)
                return;

            SoulsParam.Param? param = paramProject.FindParam(ParamNames.SpEffectParam);

            if (param == null)
            {
                return;
            }

            var rows = param.Rows;
            foreach (var row in rows) {

                if (row.ID >= 40002 && row.ID <= 40122) { 
                    if ( row.Name == null )
                        continue;
                    if (row.Name.Contains("trigger for player")) {

                        UpdateSpEffect.AddKeyValues(updateCommand, row.ID + "",
                            "maxHpRate;1;maxMpRate;1;maxStaminaRate;1");
                    }
                }
            }

        }

        private static void ProcBuddyStone(ParamProject? paramProject,UpdateCommand updateCommand)
        {


            if (paramProject == null)
                return;

            SoulsParam.Param? param = paramProject.FindParam(ParamNames.BuddyStoneParam);

            if (param == null)
            {
                return;
            }
            UpdateLogger.InfoTime("UpdateBuddyStone ProcBuddyStone");
            UpdateLogger.Begin(param.Name);
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
