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

            return "None,UnlockAll(exclude boss grace),UnlockCustom(custom_grace.txt)";
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
            return "0,1,2,3"; 
        }

        public static string GetNameList()
        {
            
            return "NotChange,Golden Rune [13](10000),Hero's Rune [2](20000),Hero's Rune [4](30000)";
        }

        /*
2912;Golden Rune [13];黄金卢恩【１３】
2915;Hero's Rune [2];英雄卢恩【２】
2917;Hero's Rune [4];英雄卢恩【４】
         */
        public static int ValueToEquipId(int value)
        {
            switch (value) { 
                case 1: 
                    return 2912;
                case 2:
                    return 2915;
                case 3:
                    return 2917;
            }
            return 0;
        }

        public static int GetRuneValue(int id) {
            switch (id) {
                case 2912:
                    return 10000;
                case 2915:
                    return 20000;
                case 2917:
                    return 30000;
                case 2919:
                    return 50000;
                case 2002951:
                    return 500;
                case 2002952:
                    return 7500;
                case 2002953:
                    return 10000;
                case 2002954:
                    return 12500;
                case 2002955:
                    return 15000;
                case 2002956:
                    return 17500;
                case 2002957:
                    return 22500;
                case 2002958:
                    return 30000;
                case 2002959:
                    return 50000;
                case 2002960:
                    return 80000;
            }
            return 100;
        }
        static string currentValue = "1";

        public static string GetValue() { 
            return currentValue;
        }

    }

    public class UpdateParamOptionItem {

        public string Name;
        public string Description;
        public int index;
        public UpdateParamOptionItem(string name)
        {
            Name = name;
            Description = name;
        }

        public UpdateParamOptionItem(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }

    public class UpdateParamOptionNames
    {

        public static readonly string UnlockCrafting = "UnlockCrafting";
        public static readonly string Buddy = "Buddy";
        public static readonly string RemoveWeight = "RemoveWeight";
        public static readonly string RemoveRequire = "RemoveRequire";
        public static readonly string UpdateShop = "UpdateShop";


        public static readonly string AllTalisman = "AllTalisman";
        public static readonly string CrimsonAmberMedallionBase = "CrimsonAmberMedallionBase";
        public static readonly string CrimsonAmberMedallionDisable = "CrimsonAmberMedallionDisable";
        public static readonly string CrimsonAmberMedallionRestore = "CrimsonAmberMedallionRestore";

        public static readonly string CrimsonAmberMedallionAttackRate = "CrimsonAmberMedallionAttackRate";
        public static readonly string CrimsonAmberMedallionConsumptionRate = "CrimsonAmberMedallionConsumptionRate";
        public static readonly string CrimsonAmberMedallionDamageCorrectRate = "CrimsonAmberMedallionDamageCorrectRate";

        public static readonly string AddInitCrimsonAmberMedallion = "AddInitCrimsonAmberMedallion";

        public static readonly string AddInit99Rune = "AddInit99Rune";
        //207010 MimicTear Ashes +10
        public static readonly string AddInitMimicTear = "AddInitMimicTear";
        //Pureblood Knight's Meda
        public static readonly string AddInitPurebloodKnightMeda = "AddInitPurebloodKnightMeda";
        public static readonly string ReplaceInitBow = "ReplaceInitBow";
        public static readonly string ReplaceInitWeaponRight3 = "ReplaceInitWeaponRight3";
        public static readonly string ReplaceInitWeaponRight2 = "ReplaceInitWeaponRight2";
        //public static string GetReplaceIntelligenceWeaponIds()
        //
        public static readonly string ReplaceInitIntelligenceWeapon = "ReplaceInitIntelligenceWeapon";
        public static readonly string ReplaceInitDexterityWeapon = "ReplaceInitDexterityWeapon";

        public static readonly string RemoveInitWeaponWeightRequire = "RemoveInitWeaponWeightRequire";

        public static readonly string UnlockGrace = "UnlockGrace";
        public static readonly string UnlockRoundtableHold = "UnlockRoundtableHold";

        public static readonly string ReplaceGoldenSeedSacredTear = "ReplaceGoldenSeedSacredTear";
        public static readonly string ReplaceScadutreeFragmentSpiritAsh = "ReplaceScadutreeFragmentSpiritAsh";
        public static readonly string ReplaceTalismanPouch = "ReplaceTalismanPouch";
        public static readonly string ReplaceFinger = "ReplaceFinger";
        public static readonly string ReplaceCookbook = "ReplaceCookbook";
        public static readonly string ReplaceDeathroot = "ReplaceDeathroot";
        public static readonly string ReplaceRune = "ReplaceRune";
        public static readonly string ReplaceRuneCount = "ReplaceRuneCount";

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


        public static readonly string ReplaceMapPiece = "ReplaceMapPiece";

        public static readonly string ReplaceWhetblade = "ReplaceWhetblade";

        //public static readonly string RemoveRemembranceRequire = "RemoveRemembranceRequire";
        //public static readonly string RemoveRecipe = "AddWhetblade";

        public static readonly string GetRuneRate = "GetRuneRate";

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
                int value = config.GetInt(key,0);
                UpdateCommandOptions[key]=value;
            }
        }

        public void AddUpdateCommandOption(string name)
        {
            UpdateCommandOptions[name]=1;
        }

        public void AddUpdateCommandOption(string name, int val)
        {
            UpdateCommandOptions[name]=val;
        }

    }


}
