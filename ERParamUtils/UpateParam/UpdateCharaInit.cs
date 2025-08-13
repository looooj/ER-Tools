using ERParamUtils.UpateParam;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Tls;
using SoulsFormats.Util;
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
        
        static Dictionary<int,int> removeWeaponRequireDict = new Dictionary<int,int>();
        static Dictionary<string, int> replaceWeaponDict = new Dictionary<string, int>();


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
            removeWeaponRequireDict.Clear();
            bool startFlag = false;
            for (int i = 0; i < 100; i++)
            {
                addItemOffset = 0;
                replaceWeaponDict.Clear();
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
                UpdateLogger.InfoTime("{0} {1}",row.ID,row.Name);
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

                ReplaceShield(updateCommand, row);

                ReplaceStaff(updateCommand, row,
                    updateCommand.GetOption(UpdateParamOptionNames.ReplaceInitStaff)
                    );

                

                ReplaceDexterityWeapon(updateCommand, row,
                    updateCommand.GetOption(UpdateParamOptionNames.ReplaceInitDexterityWeapon)
                    );

                ReplaceWep(updateCommand, row, "equip_Subwep_Right",
                    updateCommand.GetOption(UpdateParamOptionNames.ReplaceInitWeaponRight2));

                ReplaceWep(updateCommand, row, "equip_Subwep_Right3",
                    updateCommand.GetOption(UpdateParamOptionNames.ReplaceInitWeaponRight3));

                ReplaceLeft(updateCommand, row, "equip_Subwep_Left3",
                    updateCommand.GetOption(UpdateParamOptionNames.AddInitWeaponLeft3));

                ReplaceBow(updateCommand, row);
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
        static void ReplaceShield(UpdateCommand updateCommand, SoulsParam.Param.Row row) {

            int newShieldId = updateCommand.GetOption(UpdateParamOptionNames.ReplaceInitShield);
            if (newShieldId < 1)
                return;
            foreach (string key in LeftWepKeys) {
                var v = ParamRowUtils.GetCellInt(row, key, 0);
                if (v > 0)
                {
                    if (WeaponConfig.IsShield(v))
                    {
                        ReplaceWep(updateCommand, row, key, newShieldId);
                        break;
                    }
                }
            }
        }
        /*
46,arrowNum=0 
47,boltNum=0  
48,subArrowNum=0  
49,subBoltNum=0  
12,equip_Arrow=-1
13,equip_Bolt=-1 
14,equip_SubArrow=-1 
15,equip_SubBolt=-1 

50000000;Arrow;箭矢
50010000;Fire Arrow;火箭
         */

        static void AddArrow(UpdateCommand updateCommand, SoulsParam.Param.Row row) {
            updateCommand.AddItem(row, "arrowNum", 99);
            updateCommand.AddItem(row, "equip_Arrow", 50000000);

            updateCommand.AddItem(row, "subArrowNum", 99);
            updateCommand.AddItem(row, "equip_SubArrow", 50010000);
        }
        static void ReplaceBow(UpdateCommand updateCommand, SoulsParam.Param.Row row) {
            int newBowId = updateCommand.GetOption(UpdateParamOptionNames.ReplaceInitBow);
            if (newBowId < 1)
                return;
            foreach (string key in LeftWepKeys)
            {
                var v = ParamRowUtils.GetCellInt(row, key, 0);
                if (v > 0)
                {
                    if (WeaponConfig.IsBow(v)) 
                    {
                        ReplaceWep(updateCommand, row, key, newBowId);
                        AddArrow(updateCommand, row);
                        return;
                    }
                }
            }
            foreach (string key in LeftWepKeys)
            {
                var v = ParamRowUtils.GetCellInt(row, key, 0);
                if (v < 1 && !replaceWeaponDict.ContainsKey(key) )
                {
                    ReplaceWep(updateCommand, row, key, newBowId);
                    AddArrow(updateCommand, row);
                    UpdateLogger.InfoTime("add bow");
                    return;
                }
            }

            UpdateLogger.InfoTime("no space for bow");
        }

        static void ReplaceLeft(UpdateCommand updateCommand, SoulsParam.Param.Row row, string repKey, int repId) {
            if (repId < 1)
                return;

            if ( WeaponConfig.IsSeal(repId))
            foreach (string key in LeftWepKeys)
            {
                var v = ParamRowUtils.GetCellInt(row, key, 0);
                if (v > 0)
                {
                    if ( WeaponConfig.IsSeal(v))
                    {
                        ReplaceWep(updateCommand, row, key, repId);
                        return;
                    }
                }
            }
            ReplaceWep(updateCommand, row, repKey, repId);

        }
        /* 
equip_Wep_Right
equip_Subwep_Right
equip_Wep_Left
equip_Subwep_Left
         */
        static string[] RightWepKeys = { "equip_Wep_Right", "equip_Subwep_Right", "equip_Subwep_Right3" };
        static string[] LeftWepKeys = { "equip_Wep_Left", "equip_Subwep_Left", "equip_Subwep_Left3" };
        private static void ReplaceWep(UpdateCommand updateCommand, SoulsParam.Param.Row row, string key, int itemId) {
            if (itemId < 1)
                return;
            if (replaceWeaponDict.ContainsKey(key) ) {
                UpdateLogger.InfoTime("skip wep {0} {1}", key, itemId);
                return;
            }
            updateCommand.AddItem(row, key, itemId);

            replaceWeaponDict[key] = itemId;

            if (updateCommand.HaveOption(UpdateParamOptionNames.RemoveInitWeaponWeightRequire)) {
                if (removeWeaponRequireDict.ContainsKey(itemId))
                {
                    return;
                }
                RemoveWeightRequire(updateCommand, itemId);
            }
        }

        private static void RemoveWeightRequire(UpdateCommand updateCommand, int itemId) {
            removeWeaponRequireDict[itemId] = 1;
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

        private static void ReplaceStaff(UpdateCommand updateCommand, SoulsParam.Param.Row row, int itemId) {
            if (itemId < 1) {
                return;
            }
            var v = ParamRowUtils.GetCellInt(row, "equip_Wep_Right", 0);
            if (WeaponConfig.IsStaff(v))
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

            var v = ParamRowUtils.GetCellInt(row, "equip_Wep_Right", 0);
            if ( v == 9000000)
            {
                ReplaceWep(updateCommand, row, "equip_Wep_Right", itemId);
            }

            //var v = ParamRowUtils.GetCellInt(row, "baseDex", 0);
            //if (v >= 12)
            //{
            //    ReplaceWep(updateCommand, row, "equip_Wep_Right", itemId);
            //}

        }

    }
}
