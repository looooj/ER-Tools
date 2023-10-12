using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class ParamFilter
    {

        static string[] includeParamNames = new string[] {
             "ItemLotParam_enemy",
             "ItemLotParam_map",
             "SpEffectParam",
             "SpEffectSetParam",
             "ShopLineupParam",
             "CharaInitParam",
             "WorldMapPointParam",
             "BuddyParam",
             "BuddyStoneParam",
             "EquipParamWeapon",
             "EquipParamAccessory",
             "EquipParamGem",
             "EquipParamGoods",
             "EquipParamProtector",
             "Magic"

        };


        static public bool IncludesParam(string paramName)
        {

            foreach (string s in includeParamNames)
            {
                if (s == paramName)
                    return true;
            }
            return false;
        }

    }
}
