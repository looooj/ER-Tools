using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpdateParam
{
    class UpdateCharaInit
    {

        public static void Test(ParamProject paramProject, UpdateCommand updateCommand)
        {
            var param = paramProject.FindParam(ParamNames.CharaInitParam);
            if (param == null)
                return;

            var row = ParamRowUtils.FindRow(param, 3007);
            if (row == null)
                return;
            /*
             8590;Whetstone Knife;砥石小刀
             130;Spectral Steed Whistle;灵马哨笛
             */

            AddItem(updateCommand, row, 4, 8590, 1);
            AddItem(updateCommand, row, 5, 8500, 1);     


            updateCommand.AddItem(row, "HpEstMax", 13);
            updateCommand.AddItem(row, "MpEstMax", 1);

        }

        //1000
        public static void AddDefault(ParamProject paramProject, UpdateCommand updateCommand) {

            var param = paramProject.FindParam(ParamNames.CharaInitParam);
            if (param == null)
                return;
            for (int i = 0; i < 30; i++) {

                SoulsParam.Param.Row? row = param.FindRow(3000 + i);
                if ( row == null || row.Name == null || row.Name.Length < 3 )
                    continue;

                //1000;Crimson Amber Medallion;红琥珀链坠
                //updateCommand.AddItem(row, "equip_Accessory01", 1000);
                //AddSecondaryItem(updateCommand, row, 2, 130, 1);
                //AddItem(updateCommand, row, 7, 2990, 1);
                //AddItem(updateCommand, row, 8, 8590, 1);
                //AddItem(updateCommand, row, 9, 2919, 1);
            }
        }

        static void AddItem(UpdateCommand updateCommand, SoulsParam.Param.Row row, int index, int itemId, int itemCount)
        {

            updateCommand.AddItem(row, "item_0" + index, itemId);
            updateCommand.AddItem(row, "itemNum_0" + index, itemCount);
        }

        static void AddSecondaryItem(UpdateCommand updateCommand,SoulsParam.Param.Row row,  int index, int itemId, int itemCount) {

            updateCommand.AddItem(row, "secondaryItem_0"+index,itemId);
            updateCommand.AddItem(row, "secondaryItemNum_0" + index, itemCount);
        }
    }
}
