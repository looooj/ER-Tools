using ERParamUtils.UpdateParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
{
    public class UpdateTalisman
    {

        public static void Exec(ParamProject? paramProject, UpdateCommand updateCommand) {

            
            BuildSuperCrimsonAmberMedallion(paramProject, updateCommand);
            if (updateCommand.HaveOption(UpdateParamOptionNames.AllTalisman))
            {
                BuildAll(paramProject, updateCommand);
            }

            PatchHpMp(paramProject, updateCommand);
        }

        private static void PatchHpMp(ParamProject? paramProject, UpdateCommand updateCommand) {


            if (updateCommand.HaveOption(UpdateParamOptionNames.CrimsonAmberMedallionRestore)) {
                return;
            }

            //350301 [Talisman] Taker's Cameo (On Enemy Kill)
            updateCommand.AddItem(ParamNames.SpEffectParam, 350301, "changeMpPoint", "-5");

            //350200[Talisman] Blessed Dew Talisman
            updateCommand.AddItem(ParamNames.SpEffectParam, 350200, "changeHpPoint", "-3");
            updateCommand.AddItem(ParamNames.SpEffectParam, 350200, "changeMpPoint", "-2");

            //5071101[Weapon] Serpent-God's Curved Sword (On Enemy Kill)
            updateCommand.AddItem(ParamNames.SpEffectParam, 5071101, "changeMpPoint", "-5");

            //5141101[Weapon] Sacrificial Axe (On Enemy Kill)
            updateCommand.AddItem(ParamNames.SpEffectParam, 5141101, "changeHpPoint", "-25");

            //5120101[Weapon] Greathorn Hammer (On Enemy Kill)
            updateCommand.AddItem(ParamNames.SpEffectParam, 5120101, "changeMpPoint", "-5");

            //5031401[Weapon] Blasphemous Blade (On Enemy Kill)
            updateCommand.AddItem(ParamNames.SpEffectParam, 5031401, "changeMpPoint", "-5");


        }

        private static void BuildAll(ParamProject? paramProject, UpdateCommand updateCommand)
        {
            if (paramProject == null)
            {
                return;
            }


            var param = paramProject.FindParam(ParamNames.EquipParamAccessory);
            if (param == null)
                return;

            List<int> spIdList = new List<int>();

            var rows = param.Rows;
            int cellIndex = rows[0].GetParam().GetCellIndex("refId");
            foreach (var row in rows)
            {
                if (row.Name == null || row.ID < 1000)
                    continue;

                var v = ParamRowUtils.GetCellInt(row, cellIndex, 0);
                if (v > 10000)
                {
                    spIdList.Add(v);

                    updateCommand.AddItem(ParamNames.SpEffectParam, v, "itemDropRate", "1.2");
                    updateCommand.AddItem(ParamNames.SpEffectParam, v, "maxHpRate", "1.2");
                    updateCommand.AddItem(ParamNames.SpEffectParam, v, "maxMpRate", "1.2");
                    updateCommand.AddItem(ParamNames.SpEffectParam, v, "maxStaminaRate", "1.2");
                    updateCommand.AddItem(ParamNames.SpEffectParam, v, "soulRate", "1.2");

                }
            }
        }

        /*
         Shard of Alexander
physicsAttackRate 1.15
magicAttackRate 1.15
fireAttackRate 1.15
thunderAttackRate 1.15
magicSubCategoryChange1  112
magicSubCategoryChange2  111
darkAttackRate 1.15        
         */

        public static void BuildSuperTalisman2(ParamProject? paramProject, UpdateCommand updateCommand)
        {

        }

        //CrimsonAmberMedallion

        public static List<UpdateParamOptionItem> GetUpdateParams() { 
        
            var list = new List<UpdateParamOptionItem>();
            list.Add(new UpdateParamOptionItem(UpdateParamOptionNames.CrimsonAmberMedallionBase));
            list.Add(new UpdateParamOptionItem(UpdateParamOptionNames.CrimsonAmberMedallionRestore));
            list.Add(new UpdateParamOptionItem(UpdateParamOptionNames.CrimsonAmberMedallionDisable));

            list.Add(new UpdateParamOptionItem(UpdateParamOptionNames.CrimsonAmberMedallionConsumptionRate));
            list.Add(new UpdateParamOptionItem(UpdateParamOptionNames.CrimsonAmberMedallionDamageCorrectRate));
            list.Add(new UpdateParamOptionItem(UpdateParamOptionNames.CrimsonAmberMedallionAttackRate));

            //list.Add(new UpdateParamOptionItem(UpdateParamOptionNames.AllTalisman));
            return list;
        }

        private static void AddKeysValue(UpdateCommand updateCommand,string paramName, int rowId, string[] keys, string value) {

            foreach (var key in keys) {

                updateCommand.AddItem(paramName, rowId, key,value);

            }
        }

        public static void BuildSuperCrimsonAmberMedallion(ParamProject? paramProject, UpdateCommand updateCommand) {

            //@@id = 310020,310010,310000
            // #SpEffectParam,Crimson Amber Medallion
            var ids = "310000".Split(",");
            List<int> rowIdList = new();
            for (var i = 0; i < ids.Length; i++) { 
                rowIdList.Add(int.Parse(ids[i]));
            }
            foreach (var rowId in rowIdList)
            {
                //stateInfo; Destroy Accessory but Save Runes(159)
                if (updateCommand.HaveOption(UpdateParamOptionNames.CrimsonAmberMedallionBase))
                {
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "maxHpRate", "1.5");
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "maxMpRate", "1.5");
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "maxStaminaRate", "1.5");
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "equipWeightChangeRate", "1.5");
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "itemDropRate", "5");
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "soulRate", "2");
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "hearingSearchEnemyRate", "0");
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "fallDamageRate", "0");
                    //updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "stateInfo", "159");
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "bowDistRate", "65");

                    string[] addStatusKey = {
                        "addStrengthStatus",
                        "addDexterityStatus",
                        "addMagicStatus",
                        "addFaithStatus",
                        "addLuckStatus"};

                    updateCommand.AddKeysItem(ParamNames.SpEffectParam,rowId,addStatusKey,"6");

                }
                

                if (updateCommand.HaveOption(UpdateParamOptionNames.CrimsonAmberMedallionRestore))
                {
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "motionInterval", "1");
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "changeHpPoint", "-5");
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "changeMpPoint", "-2");
                }

                if (updateCommand.HaveOption(UpdateParamOptionNames.CrimsonAmberMedallionDisable))
                {
                    string[] disableKeys = { "disableMadness", "disablePoison", "disableDisease",
                        "disableBlood","disableFreeze","disableSleep","disableCurse"};
                    updateCommand.AddKeysItem(ParamNames.SpEffectParam, rowId, disableKeys, "1");
                }

                if (updateCommand.HaveOption(UpdateParamOptionNames.CrimsonAmberMedallionDamageCorrectRate)) {

                    var v = "0.75";
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "defEnemyDmgCorrectRate_Physics", v);
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "defEnemyDmgCorrectRate_Magic", v);
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "defEnemyDmgCorrectRate_Fire", v);
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "defEnemyDmgCorrectRate_Thunder", v);
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "defEnemyDmgCorrectRate_Dark", v);
                }

                if (updateCommand.HaveOption(UpdateParamOptionNames.CrimsonAmberMedallionAttackRate))
                {
                    var v = "1.2";
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "physicsAttackRate", v);
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "magicAttackRate", v);
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "fireAttackRate", v);
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "thunderAttackRate", v);
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "darkAttackRate", v);
                }

                if (updateCommand.HaveOption(UpdateParamOptionNames.CrimsonAmberMedallionConsumptionRate))
                {
                    var v = "0.8";
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "artsConsumptionRate", v);
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "magicConsumptionRate", v);
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "shamanConsumptionRate", v);
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, "miracleConsumptionRate", v);
                }
            }

        }
    }
}

