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
             "ShopLineupParam_Recipe",
             "CharaInitParam",
             "WorldMapPointParam",
             "WorldMapPlaceNameParam",
             "WorldMapPieceParam",
             "WorldMapLegacyConvParam",
             "GameAreaParam",
             "GameSystemCommonParam",
             "BuddyParam",
             "BuddyStoneParam",
             "EquipParamWeapon",
             "EquipParamAccessory",
             "EquipParamGem",
             "EquipParamGoods",
             "EquipParamProtector",
             "Magic",
             "NpcAiActionParam",
             "NpcAiBehaviorProbability",
             "NpcParam",
             "NpcThinkParam",
             "ObjActParam",
             "ActionButtonParam",
             "CharMakeMenuListItemParam",
             "CharMakeMenuTopParam",
             "BehaviorParam",
             "BehaviorParam_PC",
             "TalkParam",
             "EquipMtrlSetParam"
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
