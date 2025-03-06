﻿using ERParamUtils.UpdateParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class EventFlagForStockBuilder
    {

        static Dictionary<string, int> eventFlagForStockMap = new();

        public static Dictionary<string, int> GetEventFlagStockMap() {

            return eventFlagForStockMap;
        }

        /*
        public static void LoadFromFilex()
        {

            string fn = GlobalConfig.BaseDir + "\\docs\\recipe-param\\eventFlag_forStock.txt";
            if (!File.Exists(fn))
            {
                return;
            }
            string[] lines = File.ReadAllLines(fn);
            foreach (string line in lines)
            {

                var tmp = line.Trim();
                var items = tmp.Split(",");
                //31602,62010,8600,3
                //rowId,eventFlag_forStock,equipId,equipType
                if (items.Length == 4)
                {
                    var key = items[2] + "_" + items[3];
                    var val = items[1].Trim();
                    if (val.Length < 1)
                        continue;
                    eventFlagForStockMap.TryAdd(key, int.Parse(val));
                }
            }


        }
        */

        public static void FindFromShop(ParamProject project)
        {

            var param = project.FindParam(ParamNames.ShopLineupParam);
            if (param == null)
                return;
            foreach (var row in param.Rows)
            {
                findEventFlagFromShop(row);
            }
        }

        static void findEventFlagFromShop(SoulsParam.Param.Row row)
        {

            /*
            return ParamRowUtils.GetCellInt(row, "equipId", 0);
            */
            int itemId = ParamRowUtils.GetCellInt(row, "equipId", 0);
            int itemType = ParamRowUtils.GetCellInt(row, "equipType", 0);

            //int eqType = (int)EquipTypeUtils.ConvertShopEquipType((ShopEquipType)itemType);

            int eventFlag = ParamRowUtils.GetCellInt(row, "eventFlag_forStock", 0);

            var k =  itemId  + "_"  + itemType;
            eventFlagForStockMap.TryAdd(k, eventFlag);
        }


        public static void FindFromLot(ParamProject project)
        {

            string[] paramNames = { ParamNames.ItemLotParamMap, ParamNames.ItemLotParamEnemy };
            foreach (string paramName in paramNames)
            {
                var param = project.FindParam(paramName);
                if (param == null)
                    continue;

                foreach (var row in param.Rows)
                {
                    findEventFlagFromLot(row);
                }
            }

        }

        static void findEventFlagFromLot(SoulsParam.Param.Row row)
        {

            for (int i = 1; i < 8; i++)
            {
                string key = "lotItemId0" + i;
                int itemId = ParamRowUtils.GetCellInt(row, key, 0);
                if (itemId < 1)
                {
                    if (i >= 3)
                        return;
                    continue;
                }
                key = "lotItemCategory0" + i;
                int itemType = ParamRowUtils.GetCellInt(row, key, 0);
                if (itemType < 1)
                {
                    continue;
                }

                if (itemType == (int)EquipType.Good)
                {

                    key = "getItemFlagId";
                    int eventFlag = ParamRowUtils.GetCellInt(row, key, 0);
                    if (eventFlag < 1)
                    {
                        return;            
                    }
                    int itemType1 = (int)EquipTypeUtils.ConvertToShopEquipType((EquipType)itemType);
                    var k = itemId + "_" + itemType1;
                    eventFlagForStockMap.TryAdd(k, eventFlag);
                }
            }
        }

    }
}

/*
    https://github.com/ThomasJClark/elden-ring-glorious-merchant

    for (auto param : {L"ItemLotParam_map", L"ItemLotParam_enemy"})
    {
        for (auto [id, row] : from::params::get_param<from::paramdef::ITEMLOT_PARAM_ST>(param))
        {
            // Record flags set when looting goods
            if (row.lotItemCategory01 == lot_item_category_goods && row.getItemFlagId > 0)
            {
                goods_flags[row.lotItemId01] = row.getItemFlagId;
            }
        }
    }
    for (auto [id, row] :
         from::params::get_param<from::paramdef::SHOP_LINEUP_PARAM>(L"ShopLineupParam"))
    {
        // Record flags set when purchasing goods
        if (row.equipType == equip_type_goods)
        {
            goods_flags[row.equipId] = row.eventFlag_forStock;
        }
        // Record flags required for Hewg to duplicate AoWs
        else if (row.equipType == equip_type_gem && row.costType == cost_type_lost_ashes_of_war)
        {
            gems_flags[row.equipId] = row.eventFlag_forRelease;
        }
        // Record goods IDs that are used for replacement text in shops
        if (row.nameMsgId != -1)
        {
            dummy_goods_ids.insert(row.nameMsgId);
        }
    }     
 
 */ 