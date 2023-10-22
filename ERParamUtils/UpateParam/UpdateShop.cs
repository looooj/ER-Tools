﻿using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SoulsFormats.MSBE.Event;

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
            SellQuantity = 0;
            RowId = Int32.Parse(items[0]);
            NewEquipId = Int32.Parse(items[1]);
            EquipType = Int32.Parse(items[3]);
            if (items.Length >= 5)
                Price = Int32.Parse(items[4]);
            if (items.Length >= 6)
                SellQuantity = Int32.Parse(items[5]);

            return true;
        }


        static UpdateCommandItem Create(ShopUpdateItem shopUpdateItem) {
            UpdateCommandItem item = new();
            item.ParamName = ParamNames.ShopLineupParam;
            item.RowId = shopUpdateItem.RowId;
            return item;
        }

        public static void Proc(ShopUpdateItem shopUpdateItem, UpdateCommand updateCommand)
        {

            UpdateCommandItem item = Create(shopUpdateItem);
            item.Key = "equipType";
            item.Value = shopUpdateItem.EquipType+"";
            updateCommand.AddItem(item);


            item = Create(shopUpdateItem);
            item.Key = "equipId";
            item.Value = shopUpdateItem.NewEquipId + "";
            updateCommand.AddItem(item);

            if (shopUpdateItem.Price > 0)
            {
                item = Create(shopUpdateItem);
                item.Key = "value";
                item.Value = shopUpdateItem.Price + "";
                updateCommand.AddItem(item);
            }

            if (shopUpdateItem.SellQuantity > 0) {
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

    }




    public class UpdateShopLineupParam
    {

        public static void ExecSpec(ParamProject paramProject,UpdateCommand updateCommand) {

            UpdateLogger.Begin(ParamNames.ShopLineupParam);

            var param = paramProject.FindParam(ParamNames.ShopLineupParam);
            if (param == null)
                return;

            var lines = UpateFile.Load(paramProject, UpateFile.UpdateShopSpec);

            foreach (string line in lines)
            {
                var item = new ShopUpdateItem();
                if (item.Parse(line)) {

                    ShopUpdateItem.Proc(item, updateCommand);
                }
            }

        }
        public static void ExecDefaultUpdate(ParamProject paramProject) {

            UpdateLogger.Begin(ParamNames.ShopLineupParam);

            var param =  paramProject.FindParam(ParamNames.ShopLineupParam);
            if (param == null)
                return;

            foreach (var row in param.Rows) {
                ChangeVisibility(row);


                int equipId = GetEquipId(row);
                int equipType = GetEquipType(row);

                if (SpecEquipConfig.IsArrow(equipId) 
                    || SpecEquipConfig.IsPot(equipId)
                    || SpecEquipConfig.IsRemnant(equipId)
                    || SpecEquipConfig.IsSmithingStone(equipId)
                    || SpecEquipConfig.GetSpec(equipId) > 0 
                    )
                {
                    ChangeSellAmount(row, -1);
                }

                ChangeStoneGlovewortPrice(paramProject,row, equipId);

                ChangeSpecPrice(paramProject, row, equipId, (ShopEquipType)equipType);
            }
        }

        static void ChangeSpecPrice(ParamProject paramProject, FSParam.Param.Row row, int equipId, ShopEquipType equipType) {

            switch (equipType) {
                case ShopEquipType.Weapon:
                    {
                        ParamRowUtils.SetCellValue(paramProject, ParamNames.EquipParamWeapon, equipId, "sellValue", 5);
                        string price = "value";
                        int value = ParamRowUtils.GetCellInt(row, price, 0);
                        int maxValue = 200;
                        if (SpecEquipConfig.IsArrow(equipId))
                            maxValue = 20;

                        if (value > maxValue)
                        {
                            ParamRowUtils.SetCellValue(row, price, maxValue);
                            UpdateLogger.InfoRow(row, price, maxValue);
                        }
                    }
                    break;
                case ShopEquipType.Good:

                    break;
            }



        }

        static void ChangeStoneGlovewortPrice(ParamProject paramProject,FSParam.Param.Row row,int equipId)
        {
            if (SpecEquipConfig.IsSmithingStone(equipId) || SpecEquipConfig.IsGlovewort(equipId))

            {
                ParamRowUtils.SetCellValue(paramProject, ParamNames.EquipParamGoods, equipId, "sellValue", 50);

                string price = "value";
                int value = ParamRowUtils.GetCellInt(row, price,0);
                int maxValue = 200;
                if (value > maxValue)
                {
                    ParamRowUtils.SetCellValue(row, price, maxValue);                    
                    UpdateLogger.InfoRow(row, price, maxValue);
                }
            }
        }



        static int GetEquipId(FSParam.Param.Row row)
        {
            return ParamRowUtils.GetCellInt(row, "equipId",0);
        }

        static int GetEquipType(FSParam.Param.Row row)
        {
            return ParamRowUtils.GetCellInt(row, "equipType", -1);
        }


        static void ChangeSellAmount(FSParam.Param.Row row, int amount)
        {
            string key = "sellQuantity";
            ParamRowUtils.SetCellValue(row, key, amount);
            UpdateLogger.InfoRow(row, key, amount);

        }

        static void ChangeVisibility(FSParam.Param.Row row)
        {
            if (row.Name == null)
                return;

            if (row.Name.Contains("[") && row.Name.Contains("]"))
            {

                string key = "eventFlag_forRelease";

                if (ParamRowUtils.GetCellInt(row, key, 0) != 0)
                {
                    ParamRowUtils.SetCellValue(row, key, 0);
                    UpdateLogger.InfoRow(row, key, 0);
                }
                key = "mtrlId";
                ParamRowUtils.SetCellValue(row, key, -1);
                UpdateLogger.InfoRow(row, key, -1);

            }
        }

    }
}
