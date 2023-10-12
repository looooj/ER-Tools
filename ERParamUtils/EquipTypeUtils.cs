using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    /*
    public class EquipType
    {
        public static readonly int None = 0;
        public static readonly int Good = 1;
        public static readonly int Weapon = 2;
        public static readonly int Protector = 3;
        public static readonly int Accessory = 4;
        public static readonly int Gem = 5;


        
    };

    public static class ShopEquipType
    {
        public static const  Weapon = 0;
        public static readonly int Protector = 1;
        public static readonly int Accessory = 2;
        public static readonly int Good = 3;
        public static readonly int Ash = 4;
    };
    */
    public enum EquipType
    {
        None,
        Good,
        Weapon,
        Protector,
        Accessory,
        Gem

    };

    public enum ShopEquipType
    {
        None = -1,
        Weapon = 0,
        Protector = 1,
        Accessory = 2,
        Good = 3,
        Ash = 4
    };


    public class EquipTypeUtils
    {

        public static EquipType ConvertShopEquipType(ShopEquipType shopEquipType) {

            switch (shopEquipType) {
                case ShopEquipType.Weapon:
                    return EquipType.Weapon;
                case ShopEquipType.Good:
                    return EquipType.Good;
                case ShopEquipType.Ash:
                    return EquipType.Gem;
                case ShopEquipType.Protector:
                    return EquipType.Protector;
                case ShopEquipType.Accessory:
                    return EquipType.Accessory;
            }
            return EquipType.None;
        }
    }
}
