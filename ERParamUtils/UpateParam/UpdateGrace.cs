using Org.BouncyCastle.Asn1;
using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpdateParam
{
    class UpdateGrace
    {

        static string eventflagIdKey = "eventflagId";

        public static void UnlockGrace(ParamProject paramProject, UpdateCommand updateCommand)
        {
            var param = paramProject.FindParam(ParamNames.BonfireWarpParam);
            if (param == null)
                return;


            //100000;Godrick the Grafted;“接肢”葛瑞克
            //100001; Margit, the Fell Omen;“恶兆妖鬼”玛尔基特

            //190000[Elden Throne] Fractured Marika 
            //110000[Leyndell, Royal Capital] Elden Throne 
            //110001;Erdtree Sanctuary;黄金树大教堂

            //350000;Cathedral of the Forsaken;弃置恶兆的大教堂

            //150000;Malenia, Goddess of Rot;“腐败女神”玛莲妮亚
            //150005 [Miquella's Haligtree] Haligtree Promenade

            //160000;Rykard, Lord of Blasphemy;“亵渎君王”拉卡德
            //160001;Temple of Eiglay;艾格蕾教堂
            //160006; Abductor Virgin; 掳人少女人偶

            //130001;Dragonlord Placidusax;“龙王”普拉顿桑克斯
            //130002;Dragon Temple Altar;龙教堂祭坛
            //130000;Maliketh, the Black Blade;“黑剑”玛利喀斯

            //650008;Fire Giant;火焰巨人

            //630050;Castellan's Hall;城主大厅

            //640014;Starscourge Radahn;“碎星”拉塔恩
            //640007;Heart of Aeonia;艾奥尼亚中心地
            //640010;Redmane Castle Plaza;红狮子城（广场）

            //620047;Royal Moongazing Grounds;王室赏月地
            //620062;Ranni's Chamber;菈妮的房间

            //140000;Raya Lucaria Grand Library;雷亚卢卡利亚大书库
            //140001;Debate Parlor;讨论室

            //611011;Morne Moangrave;摩恩悲叹墓

            //390000;Magma Wyrm Makar;“熔岩土龙”马卡尔

            //650054;Castle Sol Rooftop;索尔城（屋顶）
            //610018;Waypoint Ruins Cellar;驿站街遗迹的地下室



            //120400;Astel, Naturalborn of the Void;“黑暗弃子”艾丝缇
            //120300;Prince of Death's Throne;死王子宝座
            //120201;Mimic Tear;仿身泪滴
            //120200;Great Waterfall Basin;大瀑布水潭
            //120500;Cocoon of the Empyrean;神人坠眠之茧
            //120100;Dragonkin Soldier of Nokstella;诺克史黛拉的龙人士兵

            //200000; Theatre of the Divine Beast; 神兽舞台
            //694005;Church of the Bud;花蕾教堂
            //280000;Discussion Chamber;对谈室
            //685003;Rest of the Dread Dragon;狂龙之席
            //682002;Ensis Moongazing Grounds;恩希斯赏月地
            //210001;Main Gate Plaza;正门广场
            //210100;Messmer's Dark Chamber;梅瑟莫的暗室
            //210007; Sunken Chapel; 沉水礼拜堂

            //692000;Scaduview;望影露台
            //220000;Garden of Deep Purple;深紫花园
            //694005; Church of the Bud; 花蕾教堂
            //696000; Scadutree Base; 幽影树的树脚
            //200100;Gate of Divinity;神之门

            //111000;Roundtable Hold;圆桌厅堂

            int[] skipRowIds = {
                111000,
                100001,100000,
                110000,110001,350000,190000,
                150000,150005,160000,160001,160006,
                130001,130002,130000,
                650008,630050,611011,390000,650054,610018,
                640014,640010,640007,
                620047,620062,620043,
                140000,140001,
                120400,120300,120201,120200,120500,120100,
                200000,694005,280000,685003,682002,210001,210100,210007,
                692000,220000,694005,696000,200100
               };

            for (int i = 0; i < param.Rows.Count; i++)
            {

                var row = param.Rows[i];

             
                int textId = ParamRowUtils.GetCellInt(row, "textId1", 0);                
                if (skipRowIds.Contains(row.ID) || skipRowIds.Contains(textId) )
                    continue;


                updateCommand.AddItem(row, eventflagIdKey, "71801");
            }
        }
    
        /* 
        public static void UnlockGlaceDefault(UpdateCommand updateCommand) {

            //111000;Table of Lost Grace;大赐福
            int[] defaultIds = { 111000 };
            foreach (int rowId in defaultIds) {
                updateCommand.AddItem(ParamNames.BonfireWarpParam, rowId, eventflagIdKey, 71801);
            }
        }*/


        //MapDefaultInfoParam
        public static void SetMapInfoParam(ParamProject paramProject, UpdateCommand updateCommand) {
            var param = paramProject.FindParam(ParamNames.MapDefaultInfoParam);
            if (param == null)
                return;
            string key = "EnableFastTravelEventFlagId";

            for (int i = 0; i < param.Rows.Count; i++)
            {

                var row = param.Rows[i];
                if (row.Name == null || row.Name.Length < 1)
                    continue;

                int val = ParamRowUtils.GetCellInt(row, key, -1);
                if (val > 0) {
                    updateCommand.AddItem(row, key, "82001");
                }
            }
        }


        /*
public void ExecMap(ParamProject paramProject, UpdateCommand updateCommand)
{
    var param = paramProject.FindParam(ParamNames.WorldMapPointParam);
    if (param == null)
        return;

    for (int i = 0; i < param.Rows.Count; i++)
    {

        var row = param.Rows[i];
        if (row.Name == null || row.Name.Length < 1)
            continue;

        string key = "eventFlagId";

        updateCommand.AddItem(row, key, "18000021");
    }

}*/
    }




}
