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
        public string Name = "";
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
            UpdateShopLineupParamRecipe.AddEquipX(updateCommand, item.RowId,
                param,
                item.EquipId,
                item.EquipType, item.Name, item.EventFlagForStock);

        }
    }

    public class UpdateShopLineupParamRecipe
    {

        static int recipeBaseRowId = 31000;

        public static void Init(ParamProject project)
        {
            EventFlagForStockBuilder.FindFromLot(project);
            EventFlagForStockBuilder.FindFromShop(project);
            //eventFlagForStockMap = EventFlagForStockBuilder.GetEventFlagStockMap();
        }

        //static Dictionary<string, int> eventFlagForStockMap = new();


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
                key = "mtrlId";
                if (ParamRowUtils.GetCellInt(row, key, 0) != 0)
                {
                    updateCommand.AddItem(row, key, "-1");
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



        public static bool AddEquipX(UpdateCommand updateCommand, int rowId,
             SoulsParam.Param param, int equipId, int equipType, string name, int eventFlagForStock1)

        {
            if (rowId >= 32000)
            {
                //updateCommand.
                UpdateLogger.InfoRow("{0} rowId {1} over", param.Name, rowId);
                return false;
            }
            if (name.Length < 1)
                name = "_" + equipId;

            int eventFlagForStock = eventFlagForStock1;
            if (eventFlagForStock == 0) {
                eventFlagForStock = EventFlagForStockBuilder.GetEventFlag(equipType, equipId);
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
            UpdateLogger.InfoRow("add {0} {1} {2}", rowId, equipId, equipType);
            return true;
        }

        public static void AddEquips(UpdateCommand updateCommand,
            SoulsParam.Param param, int beginEquipId, int endEquipId, int equipType, string name, int beginRowId)
        {

            int rowId = beginRowId;
            for (int equipId = beginEquipId; equipId <= endEquipId; equipId++)
            {
                var name1 = name + "_" + equipId;
                AddEquipX(updateCommand, rowId, param, equipId, equipType, name1, 0);
                rowId++;
            }
            recipeBaseRowId = rowId;
        }

        public static void AddEquip(UpdateCommand updateCommand,
            SoulsParam.Param param, int equipId, int equipType, string name) {

            AddEquips(updateCommand, param, equipId, equipId, equipType, name, recipeBaseRowId);
        }

        public static void AddWhetblade(ParamProject paramProject, UpdateCommand updateCommand)
        {
            var param = paramProject.FindParam(ParamNames.ShopLineupParamRecipe);
            if (param == null)
                return;

            AddEquips(updateCommand, param, 8970, 8974, (int)ShopEquipType.Good, "Whetblade", recipeBaseRowId);

        }


        public static void AddMapPiece(ParamProject paramProject, UpdateCommand updateCommand)
        {
            var param = paramProject.FindParam(ParamNames.ShopLineupParamRecipe);
            if (param == null)
                return;

            AddEquips(updateCommand, param, 8600, 8618, (int)ShopEquipType.Good, "", recipeBaseRowId);
            AddEquips(updateCommand, param, 2008600, 2008604, (int)ShopEquipType.Good, "", recipeBaseRowId);

        }

        //Bell Bearing
        public static void AddBellBearing(ParamProject paramProject, UpdateCommand updateCommand) {
            var param = paramProject.FindParam(ParamNames.ShopLineupParamRecipe);
            if (param == null)
                return;
            AddEquips(updateCommand, param, 8910, 8913, (int)ShopEquipType.Good, "", recipeBaseRowId);
            AddEquips(updateCommand, param, 8915, 8933, (int)ShopEquipType.Good, "", recipeBaseRowId);
            AddEquips(updateCommand, param, 8935, 8944, (int)ShopEquipType.Good, "", recipeBaseRowId);

        }

        public static void AddRemnant(ParamProject paramProject, UpdateCommand updateCommand) {
            var param = paramProject.FindParam(ParamNames.ShopLineupParamRecipe);
            if (param == null)
                return;
            AddEquips(updateCommand, param, 20950, 20957, (int)ShopEquipType.Good, "", recipeBaseRowId);
            AddEquips(updateCommand, param, 20900, 20904, (int)ShopEquipType.Good, "", recipeBaseRowId);

        }


        public static void AddOthers(ParamProject paramProject, UpdateCommand updateCommand) {
            var param = paramProject.FindParam(ParamNames.ShopLineupParamRecipe);
            if (param == null)
                return;
            AddEquips(updateCommand, param, 3050, 3051, (int)ShopEquipType.Good, "", recipeBaseRowId);
            AddEquips(updateCommand, param, 1290, 1290, (int)ShopEquipType.Good, "", recipeBaseRowId);

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
