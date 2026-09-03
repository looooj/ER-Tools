using ERParamUtils.UpdateParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
{
    public class UpdateSpEffect
    {

        //
        //312310 [Talisman] Shard of Alexander  312300 [Talisman] Warrior Jar Shard
        public static void PatchShard(UpdateCommand updateCommand)
        {
            //physicsAttackRate 1.15 1.1
            //magicAttackRate 1.15 1.1
            //fireAttackRate 1.15 1.1
            //thunderAttackRate 1.15 1.1
            //darkAttackRate 1.15 1.1
            var rowIds = "312310;312300";
            var keys = new string[] { "physicsAttackRate", "magicAttackRate",
             "fireAttackRate","thunderAttackRate","darkAttackRate"};
            foreach (var key in keys)
            {
                AddKeyValues(updateCommand, rowIds, key + ";1.15");
            }
        }

        public static void PatchHpMp(UpdateCommand updateCommand)
        {
            if (updateCommand.HaveOption(UpdateParamOptionNames.CrimsonAmberMedallionRestore))
            {
                return;
            }

            updateCommand.AddItem(ParamNames.SpEffectParam, 350301, "changeMpPoint", "-5");

            //350200[Talisman] Blessed Dew Talisman
            //updateCommand.AddItem(ParamNames.SpEffectParam, 350200, "changeHpPoint", "-5");
            //updateCommand.AddItem(ParamNames.SpEffectParam, 350200, "changeMpPoint", "-2");
            AddKeyValues(updateCommand, "350200", "changeHpPoint;-5;changeMpPoint;-2");

            updateCommand.AddItem(ParamNames.SpEffectParam, 20380000, "changeHpPoint", "-5");

            //5071101[Weapon] Serpent-God's Curved Sword (On Enemy Kill)
            updateCommand.AddItem(ParamNames.SpEffectParam, 5071101, "changeMpPoint", "-5");

            //5141101[Weapon] Sacrificial Axe (On Enemy Kill)
            updateCommand.AddItem(ParamNames.SpEffectParam, 5141101, "changeHpPoint", "-25");

            //5120101[Weapon] Greathorn Hammer (On Enemy Kill)
            updateCommand.AddItem(ParamNames.SpEffectParam, 5120101, "changeMpPoint", "-5");

            //5031401[Weapon] Blasphemous Blade (On Enemy Kill)
            updateCommand.AddItem(ParamNames.SpEffectParam, 5031401, "changeMpPoint", "-5");

            //5321400[Weapon] Icon Shield -Grant HP Restoration
            //updateCommand.AddItem(ParamNames.SpEffectParam, 5321400, "changeHpPoint", "-5");
            //updateCommand.AddItem(ParamNames.SpEffectParam, 5321400, "changeMpPoint", "-2");
            AddKeyValues(updateCommand, "5321400", "changeHpPoint;-5;changeMpPoint;-2");

        }

        static void AddKeyValues(UpdateCommand updateCommand, string rowIds, string keyValues)
        {

            var rowIds2 = rowIds.Split(";");
            foreach (var rowIdStr in rowIds2)
            {
                var keyValues2 = keyValues.Split(";");
                for (int i = 0; i < keyValues2.Length; i += 2)
                {
                    var key = keyValues2[i];
                    if (i + 1 >= keyValues2.Length)
                    {
                        break;
                    }
                    var value = keyValues2[i + 1];
                    var rowId = int.Parse(rowIdStr);
                    updateCommand.AddItem(ParamNames.SpEffectParam, rowId, key, value);
                }
            }

        }

    }

    }
