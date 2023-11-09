using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public enum EquipType
    {
        None,
        Good,
        Weapon,
        Protector,
        Accessory,
        Gem,
        U1
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

        public static string? ShopTypeParamName(ShopEquipType shopEquipType) {

            switch (shopEquipType) {
                case ShopEquipType.Weapon:
                    return ParamNames.EquipParamWeapon;
                case ShopEquipType.Protector:
                    return ParamNames.EquipParamProtector;
                case ShopEquipType.Good:
                    return ParamNames.EquipParamGoods;
                case ShopEquipType.Accessory:
                    return ParamNames.EquipParamAccessory;
                case ShopEquipType.Ash:
                    return ParamNames.EquipParamGem;
            }
            return null;
        }
    }
}
