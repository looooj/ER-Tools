using ERParamUtils.UpdateParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
{
    public enum UnlockGraceType { 
        None = 0,
        UnlockNormal,
        UnlockCustom
        //UnlockForCER
    };

    public class UnlockGraceConfig {


        public static string GetValueList()
        {
            return "0,1,2"; 
        }

        public static string GetNameList()
        {

            return "None,UnlockNormal,UnlockCustom";
        }

        public static UnlockGraceType ValueToType(int v) { 

            
            switch (v) {
                case 1:
                    return UnlockGraceType.UnlockNormal;
                case 2:
                    return UnlockGraceType.UnlockCustom;

            }
            return UnlockGraceType.None;
        }
    };

    public class ReplaceGoldenRune {


        public static string GetValueList() {
            return "0,1,2,3"; //"NotChange,Golden Rune [13],Hero's Rune [4],Lord's Rune";
        }

        public static string GetNameList()
        {
            
            return "NotChange,Golden Rune [13],Hero's Rune [4],Lord's Rune";
        }

        /*
2912;Golden Rune [13];黄金卢恩【１３】
2917;Hero's Rune [4];英雄卢恩【４】
2919;Lord's Rune;王之卢恩
         */
        public static int ValueToEquipId(int value)
        {
            switch (value) { 
                case 1: 
                    return 2912;
                case 2:
                    return 2917;
                case 3:
                    return 2919;
            }
            return 0;
        }

        /*
        public static int ValueToEquipId(string value) {
            switch (value) {
                case "Golden Rune [13]":
                    return 2912;
                case "Hero's Rune [4]":
                    return 2917;
                case "Lord's Rune":
                    return 2919;
            }
            return 2912;
        }
        */
        static string currentValue = "1";

        public static string GetValue() { 
            return currentValue;
        }

    }

    public class UpdateParamOptionNames
    {

        public string Name;
        public string Description;
        public int index;
        public UpdateParamOptionNames(string name)
        {
            Name = name;
            Description = name;
        }

        public UpdateParamOptionNames(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public static readonly string UnlockGrace = "UnlockGrace";
        public static readonly string ReplaceGoldenSeedSacredTear = "ReplaceGoldenSeedSacredTear";
        public static readonly string ReplaceScadutreeFragmentSpiritAsh = "ReplaceScadutreeFragmentSpiritAsh";
        public static readonly string ReplaceTalismanPouch = "ReplaceTalismanPouch";
        public static readonly string ReplaceFinger = "ReplaceFinger";
        public static readonly string ReplaceCookbook = "ReplaceCookbook";
        public static readonly string ReplaceDeathroot = "ReplaceDeathroot";
        public static readonly string ReplaceRune = "ReplaceRune";
        public static readonly string ReplaceRuneArc = "ReplaceRuneArc";
        //public static readonly string ReplaceBolt = "ReplaceBolt";

        public static readonly string ReplaceBellBearing = "ReplaceBellBearing";
        public static readonly string ReplaceStoneswordKey = "ReplaceStoneswordKey";
        public static readonly string ReplaceMemoryStone = "ReplaceMemoryStone";
        public static readonly string ReplaceDragonHeart = "ReplaceDragonHeart";
        public static readonly string IncRemnant = "IncRemnant";
        public static readonly string ReplaceRemnant = "ReplaceRemnant";


        public static readonly string ReplaceGiantCrowSoul = "ReplaceGiantCrowSoul";
        public static readonly string ReplaceGoldenRune = "ReplaceGoldenRune";
        //public static readonly string DoubleGetSoul = "DoubleGetSoul";

        public static readonly string InitMagicSlotAccSlot = "InitMagicSlotAccSlot";


        public static readonly string AddMapPiece = "AddMapPiece";
        public static readonly string AddWhetblade = "AddWhetblade";

        //public static readonly string RemoveRemembranceRequire = "RemoveRemembranceRequire";
        //public static readonly string RemoveRecipe = "AddWhetblade";

        public static readonly string TimesGetSoul = "TimesGetSoul";

    }


    public class UpdateParamExecOptions
    {

        public bool Restore = true;
        public bool Publish = true;

        public List<UpdateParamTask> UpdateTasks = new();
        public Dictionary<string, int> UpdateCommandOptions = new();

        public void AddTask(UpdateParamTask task)
        {
            UpdateTasks.Add(task);
        }

        public void AddUpdateCommandOption(DictConfig config) {

            foreach (string key in config.GetDict().Keys) {
                int  value = config.GetInt(key,0);
                UpdateCommandOptions.TryAdd(key, value);
            }
        }

        public void AddUpdateCommandOption(string name)
        {
            UpdateCommandOptions.TryAdd(name, 1);
        }

        public void AddUpdateCommandOption(string name, int val)
        {
            UpdateCommandOptions.TryAdd(name, val);
        }

    }


}
