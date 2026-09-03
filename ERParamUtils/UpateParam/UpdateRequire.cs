using ERParamUtils.UpateParam;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpdateParam
{
    public class ParamUpdateRequire
    {



        public static void ExecSpec(ParamProject paramProject, UpdateCommand updateCommand)
        {
            /*
           34000000;Finger Seal;指头圣印记
34010000;Godslayer's Seal;狩猎神祇圣印记
34020000;Giant's Seal;巨人圣印记
34030000;Gravel Stone Seal;碎石圣印记
34040000;Clawmark Seal;爪痕圣印记
34060000;Golden Order Seal;黄金律法圣印记
34070000;Erdtree Seal;黄金树圣印记
34080000;Dragon Communion Seal;龙飨印记
34090000;Frenzied Flame Seal;癫火圣印记  
             */
            var idSeal = new int[]{ 34000000, 34010000, 34020000,
                34030000, 34040000,
                34060000, 34070000,
                34080000, 34090000,
            };

            foreach (var id in idSeal) { 
            
                RemoveWeightRequire2(updateCommand, id);
            }


        }


        public static void Exec(ParamProject paramProject, UpdateCommand updateCommand)
        {

            if (!updateCommand.HaveOption(UpdateParamOptionNames.RemoveRequire))
            {
                return;
            }
            UpdateLogger.InfoTime("===ParamUpdateRequire.Exec");

            ProcWeapon(paramProject, updateCommand);
            ProcMagic(paramProject, updateCommand);

        }

        public static void ProcWeapon(ParamProject? paramProject, UpdateCommand updateCommand)
        {

            if (paramProject == null)
                return;

            SoulsParam.Param? param = paramProject.FindParam(ParamNames.EquipParamWeapon);

            if (param == null)
            {
                return;
            }

            for (int i = 0; i < param.Rows.Count; i++)
            {

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



        public static void RemoveWeightRequire(UpdateCommand updateCommand, int eqId)
        {
            SoulsParam.Param? param = updateCommand.GetProject().FindParam(ParamNames.EquipParamWeapon);

            if (param == null)
            {
                return;
            }
            var row = ParamRowUtils.FindRow(param, eqId);
            if (row != null)
            {
                RemoveWeightRequire(updateCommand, row);
            }
        }
        public static void RemoveWeightRequire(UpdateCommand updateCommand, SoulsParam.Param.Row row)
        {
            string[] keys = { "properStrength", "properAgility",
                    "properMagic", "properFaith","properLuck"};
            foreach (var key in keys)
            {
                updateCommand.AddItem(row, key, "0");
            }
            updateCommand.AddItem(row, "weight", 1);
        }

        static void RemoveWeightRequire2(UpdateCommand updateCommand, int id)
        {
            string[] keys = { "properStrength", "properAgility",
                    "properMagic", "properFaith","properLuck"};
            foreach (var key in keys)
            {
                updateCommand.AddItem(ParamNames.EquipParamWeapon, id, key, "0");
            }
            updateCommand.AddItem(ParamNames.EquipParamWeapon, id, "weight", 1);
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
                if (!SpecEquipConfig.IsMagic(row.ID, EquipType.Good))
                {
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

    class ParamRemoveWeight
    {



        private static void RemoveWeight(SoulsParam.Param? param, UpdateCommand updateCommand)
        {

            if (param == null)
            {
                return;
            }

            UpdateLogger.InfoTime("RemoveWeight {0}", param.Name);

            for (int i = 0; i < param.Rows.Count; i++)
            {


                SoulsParam.Param.Row row = param.Rows[i];
                if (row.ID < 4000)
                    continue;
                UpdateCommandItem item = UpdateCommandItem.Create(row, "weight", "1");
                updateCommand.AddItem(item);

            }
        }

        private static void ProcProtector(ParamProject? paramProject, UpdateCommand updateCommand)
        {

            if (paramProject == null)
                return;

            SoulsParam.Param? param = paramProject.FindParam(ParamNames.EquipParamProtector);

            //UpdateLogger.Info("RemoveWeight ProcProtector");

            RemoveWeight(param, updateCommand);
        }

        private static void ProcWeapon(ParamProject? paramProject, UpdateCommand updateCommand)
        {

            if (paramProject == null)
                return;

            SoulsParam.Param? param = paramProject.FindParam(ParamNames.EquipParamWeapon);

            //UpdateLogger.Info("RemoveWeight ProcWeapon");
            RemoveWeight(param, updateCommand);

        }

        internal static void Exec(ParamProject project, UpdateCommand updateCommand)
        {
            if (!updateCommand.HaveOption(UpdateParamOptionNames.RemoveWeight))
            {
                return;
            }
            UpdateLogger.InfoTime("===ParamRemoveWeight.Exec");

            ProcWeapon(project, updateCommand);
            ProcProtector(project, updateCommand);
        }
    }



}
