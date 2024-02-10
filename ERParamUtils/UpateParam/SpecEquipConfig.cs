using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpdateParam
{


    public class SpecEquipConfig
    {



        static Dictionary<int, int> SpecGoodDict = new();

        public static int GetSpec(int equipId, EquipType equipType)
        {
            if (equipType != EquipType.Good)
                return 0;

            if (SpecGoodDict.ContainsKey(equipId))
            {
                return SpecGoodDict[equipId];
            }
            return 0;
        }
        //6070;Sacrificial Twig;牺牲细枝
        public static bool IsSacrificialTwig(int equipId, EquipType equipType) {
            return (equipId == 6070 && equipType == EquipType.Accessory);
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
            //1200 Gold - Pickled Fowl Foot
            //190 Rune Arc
            //20723 Bloodrose
            //9500 Cracked Pot
            //9501 Ritual Pot
            //9510 Perfume Bottle
            //10030 Memory Stone
            //10040 Talisman Pouch
            //10060 Dragon Heart
            //8000 Stonesword Key
            //1290 Starlight Shards
            //10070 Lost Ashes of War        
            //3350;Bewitching Branch;魅惑树枝
            //20751;Rimed Crystal Bud;冰结晶木芽
            //20825;Silver Tear Husk;银色泪滴空壳

         */
        public static void AddDefault()
        {

            //15060;Flight Pinion;拨风羽毛
            //15430;Stormhawk Feather;风暴鹰羽毛
            //15340; Thin Beast Bones; 细小兽骨
            //15341; Hefty Beast Bone; 粗大兽骨
            AddSpec(90, new int[] { 15060, 15430, 15340, 15341 });


            AddSpec(50, new int[] { 190, 1200, 1210, 
                20723, 20801, 20691, 20651, 20652, 20653,20751,20825,20775,20750,
                3350 });

            AddSpec(20, new int[] { 8000, 10060 });


            AddSpec(20,
                new int[] { 9500, 9510, 820, 830 });


            AddSpec(10, new int[] { 9501,10070, 8185, 8193, 2090 });


            AddSpec(4, new int[] { 10030, 10040, 8186, 1290 });


            AddSpec(2, new int[] { 20720 });

            AddSpec(1, new int[] { 10010,10020 });

        }

       //150;Furlcalling Finger Remedy;唤勾指药
       //100;Tarnished's Furled Finger;褪色者勾指
       //101; Duelist's Furled Finger;斗士勾指
       //170;Tarnished's Furled Finger;褪色者勾指
       //171;Duelist's Furled Finger;斗士勾指
        public static bool IsFinger(int itemId, EquipType equipType) {
            if (equipType != EquipType.Good)
                return false;

            if (itemId == 150
                || itemId == 100
                || itemId == 101
                || itemId == 170
                || itemId == 171
                )
            {
                return true;
            }
            return false;
        }

        public static bool IsRune(int itemId, EquipType equipType)
        {
            if (equipType != EquipType.Good)
                return false;
            if (itemId >= 2900 && itemId <= 2919)
                return true;
            return false;
        }

        public static bool IsSmithingStone(int itemId, EquipType equipType)
        {
            if (equipType != EquipType.Good)
                return false;

            if (itemId >= 10100 && itemId <= 10200)
                return true;
            return false;
        }

        public static bool IsGlovewort(int itemId, EquipType equipType)
        {
            if (equipType != EquipType.Good)
                return false;
            if (itemId >= 10900 && itemId <= 10919)
                return true;
            return false;
        }

        public static bool IsBoluses(int itemId, EquipType equipType)
        {
            if (equipType != EquipType.Good)
                return false;

            if (itemId >= 900 && itemId <= 960)
                return true;
            return false;

        }
        //
        public static bool IsAromatic(int itemId, EquipType equipType)
        {
            if (equipType != EquipType.Good)
                return false;

            if (itemId >= 3500 && itemId <= 3580)
                return true;
            return false;

        }

        //1100
        //2030
        public static bool IsMeat(int itemId, EquipType equipType)
        {
            if (equipType != EquipType.Good)
                return false;

            if (itemId >= 1100 && itemId <= 2030)
                return true;
            return false;
        }


        public static bool IsMaterial(int itemId, EquipType equipType)
        {
            if (equipType != EquipType.Good)
                return false;

            if (itemId >= 15000 && itemId <= 20855)
                return true;

            return false;

        }

        public static bool IsArrow(int itemId, EquipType equipType)
        {
            if (equipType != EquipType.Weapon)
                return false;
            if (itemId >= 50000000 && itemId <= 53030000)
            {
                return true;
            }
            return false;
        }

        public static bool IsPot(int itemId, EquipType equipType)
        {
            if (equipType != EquipType.Good)
                return false;
            if (itemId >= 300 && itemId < 700)
            {
                return true;
            }
            return false;
        }

        public static bool IsMagic(int itemId, EquipType equipType)
        {
            if (equipType != EquipType.Good)
                return false;
            if (itemId >= 4000 && itemId < 8000)
            {
                return true;
            }
            return false;
        }

        //for mod cer
        public static bool IsRemnant(int itemId, EquipType equipType)
        {
            if (equipType != EquipType.Good)
                return false;

            if (itemId >= 20950 && itemId <= 20956)
                return true;
            return false;

        }
        public static bool IsPhysickRemnant(int itemId, EquipType equipType)
        {
            if (equipType != EquipType.Good)
                return false;
            if (itemId >= 20900 && itemId <= 20954)
                return true;
            return false;

        }

        public static bool IsRemembrance(int itemId, EquipType equipType) {

            if (equipType != EquipType.Good)
                return false;

            if (itemId >= 2950 && itemId <= 2964)
                return true;

            return false;
        }

        public static bool IsCookBook(int itemId, EquipType equipType) {
            if (equipType != EquipType.Good)
                return false;

            if (itemId >= 9300 && itemId <= 9441)
                return true;

            return false;
        }

        public static void LoadConfig()
        {
            AddDefault();


        }

    }
}
