using ERParamUtils.UpateParam;
using Org.BouncyCastle.Asn1.Mozilla;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ERParamUtils.UpateParam.ShopConfigItem;
using static SoulsFormats.MSBE.Event;
using static SoulsFormats.PARAM;

namespace ERParamUtils.UpdateParam
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

        public string NewName="";

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
            NewName = items[2];
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

        //static int insertBaseId=0;
        //public static void SetInsertBaseId(int v) { 
        //
        //    insertBaseId = v;
        //}
        public static void Proc(ShopUpdateItem shopUpdateItem, SoulsParam.Param param, ParamProject paramProject, UpdateCommand updateCommand, bool forceMinPrice)
        {
            if (shopUpdateItem.RowId == 0) {
                shopUpdateItem.RowId = ESDShopConfig.GetNextId();
                if (shopUpdateItem.RowId == 0) {

                    return;
                }
            }                
            UpdateCommandItem item = Create(shopUpdateItem);
            var row = ParamRowUtils.FindRow(param, item.RowId);
            var shopEquipType = (ShopEquipType)shopUpdateItem.EquipType;
            if (row == null) {

                row =UpdateShopLineupParam.AddRow(paramProject, updateCommand, shopUpdateItem.RowId,
                    shopEquipType, shopUpdateItem.NewEquipId, shopUpdateItem.Price, shopUpdateItem.SellQuantity,
                    shopUpdateItem.NewName);

                if ( row != null )
                    UpdateShopLineupParam.ChangeToMinPrice(paramProject, row, shopUpdateItem.NewEquipId, shopEquipType, updateCommand, forceMinPrice);
                return;
            }
            item.Key = "equipType";
            item.Value = shopUpdateItem.EquipType + "";
            updateCommand.AddItem(item);


            item = Create(shopUpdateItem);
            item.Key = "equipId";
            item.Value = shopUpdateItem.NewEquipId + "";
            updateCommand.AddItem(item);

            if (shopUpdateItem.Price > 0)
            {

                UpdateShopLineupParam.ChangeToMinPrice(paramProject, row, shopUpdateItem.NewEquipId, shopEquipType, updateCommand,false);                
            }

            if (shopUpdateItem.SellQuantity != 0)
            {
                item = Create(shopUpdateItem);
                item.Key = "sellQuantity";
                item.Value = shopUpdateItem.SellQuantity + "";
                updateCommand.AddItem(item);
            }
            if (string.IsNullOrEmpty(shopUpdateItem.NewName)) {
                shopUpdateItem.NewName = shopUpdateItem.EquipType +"_"+shopUpdateItem.EquipType;
            } 
            row.GetParamRow().Name = shopUpdateItem.NewName;


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
    }


       
    public class UpdateShopLineupParam
    {

        public static void ExecFile(ParamProject paramProject, UpdateCommand updateCommand,String fn)
        {

            UpdateLogger.Begin(ParamNames.ShopLineupParam);

            var param = paramProject.FindParam(ParamNames.ShopLineupParam);
            if (param == null)
                return;

            var lines = UpdateFile.Load(paramProject, fn);

            foreach (string line in lines)
            {
                var item = new ShopUpdateItem();
                if (item.Parse(line))
                {

                    ShopUpdateItem.Proc(item, param, paramProject, updateCommand,false);
                }
            }

        }

        /*
        private static void ProcKale(ParamProject paramProject, UpdateCommand updateCommand) {

            if (!updateCommand.HaveOption(UpdateParamOptionNames.ShopSuperKale)) {
                return;
            }

            var param = paramProject.FindParam(ParamNames.ShopLineupParam);
            if (param == null)
                return;
            UpdateLogger.InfoParam("ProcKale");
            var oldIdRange = ModConfig.GetOldKaleId();

            var newIdRange = ModConfig.GetNewKaleId();

            int startId = newIdRange.StartID;
            for (int i = oldIdRange.StartID; i <= oldIdRange.EndId; i++) {

                param.InsertRow(startId, "", i);
                startId++;
            }

            ShopUpdateItem.SetInsertBaseId(startId);

            ExecFile(paramProject, updateCommand, ModConfig.GetShopKaleFile());// "shop-kale.txt");
        }
        */
        static void ExecESDShopFile(ParamProject paramProject, UpdateCommand updateCommand, SoulsParam.Param param, string shopName) {
            
            var config = ESDShopConfig.GetIdRange(shopName);
            string fn = ModConfig.GetShopFile(config.File);
            var lines = UpdateFile.Load(paramProject, fn);
            ESDShopConfig.SetCurrent(config);

            foreach (string line in lines)
            {
                var item = new ShopUpdateItem();
                if (item.Parse(line))
                {
                    ShopUpdateItem.Proc(item, param, paramProject, updateCommand,true);
                }
            }

        }
        private static void ProcESDShop(ParamProject paramProject, UpdateCommand updateCommand) {


            //var opt = updateCommand.GetOption(UpdateParamOptionNames.SuperMerchant);

            //if ( opt == 0 )
            //{
            //    return;
            //}

            var param = paramProject.FindParam(ParamNames.ShopLineupParam);
            if (param == null)
                return;

            UpdateLogger.InfoParam("ProcESDShop");
            UpdateShopFile.GenShopFiles();

            ExecESDShopFile(paramProject, updateCommand, param, UpdateParamOptionNames.ShopTable);
            ExecESDShopFile(paramProject, updateCommand, param, UpdateParamOptionNames.ShopGrace);
            ExecESDShopFile(paramProject, updateCommand, param, UpdateParamOptionNames.ShopKale);

            /*
            var superIdRange = ModConfig.GetSuperMerchantId();

            int startId = superIdRange.StartID;
            /*
            if (opt == SuperMerchantType.AppendKale)
            {

                UpdateLogger.InfoParam("Copy Kale");
                var kaleIdRange = ModConfig.GetKaleId();
                for (int i = kaleIdRange.StartID; i <= kaleIdRange.EndId; i++)
                {
                    //copy to SuperMerchant
                    param.InsertRow(startId, "", i);
                    startId++;
                }
            }
            SuperMerchantConfig.SetStartId(startId);
            */
            //?ShopUpdateItem.SetInsertBaseId(startId);

            //ExecFile(paramProject, updateCommand, ModConfig.GetSuperMerchantFile());

        }



        //100000[Gatekeeper Gostoc] Festering Bloody Finger
        //
        //100009[Gatekeeper Gostoc] Furlcalling Finger Remedy
        //100011[Gatekeeper Gostoc] Stormhawk Feather,
        //100013[Gatekeeper Gostoc] Great Arrow
        //100014[Gatekeeper Gostoc] Ballista Bolt
        private static bool ReplaceGatekeeperGostoc(int eqId, EquipType eqType) {

            if (eqType == EquipType.Weapon) {
                if (SpecEquipConfig.IsArrow(eqId, eqType)) {
                    return true;
                }                  
            }
            if (eqType == EquipType.Good)
            {
                if (eqId == 150 || eqId == 111)
                    return true;
            }
            return false;
        }

        private static void AddAncientStone(ParamProject paramProject, UpdateCommand updateCommand,SoulsParam.Param param) {

            int[] idList = { 10168, 10909, 10919 };
            string[] nameList = { "Ancient Dragon Smithing Stone",
                "Somber Ancient Dragon Smithing Stone", "Great Ghost Glovewort" };

            for (int i = 0; i < idList.Length; i++) {
                //AddSuperMerchantItem(paramProject, updateCommand, param, "", idList[i], ShopEquipType.Good, 1000, -1);
            }

        }
        public static void ReplaceAncientStone(ParamProject paramProject, UpdateCommand updateCommand)
        {

            var param = paramProject.FindParam(ParamNames.ShopLineupParam);
            if (param == null)
                return;
            if (updateCommand.HaveOption(UpdateParamOptionNames.ShopTable)) {
                //var startId = ModConfig.GetSuperMerchantId().StartID;
                //SuperMerchantConfig.SetStartId(startId);
                //AddAncientStone(paramProject, updateCommand, param);
                return;
            }
            if (ModConfig.GetModType() == ModConfig.ModType.RAND) {
                UpdateShopLineupParamRecipe.AddAncient(paramProject, updateCommand);
                return;
            }
            int replaceIndex = 0;
            //10140; Ancient Dragon Smithing Stone; 古龙岩锻造石
            //10168; Somber Ancient Dragon Smithing Stone; 古龙岩失色锻造石
            int[] idList = { 10168,10909,10919};

            foreach (var row in param.Rows)
            {
                if (row.ID >= 100000 && row.ID <= 100018)
                {
                    int equipId = GetEquipId(row);

                    ShopEquipType shopEquipType = (ShopEquipType)GetEquipType(row);
                    EquipType equipType = EquipTypeUtils.ConvertFromShopEquipType((ShopEquipType)shopEquipType);

                    if (ReplaceGatekeeperGostoc(equipId, equipType) && replaceIndex < idList.Length) {

                        int tmpId = idList[replaceIndex];
                        UpdateLogger.InfoRow("ReplaceAncientStone {0} [{1},{2}]", tmpId, row.ID,equipId);

                        updateCommand.AddItem(row, "equipId", tmpId);
                        updateCommand.AddItem(row, "equipType", (int)ShopEquipType.Good);
                        updateCommand.AddItem(row, "value", 2000);
                        updateCommand.AddItem(row, "sellQuantity", -1);
                        updateCommand.AddItem(row, "eventFlag_forRelease", 0);

                        replaceIndex++;
                    }

                    if (equipId == 10140) {
                        updateCommand.AddItem(row, "value", 2000);
                        updateCommand.AddItem(row, "sellQuantity", -1);
                        updateCommand.AddItem(row, "eventFlag_forRelease", 0);
                    }
                }

            }
        }


        public static SoulsParam.Param.Row? AddRow(ParamProject paramProject, UpdateCommand updateCommand, int rowId,ShopEquipType equipType, int equipId, int value, int count,string name) {

            var param = paramProject.FindParam(ParamNames.ShopLineupParam);
            if (param == null)
                return null;
            //var shopEqType = EquipTypeUtils.ConvertToShopEquipType(equipType);
            var newName = name;
            if (newName == "")
                newName = equipId + "";
            var row = param.InsertRow(rowId, newName);
            if (row == null) {
                return null;
            }

            /*
0,equipId=52000000  Reference ID(装備ID) s32 Bolt
1,value=20  Sell Price Overwrite(価格) s32 
2,mtrlId=-1  Required Material ID(購入に必要な素材ID) s32 
3,eventFlag_forStock=150200  Quantity - Event Flag ID(個数保持イベントフラグ) u32 
4,eventFlag_forRelease=0  Visibility - Event Flag ID(販売解禁イベントフラグ) u32 
5,sellQuantity=-1  Amount to Sell(販売個数) s16 
7,equipType=Weapon(0)  Equipment Type(装備タイプ) SHOP_LINEUP_EQUIPTYPE(u8) 0=Weapon;1=Protector;2=Accessory;3=Good;4=Ashes;
8,costType=Runes(0)  Currency Type(価格タイプ) SHOP_LINEUP_COSTTYPE(u8) 0=Runes;1=Dragon Heart (Goods 10060);2=Starlight Shard (Goods 1290);3=Unknown (Goods 22000);4=Lost Ashes of War (Goods 10070);
10,setNum=1  Amount on Purchase(販売セット数) u16 
11,value_Add=0  Price Addition(加算) s32 
12,value_Magnification=1  Price Multiplier(倍率) f32 
13,iconId=-1  Icon ID(アイコンID) s32 
14,nameMsgId=-1  Name - Text ID(テキストID) s32 
15,menuTitleMsgId=-1  Menu Title - Text ID(メニュータイトルテキストID) s32 
16,menuIconId=-1  Menu Icon ID(メニューアイコンID) s16 
              
             */
            var eventFlag = EventFlagForStockBuilder.GetEventFlag((int)equipType, equipId);
            string keyValues = 
                "mtrlId,-1,"+
                //"eventFlag_forStock,150200," +
                "eventFlag_forRelease,0," +
                "costType,0," +
                "setNum,1," +
                "value_Add,0," +
                "value_Magnification,1," +
                "iconId,-1," +
                "nameMsgId,-1," +
                "menuTitleMsgId,-1," +
                "menuIconId,-1"
                ;
            ParamRowUtils.SetCellValue(row, "value", value +"");
            updateCommand.AddKeyValues(param.Name, row.ID, keyValues);
            updateCommand.AddItem(row, "equipId", equipId);
            updateCommand.AddItem(row, "equipType", (int)equipType);
            //updateCommand.AddItem(row, "value", value);
            updateCommand.AddItem(row, "sellQuantity", count);
            updateCommand.AddItem(row, "eventFlag_forStock", eventFlag);

            return row;
        }

        public static void Exec(ParamProject paramProject, UpdateCommand updateCommand)
        {

            //if (updateCommand.HaveOption(UpdateParamOptionNames.ShopSetAll)) {
            //    updateCommand.SetOption(UpdateParamOptionNames.ShopVisibilityAll,1);
            //    updateCommand.SetOption(UpdateParamOptionNames.ShopSellQuantityUnlimited, 1);
            //    updateCommand.SetOption(UpdateParamOptionNames.ShopAddAncient, 1);
            //}


            UpdateLogger.InfoTime("===UpdateShopLineupParam.Exec");


            UpdateLogger.Begin(ParamNames.ShopLineupParam);

            //
            ProcESDShop(paramProject,updateCommand);


            var param = paramProject.FindParam(ParamNames.ShopLineupParam);
            if (param == null)
                return;

            foreach (var row in param.Rows)
            {

                if (row.ID >= 110000 || row.ID < 100000)
                    continue;

                if (updateCommand.HaveOption(UpdateParamOptionNames.ShopVisibilityAll))
                {
                    ChangeVisibility(row, updateCommand);
                }

                if (row.Name != null && row.Name.Contains("Enia")) {
                    ChangeVisibility(row, updateCommand);
                }

                int equipId = GetEquipId(row);
                ShopEquipType shopEquipType = (ShopEquipType)GetEquipType(row);
                EquipType equipType = EquipTypeUtils.ConvertFromShopEquipType((ShopEquipType)shopEquipType);
                if (SpecEquipConfig.IsMagic(equipId, equipType) || shopEquipType == ShopEquipType.Ash) {
                    ChangeVisibility(row, updateCommand);
                }
                //
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
                    || SpecEquipConfig.IsFinger(equipId,equipType)
                    || SpecEquipConfig.GetSpec(equipId, equipType) > 0
                    )
                {
                    if ( updateCommand.HaveOption(UpdateParamOptionNames.ShopSellQuantityUnlimited))
                        ChangeSellAmount(row, "sellQuantity", -1, updateCommand);
                }
                ChangeToMinPrice(paramProject, row, equipId, shopEquipType, updateCommand,false);

            }


            /*
            if (updateCommand.HaveOption(UpdateParamOptionNames.ShopAddAncient))
            {
                
                //if (ModConfig.GetModType() == ModConfig.ModType.STD)
                    ReplaceAncientStone(paramProject, updateCommand);
                //if (ModConfig.GetModType() == ModConfig.ModType.RAND) {                 
                //    UpdateShopLineupParamRecipe.AddAncient(paramProject, updateCommand);
                //}
            }*/

        }


        public static void ChangeToMinPrice(ParamProject paramProject,
            SoulsParam.Param.Row row, int equipId, ShopEquipType shopEquipType, UpdateCommand updateCommand, bool force)
        {

            EquipType equipType = EquipTypeUtils.ConvertFromShopEquipType(shopEquipType);

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

            if (price > minPrice || price < 1 || force)
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
            }
        }



    }
}
