using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    class ERConfig
    {
    }


    public static class ERShopCol
    {
        /*
         0,Reference ID,equipId,1100100,s32 equipId
1,Sell Price Overwrite,value,1500,s32 value
2,Required Material ID,mtrlId,-1,s32 mtrlId
3,Quantity - Event Flag ID,eventFlag_forStock,150160,u32 eventFlag_forStock
4,Visibility - Event Flag ID,eventFlag_forRelease,0,u32 eventFlag_forRelease
5,Amount to Sell,sellQuantity,1,s16 sellQuantity
7,Equipment Type,equipType,1,u8 equipType
8,Currency Type,costType,0,u8 costType
10,Amount on Purchase,setNum,1,u16 setNum
11,Price Addition,value_Add,0,s32 value_Add
12,Price Multiplier,value_Magnification,1,f32 value_Magnification
13,Icon ID,iconId,-1,s32 iconId
14,Name - Text ID,nameMsgId,-1,s32 nameMsgId
15,Menu Title - Text ID,menuTitleMsgId,-1,s32 menuTitleMsgId
16,Menu Icon ID,menuIconId,-1,s16 menuIconId 
         */

        public static readonly int Price = 1;
        public static readonly int EquipId = 0;
        public static readonly int Visibility = 4;
        public static readonly int SellAmount = 5;
        public static readonly int EquipType = 7;
        public static readonly int CostType = 8;

    }

}
