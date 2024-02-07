using NLog.LayoutRenderers.Wrappers;
using SoulsParam;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpdateParam
{
    class RecipeUpdateItem
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public int EquipId;
        public int EquipType;
        public int EventFlagForStock;
        public int RowId;
        public string Name="";
        public bool Parse(string line)
        {

            line = line.Trim();
            if (line.StartsWith("#"))
                return false;


            string[] items = line.Split(';');
            if (items.Length < 2)
                return false;

            EquipId = Int32.Parse(items[0]);
            EquipType = Int32.Parse(items[1]);
            EventFlagForStock = 0;
            if (items.Length >= 3)
                EventFlagForStock = Int32.Parse(items[2]);
            if (items.Length >= 4)
                Name = items[3].Trim();
            return true;
        }

        internal static void Proc(RecipeUpdateItem item, Param param, ParamProject paramProject, UpdateCommand updateCommand)
        {
            UpdateShopLineupParamRecipe.AddEquip(updateCommand, item.RowId,
                param,
                item.EquipId,
                item.EquipType, item.Name, item.EventFlagForStock);

        }
    }

    public class UpdateShopLineupParamRecipe
    {

        static int recipeBaseRowId = 31000;
        static Dictionary<string, int> eventFlagForStockMap = new();
        public static void Init() {
            recipeBaseRowId = 31000;

            string fn = GlobalConfig.BaseDir + "\\docs\\recipe-param\\eventFlag_forStock.txt";
            string[] lines = File.ReadAllLines(fn);
            foreach (string line in lines) {

                var tmp = line.Trim();
                var items = tmp.Split(",");
                //31602,62010,8600,3
                if (items.Length == 4) {
                    var key = items[2] + "_" + items[3];
                    var val = items[1].Trim();
                    if (val.Length < 1)
                        continue;
                    eventFlagForStockMap.TryAdd(key, int.Parse(val));
                }
            }



        }

        public static void UnlockCrafting(ParamProject paramProject, UpdateCommand updateCommand)
        {

            var param = paramProject.FindParam(ParamNames.ShopLineupParamRecipe);
            if (param == null)
                return;

            for (int i = 0; i < param.Rows.Count; i++)
            {

                var row = param.Rows[i];

                string key = "eventFlag_forRelease";

                if (ParamRowUtils.GetCellInt(row, key, 0) != 0)
                {
                    updateCommand.AddItem(row, key, "0");
                }
            }            
        }

        public static void RemoveRequire(ParamProject paramProject, UpdateCommand updateCommand)
        {

            var param = paramProject.FindParam(ParamNames.ShopLineupParamRecipe);
            if (param == null)
                return;

            for (int i = 0; i < param.Rows.Count; i++)
            {

                var row = param.Rows[i];

                string key = "mtrlId";

                if (ParamRowUtils.GetCellInt(row, key, 0) > 0)
                {
                    updateCommand.AddItem(row, key, "0");
                }
            }
        }


        public static bool AddEquip(UpdateCommand updateCommand, int rowId,
             SoulsParam.Param param, int equipId, int equipType, string name, int eventFlagForStock1)

        {
            if (name.Length < 1)
                name = "_" + equipId;

            var key = equipId + "_" + equipType;
            int eventFlagForStock = eventFlagForStock1;
            if (eventFlagForStock == 0 )
               if (eventFlagForStockMap.TryGetValue(key, out int eventFlagForStock2)) {
                    eventFlagForStock = eventFlagForStock2;
            }

            var row = param.InsertRow(rowId, name);
            if (row == null)
                return false;

            var keyValues =
                "value,0," +
                "mtrlId,-1," +
                "eventFlag_forRelease,0," +
                "eventFlag_forStock," + eventFlagForStock + "," +
                "sellQuantity,-1," +
                //"equipId," + equipId + "," +
                "equipType," + equipType + "," +
                "costType,0," +
                "setNum,1," +
                "value_Add,0," +
                "value_Magnification,1," +
                "iconId,-1," +
                "nameMsgId,-1," +
                "menuTitleMsgId,-1," +
                "menuIconId,-1";

            ParamRowUtils.SetCellValue(row, keyValues);
            updateCommand.AddItem(row, "equipId", equipId);
            return true;
        }
/*
        public static void AddGood(UpdateCommand updateCommand,
            SoulsParam.Param param, int beginEquipId, int endEquipId, string name, int beginRowId, int beginStockId)
        {

            for (int equipId = beginEquipId; equipId <= endEquipId; equipId++)
            {

                int offset = equipId - beginEquipId;
                int rowId = beginRowId + offset;
                int eventFlag_forStock = beginStockId + offset;
                var row = param.InsertRow(rowId, name + "-" + equipId);
                if (row == null)
                    continue;
                var keyValues =
                    "value,0," +
                    "mtrlId,-1," +
                    "eventFlag_forRelease,0," +
                    "eventFlag_forStock,0," +
                    "sellQuantity,-1," +
                    "equipType,3," +
                    "costType,0," +
                    "setNum,1," +
                    "value_Add,0," +
                    "value_Magnification,1," +
                    "iconId,-1," +
                    "nameMsgId,-1," +
                    "menuTitleMsgId,-1," +
                    "menuIconId,-1";

                ParamRowUtils.SetCellValue(row, keyValues);
                ParamRowUtils.SetCellValue(row, "eventFlag_forStock", eventFlag_forStock);
                updateCommand.AddItem(row, "equipId", equipId);
            }
        }

        //? 
        public static void AddWhetblade(ParamProject paramProject, UpdateCommand updateCommand)
        {
            var param = paramProject.FindParam(ParamNames.ShopLineupParamRecipe);
            if (param == null)
                return;
            AddGood(updateCommand, param, 8970, 8974, "Whetblade", 30151, 65610);

        }
*/

        public static void AddMapPiece(ParamProject paramProject, UpdateCommand updateCommand)
        {
            var param = paramProject.FindParam(ParamNames.ShopLineupParamRecipe);
            if (param == null)
                return;

            for (int equipId = 8600; equipId <= 8618; equipId++) {
                recipeBaseRowId++;
                AddEquip(updateCommand, recipeBaseRowId, param, equipId, (int)ShopEquipType.Good, "", 0);

            }
        }

        public static void ExecSpec(ParamProject paramProject, UpdateCommand updateCommand)
        {

            UpdateLogger.Begin(ParamNames.ShopLineupParamRecipe);

            var param = paramProject.FindParam(ParamNames.ShopLineupParamRecipe);
            if (param == null)
                return;

            var lines = UpdateFile.Load(paramProject, UpdateFile.UpdateRecipeSpec);

            foreach (string line in lines)
            {
                var item = new RecipeUpdateItem();
                if (item.Parse(line))
                {
                    recipeBaseRowId++;
                    item.RowId = recipeBaseRowId;
                    RecipeUpdateItem.Proc(item, param, paramProject, updateCommand);
                }
            }

        }
    }

}
