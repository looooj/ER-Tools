using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
{


    public class SpecEquipConfig
    {



        static Dictionary<int, int> SpecGoodDict = new();

        public static int GetSpec(int equipId)
        {

            if (SpecGoodDict.ContainsKey(equipId))
            {
                return SpecGoodDict[equipId];
            }
            return 0;
        }


        public static void AddSpec(int lotCount, params int[] equipIds)
        {
            for (int i = 0; i < equipIds.Length; i++)
            {
                int equipId = equipIds[i];
                SpecGoodDict.Remove(equipId);
                SpecGoodDict.Add(equipId, lotCount);
            }
        }

        /*
         
            //1210 Exalted Flesh
            //20691 Arteria Leaf
            //20651 Trina's Lily
            //20652 Fulgurbloom
            //20653 Miquella's Lily
            //1190 Silver - Pickled Fowl Foot
            //1200 Gold - Pickled Fowl Foot
            //190 Rune Arc
            //20723 Bloodrose
            //20801 Aeonian Butterfly
            //1235 Raw Meat Dumpling
            //9500 Cracked Pot
            //9501 Ritual Pot
            //9510 Perfume Bottle
            //10010 Golden Seed
            //10020 Sacred Tear
            //10030 Memory Stone
            //10040 Talisman Pouch
            //10060 Dragon Heart
            //8000 Stonesword Key
            //1290 Starlight Shards
            //10070 Lost Ashes of War        
            // 3350
            // 20751
            //15340
            //15341
         */
        public static void AddDefault()
        {

            AddSpec(99, new int[] {190, 1190, 1200, 1210, 1235,15341,15340,15430,
                1190, 20723, 20801, 20691, 20651, 20652, 20653,20751,
                3350 });


            AddSpec(20, new int[] { 8000, 10060, 1290 });


            AddSpec(20,
                new int[] { 9500, 9501, 9510,820,830});

            AddSpec(10, new int[] { 10010, 10020, 10070 });


            AddSpec(4, new int[] { 10030, 10040 });

        }


        public static bool IsRune(int itemId)
        {
            if (itemId >= 2900 && itemId <= 2919)
                return true;
            return false;
        }

        public static bool IsSmithingStone(int itemId)
        {

            if (itemId >= 10100 && itemId <= 10200)
                return true;
            return false;
        }

        public static bool IsGlovewort(int itemId)
        {

            if (itemId >= 10900 && itemId <= 10919)
                return true;
            return false;
        }

        public static bool IsBoluses(int itemId) {
            if (itemId >= 900 && itemId <= 960)
                return true;
            return false;

        }
        //
        public static bool IsAromatic(int itemId)
        {
            if (itemId >= 3500 && itemId <= 3580)
                return true;
            return false;

        }


        public static bool IsArrow(int itemId)
        {

            if (itemId >= 50000000 && itemId <= 53030000)
            {
                return true;
            }
            return false;
        }

        public static bool IsPot(int itemId)
        {

            if (itemId >= 300 && itemId < 700)
            {
                return true;
            }
            return false;
        }

        public static bool IsMagic(int itemId)
        {

            if (itemId >= 4000 && itemId <= 8001)
            {
                return true;
            }
            return false;
        }

        //for mod cer
        public static bool IsRemnant(int itemId)
        {

            if (itemId >= 20950 && itemId <= 20956)
                return true;
            return false;

        }

        //4000


        public static void LoadConfig()
        {
            AddDefault();


        }

    }
}
