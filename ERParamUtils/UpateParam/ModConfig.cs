using ERParamUtils.UpdateParam;
using SoulsFormats.KF4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
{
    public class ModConfig
    {
        public static string GetModTypeNames() {

            return "STD,RAND,CER";
        }

        public static List<string> GetModTypeNameList() { 
        
            var list = new List<string>();
            var items = GetModTypeNames().Split(',');
            foreach (var item in items)
            {
                list.Add(item);
            }
            return list;
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

        public static void SetModType(ModConfig.ModType v) { 
            currentModType=v;
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
            if (currentModType == ModType.STD)
                return true;

            return false;
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
            return 12;            
        }

        public static string GetCharaClassNames() {

            switch (currentModType)
            {
                case ModType.CER:
                    return "chara-names-cer.txt";
            }
            return "chara-names.txt";
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

        internal static bool CanSetupSomber()
        {
            if (currentModType == ModType.CER)
                return false;

            return true;
        }

        

        /*
        public static RowIdRange GetKaleId() {

            var ret = new RowIdRange(100500, 100524);

            return ret;            
        }
        */

        //public static RowIdRange GetSuperMerchantId() {
        //    var ret = new RowIdRange(810000, 810400);
        //    return ret;
        //}


        public static string GetDefaultModTypeName()
        {
            return "STD";
        }

        /*
        internal static string GetSuperMerchantFile()
        {
            switch (currentModType) { 
                case ModType.CER:
                    return "super-merchant-cer.txt"; ;
            }
            return "super-merchant.txt";
        }



        internal static string GetShopKaleFile()
        {
            switch (currentModType)
            {
                case ModType.CER:
                    return "shop-kale-cer.txt"; ;
            }
            return "shop-kale.txt";
        }

        internal static string GetShopTaleFile()
        {
            switch (currentModType)
            {
                case ModType.CER:
                    return "shop-table-cer.txt"; ;
            }
            return "shop-table.txt";
        }
        */
        public static string GetShopFile(string fn)
        {
            switch (currentModType)
            {
                case ModType.CER:
                    return fn + "-cer.txt"; ;
            }
            return fn + ".txt";
        }

        public enum ModType { 
            STD = 0,
            RAND = 1,
            CER = 2
        }

    }
}
