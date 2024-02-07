using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class ParamNames
    {
        public static readonly string BonfireWarpParam = "BonfireWarpParam";
        public static readonly string ShopLineupParam = "ShopLineupParam";
        public static readonly string ShopLineupParamRecipe = "ShopLineupParam_Recipe";
        public static readonly string ItemLotParamMap = "ItemLotParam_map";
        public static readonly string ItemLotParamEnemy = "ItemLotParam_enemy";
        public static readonly string CharaInitParam = "CharaInitParam";

        public static readonly string SpEffectParam = "SpEffectParam";
        public static readonly string SpEffectSetParam = "SpEffectSetParam";

        public static readonly string BuddyStoneParam = "BuddyStoneParam";
        public static readonly string BuddyParam = "BuddyParam";

        public static readonly string MagicParam = "Magic";
        //
        public static readonly string EquipMtrlSetParam = "EquipMtrlSetParam";
        public static readonly string EquipParamProtector = "EquipParamProtector";
        public static readonly string EquipParamWeapon = "EquipParamWeapon";
        public static readonly string EquipParamGoods = "EquipParamGoods";
        public static readonly string EquipParamGem = "EquipParamGem";
        public static readonly string EquipParamAccessory = "EquipParamAccessory";
        public static readonly string PlayerCommonParam = "PlayerCommonParam";
        public static readonly string NpcParam = "NpcParam";
        //
        public static readonly string WorldMapPointParam = "WorldMapPointParam";
        public static readonly string MapDefaultInfoParam = "MapDefaultInfoParam";

        internal static bool CheckValid(string paramName)
        {
            if (paramName == null)
                return false;
            if (paramName.Length < 3)
                return false;
            return true;
        }

        static Dictionary<string, string> paramNameDescMap = new();

        public static string? GetDesc(string paramName) {
            if (paramNameDescMap.ContainsKey(paramName))
                return paramNameDescMap[paramName];
            return null;
        } 
        public static void LoadDesc(string lang) {

            paramNameDescMap.Clear();
            //docs\param - define\param - names - zho.txt
            //docs\params-info\param-name-desc-zho.txt
            string fn = GlobalConfig.BaseDir + @"\docs\params-info\param-name-desc-" + lang + ".txt";
            if (!File.Exists(fn)) {
                return;
            }

            var lines = File.ReadAllLines(fn);
            foreach (string line in lines) {

                var items = line.Trim().Split(";");
                if (items.Length < 2)
                    continue;
                var key = items[0].Trim();
                var val = items[1].Trim();
                if (key.Length < 1 || val.Length < 1)
                    continue;
                paramNameDescMap.TryAdd(key, val);
            }

        }
    }
}
