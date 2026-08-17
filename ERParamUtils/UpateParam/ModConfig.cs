using ERParamUtils.UpdateParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
{
    public class ModConfig
    {
        public static string GetModTypeNames() {

            return "STD,RAND,CER";
        }

        public static string GetModTypeIds()
        {
            return "0,1,2";
        }

        static ModType currentModType = ModType.STD;
        public static void SetModType(UpdateCommand updateCommand) {
            if (updateCommand.HaveOption(UpdateParamOptionNames.ModType)) { 

                var v = updateCommand.GetOption(UpdateParamOptionNames.ModType);
                currentModType = (ModType)v;
            }
            
        }

        public static bool AddWhetblade()
        {
            if (currentModType == ModType.CER)
                return false;
             return true;            
        }

        public static bool InitAccSlot() {
            if (currentModType == ModType.CER)
                return false;

            return true;
        }

        public static bool AddMapPiece() {
            if (currentModType == ModType.CER)
                return false;

            return true;
        }

        public static bool EnhanceBuddy() {
            if (currentModType == ModType.CER)
                return false;

            return true;
        }

        public static bool UnlockRoundtableHold()
        {
            if (currentModType == ModType.CER)
                return false;

            return true;
        }

        internal static ModType GetModType()
        {
            return currentModType;
        }

        internal static int GetCharaClassCount()
        {
            switch (currentModType)
            {
                case ModType.CER:
                    return 27;
            }
            return 10;
            
        }

        public static string GetUnlockGraceEventId() {
            //        //static string eventflagIdValue = "71190";//"76101";//71801

            switch (currentModType)
            {
                case ModType.CER:
                    return "71190";
            }

            return "71801";

        }

        public static string GetUnlockGraceName()
        {
            switch (currentModType)
            {
                case ModType.CER:
                    return "unlock_grace_all_cer.txt";
            }
            return "unlock_grace_all.txt";

        }



        public static string GetUnlockGraceSkipName()
        {
            switch (currentModType)
            {
                case ModType.CER:
                    return "unlock_grace_skip_cer.txt";
            }
            return "unlock_grace_skip.txt";

        }

        public static string GetUnlockGraceSkipName(ParamProject paramProject)
        {
            return "";
        }

        public static string GetUnlockGraceName(ParamProject paramProject)
        {
            return "";
        }


        public enum ModType { 
            STD = 0,
            RADN = 1,
            CER = 2
        }

    }
}
