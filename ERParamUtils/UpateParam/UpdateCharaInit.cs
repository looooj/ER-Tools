using ERParamUtils.UpateParam;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ERParamUtils.UpdateParam
{
    public class CharInitRow {
        public string RowName="";
        public int RowId;

        public CharInitRow(int id, string name)
        {
            RowId = id;
            RowName = name;
        }
    };
    public class UpdateCharaInit
    {
        
        static Dictionary<int,int> removeWeaponDict = new Dictionary<int,int>();

        public static List<CharInitRow> GetCharInitClassRowList(ParamProject paramProject)
        {

            List<CharInitRow> list = new();
            var param = paramProject.FindParam(ParamNames.CharaInitParam);
            if (param == null)
                return list;
            bool startFlag = false;
            for (int i = 0; i < 100; i++)
            {
                addItemOffset = 0;
                addSecondaryItemOffset = 0;
                SoulsParam.Param.Row? row = param.FindRow(3000 + i);
                if (row == null || row.Name == null || row.Name.Length < 3
                    || !row.Name.StartsWith("Class")
                    )
                {
                    if (startFlag)
                    {
                        break;
                    }
                    continue;
                }
                list.Add(new CharInitRow(row.ID, row.Name));
            }
            return list;
        }

        public static string GetCharInitClassNames(List<CharInitRow> charInitRows) { 

            string s = string.Empty;
            for (int i = 0; i < charInitRows.Count; i++) {
                if ( i > 0 )
                    s += ", ";
                s = s + charInitRows[i].RowName;
            }
            return s;
        }

        public static string GetCharInitClassIds(List<CharInitRow> charInitRows)
        {

            string s = string.Empty;
            for (int i = 0; i < charInitRows.Count; i++)
            {
                if (i > 0)
                    s += ", ";
                s = s + charInitRows[i].RowId;
            }
            return s;
        }


        public static void Exec(ParamProject paramProject, UpdateCommand updateCommand)
        {
            var param = paramProject.FindParam(ParamNames.CharaInitParam);
            if (param == null)
                return;
            removeWeaponDict.Clear();
            bool startFlag = false;
            for (int i = 0; i < 100; i++)
            {
                addItemOffset = 0;
                SoulsParam.Param.Row? row = param.FindRow(3000 + i);
                if (row == null || row.Name == null || row.Name.Length < 3
                    || !row.Name.StartsWith("Class")
                    )
                {
                    if (startFlag)
                    {
                        break;
                    }
                    continue;
                }
                startFlag = true;
                if (updateCommand.HaveOption(UpdateParamOptionNames.AddInitCrimsonAmberMedallion))
                {
                    AddCrimsonAmberMedallion(updateCommand, row);
                }

                if (updateCommand.HaveOption(UpdateParamOptionNames.AddInit99Rune))
                {
                    AddItem(updateCommand, row, 2919, 99);
                }
                //207010 MimicTear Ashes +10
                if (updateCommand.HaveOption(UpdateParamOptionNames.AddInitMimicTear))
                {
                    AddItem(updateCommand, row, 207010, 1);
                }
                //2160;Pureblood Knight's Medal;纯血骑士勋章
                if (updateCommand.HaveOption(UpdateParamOptionNames.AddInitPurebloodKnightMeda))
                {
                    AddItem(updateCommand, row, 2160, 1);
                }

                ReplaceBow(updateCommand, row);

                ReplaceIntelligenceWeapon(updateCommand, row,
                    updateCommand.GetOption(UpdateParamOptionNames.ReplaceInitIntelligenceWeapon)
                    );
                //ReplaceInitDexterityWeapon
                ReplaceDexterityWeapon(updateCommand, row,
                    updateCommand.GetOption(UpdateParamOptionNames.ReplaceInitDexterityWeapon)
                    );

                ReplaceWep(updateCommand, row, "equip_Subwep_Right",
                    updateCommand.GetOption(UpdateParamOptionNames.ReplaceInitWeaponRight2));

                ReplaceWep(updateCommand, row, "equip_Subwep_Right3",
                    updateCommand.GetOption(UpdateParamOptionNames.ReplaceInitWeaponRight3));
            }
        }



        private static void AddCrimsonAmberMedallion(UpdateCommand updateCommand, SoulsParam.Param.Row row)
        {
            for (int i = 1; i <= 4; i++)
            {
                string key = "equip_Accessory0" + i;
                var v = ParamRowUtils.GetCellInt(row, key, 0);
                if (v < 1)
                {
                    updateCommand.AddItem(row, key, 1000);
                    return;
                }
            }
        }

        //item_01
        static int addItemOffset = 0;
        private static void AddItem(UpdateCommand updateCommand, SoulsParam.Param.Row row, int itemId, int itemCount)
        {
            for (int i = 1; i < 10; i++)
            {
                string key = "item_0" + (i);
                string numKey = "itemNum_0" + (i);
                var v = ParamRowUtils.GetCellInt(row, key, 0);
                if (v < 1)
                {
                    key = "item_0" + (i + addItemOffset);
                    numKey = "itemNum_0" + (i + addItemOffset);
                    updateCommand.AddItem(row, key, itemId);
                    updateCommand.AddItem(row, numKey, itemCount);
                    addItemOffset++;
                    return;
                }
            }
        }
        //secondaryItem_01
        //secondaryItemNum_01
        static int addSecondaryItemOffset = 0;
        private static void AddSecondaryItem(UpdateCommand updateCommand, SoulsParam.Param.Row row, int itemId, int itemCount)
        {
            for (int i = 1; i <= 6; i++)
            {
                string key = "secondaryItem_0" + (i);
                string numKey = "secondaryItemNum_0" + (i);
                var v = ParamRowUtils.GetCellInt(row, key, 0);
                if (v < 1)
                {
                    key = "secondaryItem_0" + (i + addSecondaryItemOffset);
                    numKey = "secondaryItemNum_0" + (i + addSecondaryItemOffset);
                    updateCommand.AddItem(row, key, itemId);
                    updateCommand.AddItem(row, numKey, itemCount);
                    addSecondaryItemOffset++;
                    return;
                }
            }
        }
        //
        static void ReplaceBow(UpdateCommand updateCommand, SoulsParam.Param.Row row) {

            int newBowId = updateCommand.GetOption("ReplaceInitBow");
            if (newBowId < 1)
                return;
            foreach (string key in LeftWepKeys) {
                var v = ParamRowUtils.GetCellInt(row, key, 0);
                if (v > 0)
                {
                    if (WeaponConfig.IsBow(v))
                    {
                        updateCommand.AddItem(row, key, newBowId);
                        break;
                    }
                }
                else
                {
                    updateCommand.AddItem(row, key, newBowId);
                    break;
                }
            }
        }
        /* 
equip_Wep_Right
equip_Subwep_Right
equip_Wep_Left
equip_Subwep_Left
         */
        static string[] RightWepKeys = { "equip_Wep_Right", "equip_Subwep_Right", "equip_Subwep_Right3" };
        static string[] LeftWepKeys = { "equip_Wep_Left", "equip_Subwep_Left", "equip_Subwep_Left" };
        private static void ReplaceWep(UpdateCommand updateCommand, SoulsParam.Param.Row row, string key, int itemId) {
            if (itemId < 1)
                return;
            if (removeWeaponDict.ContainsKey(itemId)) {
                return;
            }
            updateCommand.AddItem(row, key, itemId);
            if (updateCommand.HaveOption(UpdateParamOptionNames.RemoveInitWeaponWeightRequire)) {
                RemoveWeightRequire(updateCommand, itemId);
            }
        }

        private static void RemoveWeightRequire(UpdateCommand updateCommand, int itemId) {
            removeWeaponDict[itemId] = 1;
            ParamUpdateRequire.RemoveWeightRequire(updateCommand, itemId);
        }

        /*
        private static void AddWepRight3(UpdateCommand updateCommand, SoulsParam.Param.Row row, int itemId)
        {
            updateCommand.AddItem(row, "equip_Subwep_Right3", itemId);
        }

        private static void AddWepLeft3(UpdateCommand updateCommand, SoulsParam.Param.Row row, int itemId)
        {
            updateCommand.AddItem(row, "equip_Subwep_Left3", itemId);
        }*/

        //equip_Spell_01
        private static void AddSpell(UpdateCommand updateCommand, SoulsParam.Param.Row row, int itemId) {
            for (int i = 1; i < 7; i++) {
                string key = "equip_Spell_0"+i;
                var v = ParamRowUtils.GetCellInt(row, key, 0);
                if (v < 1) {
                    updateCommand.AddItem(row, key,itemId);
                    return;
                }
            }
        }

        private static void ReplaceIntelligenceWeapon(UpdateCommand updateCommand, SoulsParam.Param.Row row, int itemId) {
            if (itemId < 1) {
                return;
            }
            var v = ParamRowUtils.GetCellInt(row, "baseMag", 0);
            if (v >= 12)
            {
                ReplaceWep(updateCommand, row, "equip_Wep_Right", itemId);
                //updateCommand.AddItem(row, "equip_Wep_Right", itemId);
                //33250000;Meteorite Staff;陨石杖
                //4710;Rock Sling;岩石球
                if (itemId == 33250000)
                {
                    AddSpell(updateCommand, row, 4710);
                }
            }
        }

        private static void ReplaceDexterityWeapon(UpdateCommand updateCommand, SoulsParam.Param.Row row, int itemId)
        {
            if (itemId < 1)
            {
                return;
            }
            var v = ParamRowUtils.GetCellInt(row, "baseDex", 0);
            if (v >= 12)
            {
                ReplaceWep(updateCommand, row, "equip_Wep_Right", itemId);
                //updateCommand.AddItem(row, "equip_Wep_Right", itemId);
            }

        }
        /*
        public static void Test(ParamProject paramProject, UpdateCommand updateCommand)
        {
            var param = paramProject.FindParam(ParamNames.CharaInitParam);
            if (param == null)
                return;

            var row = ParamRowUtils.FindRow(param, 3007);
            if (row == null)
                return;
            
             //8590;Whetstone Knife;砥石小刀
             //130;Spectral Steed Whistle;灵马哨笛
             

            AddItem(updateCommand, row, 4, 8590, 1);
            AddItem(updateCommand, row, 5, 8500, 1);     


            updateCommand.AddItem(row, "HpEstMax", 13);
            updateCommand.AddItem(row, "MpEstMax", 1);

        }
        */
        /*
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
        }*/
    }
}
