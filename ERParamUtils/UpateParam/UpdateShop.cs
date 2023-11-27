using Org.BouncyCastle.Asn1.Mozilla;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SoulsFormats.MSBE.Event;
using static SoulsFormats.PARAM;

namespace ERParamUtils.UpateParam
{

    class ShopUpdateItem
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        //public string NewName { get; set; }
        public int RowId;
        public int NewEquipId;
        public int Price;
        public int EquipType;
        public int SellQuantity;

        public bool Parse(string line)
        {

            line = line.Trim();
            if (line.StartsWith("#"))
                return false;


            RowId = 0;

            string[] items = line.Split(';');
            if (items.Length < 4)
                return false;
            Price = 0;
            SellQuantity = 1;
            RowId = Int32.Parse(items[0]);
            NewEquipId = Int32.Parse(items[1]);
            EquipType = Int32.Parse(items[3]);
            if (items.Length >= 5)
                Price = Int32.Parse(items[4]);
            if (items.Length >= 6)
                SellQuantity = Int32.Parse(items[5]);

            return true;
        }


        static UpdateCommandItem Create(ShopUpdateItem shopUpdateItem)
        {
            UpdateCommandItem item = new();
            item.ParamName = ParamNames.ShopLineupParam;
            item.RowId = shopUpdateItem.RowId;
            return item;
        }

        public static void Proc(ShopUpdateItem shopUpdateItem, SoulsParam.Param param, ParamProject paramProject, UpdateCommand updateCommand)
        {

            UpdateCommandItem item = Create(shopUpdateItem);
            item.Key = "equipType";
            item.Value = shopUpdateItem.EquipType + "";
            updateCommand.AddItem(item);


            item = Create(shopUpdateItem);
            item.Key = "equipId";
            item.Value = shopUpdateItem.NewEquipId + "";
            updateCommand.AddItem(item);

            if (shopUpdateItem.Price > 0)
            {

                ChangePrice(paramProject, param, shopUpdateItem, updateCommand);
            }

            if (shopUpdateItem.SellQuantity != 0)
            {
                item = Create(shopUpdateItem);
                item.Key = "sellQuantity";
                item.Value = shopUpdateItem.SellQuantity + "";
                updateCommand.AddItem(item);
            }

            /*
             * todo change row name
            string name = row.Name;
            int pos = name.IndexOf("]");
            if (pos >= 0)
            {
                name = name.Substring(0, pos + 2);
                name += NewName;
                row.Name = name;
            } */
        }


        public static void ChangePrice(ParamProject paramProject,
            SoulsParam.Param param, ShopUpdateItem shopUpdateItem, UpdateCommand updateCommand)
        {

            var row = ParamRowUtils.FindRow(param, shopUpdateItem.RowId);
            if (row == null)
                return;

            string? paramName = EquipTypeUtils.ShopTypeParamName((ShopEquipType)shopUpdateItem.EquipType);
            if (paramName == null)
                return;

            int minPrice = ParamRowUtils.GetCellInt(paramProject, paramName, shopUpdateItem.NewEquipId, "sellValue", -1);
            if (minPrice <= 0)
                minPrice = 100;

            int price = minPrice;//shopUpdateItem.Price > minPrice ? minPrice : shopUpdateItem.Price;

            updateCommand.AddItem(row, "value", price);
        }
    }


    public class UpdateShopLineupParamRecipe
    {


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

        //Whetblade

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
                //ParamRowUtils.SetCellValue(row, "equipId", equipId);
                ParamRowUtils.SetCellValue(row, "eventFlag_forStock", eventFlag_forStock);
                updateCommand.AddItem(row, "equipId", equipId);
            }
        }


        public static void AddWhetblade(ParamProject paramProject, UpdateCommand updateCommand)
        {
            var param = paramProject.FindParam(ParamNames.ShopLineupParamRecipe);
            if (param == null)
                return;
            AddGood(updateCommand, param, 8970, 8974, "Whetblade", 30151, 65610);

        }

        public static void AddMapPiece(ParamProject paramProject, UpdateCommand updateCommand)
        {
            var param = paramProject.FindParam(ParamNames.ShopLineupParamRecipe);
            if (param == null)
                return;

            AddGood(updateCommand, param, 8600, 8618, "Map", 30132, 62010);
        }
    }

       
    public class UpdateShopLineupParam
    {

        public static void ExecSpec(ParamProject paramProject, UpdateCommand updateCommand)
        {

            UpdateLogger.Begin(ParamNames.ShopLineupParam);

            var param = paramProject.FindParam(ParamNames.ShopLineupParam);
            if (param == null)
                return;

            var lines = UpateFile.Load(paramProject, UpateFile.UpdateShopSpec);

            foreach (string line in lines)
            {
                var item = new ShopUpdateItem();
                if (item.Parse(line))
                {

                    ShopUpdateItem.Proc(item, param, paramProject, updateCommand);
                }
            }

        }
        public static void ExecDefaultUpdate(ParamProject paramProject, UpdateCommand updateCommand)
        {

            UpdateLogger.Begin(ParamNames.ShopLineupParam);

            var param = paramProject.FindParam(ParamNames.ShopLineupParam);
            if (param == null)
                return;

            foreach (var row in param.Rows)
            {

                if (row.ID >= 110000 || row.ID < 100000)
                    continue;

                ChangeVisibility(row, updateCommand);

                int equipId = GetEquipId(row);
                ShopEquipType shopEquipType = (ShopEquipType)GetEquipType(row);
                EquipType equipType = EquipTypeUtils.ConvertShopEquipType((ShopEquipType)shopEquipType);

                if (SpecEquipConfig.IsArrow(equipId, equipType)
                    || SpecEquipConfig.IsPot(equipId, equipType)
                    || SpecEquipConfig.IsRemnant(equipId, equipType)
                    || SpecEquipConfig.IsSmithingStone(equipId, equipType)
                    || SpecEquipConfig.IsGlovewort(equipId, equipType)
                    || SpecEquipConfig.IsAromatic(equipId, equipType)
                    || SpecEquipConfig.IsBoluses(equipId, equipType)
                    || SpecEquipConfig.IsMaterial(equipId, equipType)
                    || SpecEquipConfig.IsMeat(equipId, equipType)
                    || SpecEquipConfig.IsRemembrance(equipId, equipType)
                    || SpecEquipConfig.IsSacrificialTwig(equipId, equipType)
                    || SpecEquipConfig.GetSpec(equipId, equipType) > 0
                    )
                {
                    ChangeSellAmount(row, "sellQuantity", -1, updateCommand);
                }
                ChangeToMinPrice(paramProject, row, equipId, shopEquipType, updateCommand);

            }
        }


        public static void ChangeToMinPrice(ParamProject paramProject,
            SoulsParam.Param.Row row, int equipId, ShopEquipType shopEquipType, UpdateCommand updateCommand)
        {

            EquipType equipType = EquipTypeUtils.ConvertShopEquipType(shopEquipType);

            string? paramName = EquipTypeUtils.ShopTypeParamName(shopEquipType);
            if (paramName == null)
                return;

            int minPrice = ParamRowUtils.GetCellInt(paramProject, paramName, equipId, "sellValue", -1);
            if (minPrice <= 0)
            {
                minPrice = 100;
            }

            int price = ParamRowUtils.GetCellInt(row, "value", 0);
            int costType = ParamRowUtils.GetCellInt(row, "costType", 0);
            if (costType != 0)
            {
                updateCommand.AddItem(row, "costType", 0);
                price = -1;
            }

            if (SpecEquipConfig.IsArrow(equipId, equipType))
                minPrice = 20;

            if (price > minPrice || price < 1)
            {
                updateCommand.AddItem(row, "value", minPrice);
            }

        }



        static int GetEquipId(SoulsParam.Param.Row row)
        {
            return ParamRowUtils.GetCellInt(row, "equipId", 0);
        }

        static int GetEquipType(SoulsParam.Param.Row row)
        {
            return ParamRowUtils.GetCellInt(row, "equipType", -1);
        }


        static void ChangeSellAmount(SoulsParam.Param.Row row, string key, int amount, UpdateCommand updateCommand)
        {

            updateCommand.AddItem(row, key, amount);


        }

        static void ChangeVisibility(SoulsParam.Param.Row row, UpdateCommand updateCommand)
        {
            //if (row.Name == null)
            //    return;

            //if (row.Name.Contains("[") && row.Name.Contains("]"))
            if (row.ID < 110000 && row.ID >= 100000)
            {

                string key = "eventFlag_forRelease";

                if (ParamRowUtils.GetCellInt(row, key, 0) != 0)
                {
                    updateCommand.AddItem(row, key, "0");
                }

                //key = "mtrlId";
                //updateCommand.AddItem(row, key, "-1");
            }
        }

    }
}
