﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class ParamNameFilter
    {

        static string[] includeParamNames1 = new string[] {
             "ItemLotParam_enemy",
             "ItemLotParam_map",
             "SpEffectParam",
             "ShopLineupParam",
             "BuddyParam",
             "BuddyStoneParam",
             "EquipParamWeapon",
             "EquipParamAccessory",
             "EquipParamGem",
             "EquipParamGoods",
             "EquipParamProtector",
             "Magic",
             "CharaInitParam",
             //"EquipMtrlSetParam",
             "NpcParam",
             "WorldMapPointParam",
             "WorldMapPlaceNameParam",
             "WorldMapPieceParam",
             "GameAreaParam",
        };

        static string[] defaultIncludeParamNames = new string[] {
             //"Bullet",
             //"AtkParam_Npc",
             "AtkParam_Pc",
             "BonfireWarpParam",
             "ItemLotParam_enemy",
             "ItemLotParam_map",
             "SpEffectParam",
             //"SpEffectSetParam",
             "ShopLineupParam",
             "ShopLineupParam_Recipe",
             "CharaInitParam",
             "WorldMapPointParam",
             "WorldMapPlaceNameParam",
             "WorldMapPieceParam",
             //"WorldMapLegacyConvParam",
             //"GameAreaParam",
             //"GameSystemCommonParam",
             "BuddyParam",
             "BuddyStoneParam",
             "EquipParamWeapon",
             "EquipParamAccessory",
             "EquipParamGem",
             "EquipParamGoods",
             "EquipParamProtector",
             "Magic",
             //"NpcAiActionParam",
             //"NpcAiBehaviorProbability",
             "NpcParam",
             "NpcThinkParam",
             "ObjActParam",
             "ActionButtonParam",
             //"CharMakeMenuListItemParam",
             //"CharMakeMenuTopParam",
             "BehaviorParam",
             "BehaviorParam_PC",
             //"TalkParam",
             "EquipMtrlSetParam",
             "PlayerCommonParam",
             "SwordArtsParam",
             "EquipParamCustomWeapon"
             //"MapDefaultInfoParam"
        };

        static string[]? includeParamNames = null;


        static public void LoadParamNames() {

            string fn = GlobalConfig.BaseDir + "\\param-names.txt";
            var result = File.ReadAllLines(fn);
            if ( result.Length > 1) {
                includeParamNames = result;
                return;
            }
            includeParamNames = defaultIncludeParamNames;
        }
        static public bool IncludesParam(string paramName)
        {
            if (includeParamNames == null)
                includeParamNames = defaultIncludeParamNames;

            foreach (string s in includeParamNames)
            {
                if (s == paramName)
                    return true;
            }
            return false;
        }

    }
}
