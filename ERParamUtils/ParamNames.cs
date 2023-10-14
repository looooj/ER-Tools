﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class ParamNames
    {
        public static readonly string ShopLineupParam = "ShopLineupParam";
        public static readonly string ItemLotParamMap = "ItemLotParam_map";
        public static readonly string ItemLotParamEnemy = "ItemLotParam_enemy";
        public static readonly string CharaInitParam = "CharaInitParam";
        public static readonly string SpEffectParam = "SpEffectParam";
        public static readonly string BuddyStoneParam = "BuddyStoneParam";

        public static readonly string MagicParam = "Magic";
        public static readonly string EquipParamProtector = "EquipParamProtector";
        public static readonly string EquipParamWeapon = "EquipParamWeapon";
        public static readonly string EquipParamGoods = "EquipParamGoods";
      

        internal static bool CheckValid(string paramName)
        {
            if (paramName == null)
                return false;
            if (paramName.Length < 3)
                return false;
            return true;
        }
    }
}