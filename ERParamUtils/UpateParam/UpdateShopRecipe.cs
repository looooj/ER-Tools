using SoulsParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
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
        public static void Init() {
            recipeBaseRowId = 31000;
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
            //AddTest(paramProject, updateCommand);
        }



        public static bool AddEquip(UpdateCommand updateCommand, int rowId,
             SoulsParam.Param param, int equipId, int equipType, string name, int eventFlagForStock)

        {
            if (name.Length < 1)
                name = "_" + equipId;

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
            int eventFlagForStock = 62010;
            for (int equipId = 8600; equipId <= 8618; equipId++) {
                recipeBaseRowId++;

                AddEquip(updateCommand, recipeBaseRowId, param, equipId, (int)ShopEquipType.Good, "", eventFlagForStock);
                    eventFlagForStock++;
                //AddGood(updateCommand, param, 8600, 8618, "Map", 30132, 62010);
            }
        }

        public static void ExecSpec(ParamProject paramProject, UpdateCommand updateCommand)
        {

            UpdateLogger.Begin(ParamNames.ShopLineupParamRecipe);

            var param = paramProject.FindParam(ParamNames.ShopLineupParamRecipe);
            if (param == null)
                return;

            var lines = UpateFile.Load(paramProject, UpateFile.UpdateRecipeSpec);

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
