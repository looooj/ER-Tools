using ERParamUtils.UpateParam;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace ERParamUtils.UpdateParam
{
    public class UpdateItemLot
    {

        string paramName = "";
        int currentEquipType = -1;
        int currentLotIndex = -1;
        int currentLotCount = 1;
        int currentGetItemFlagId = -1;
        public UpdateItemLot(string paramName)
        {
            this.paramName = paramName;
        }

        public UpdateCommandItem CreateItem()
        {
            UpdateCommandItem item = new();
            item.ParamName = paramName;
            return item;
        }

        public UpdateCommandItem CreateItem(string rowId)
        {
            UpdateCommandItem item = new();
            item.ParamName = paramName;
            item.RowId = int.Parse(rowId); ;
            return item;
        }

        public void Proc(string line, UpdateCommand updateCommand)
        {

            if (line.StartsWith("@@et="))
            {
                string[] ss = line.Split('=');

                currentEquipType = Int32.Parse(ss[1]);
                currentLotCount = 1;
                return;
            }

            if (line.StartsWith("@@index="))
            {
                string[] ss = line.Split('=');
                currentLotCount = 1;
                currentLotIndex = Int32.Parse(ss[1]);
                return;
            }

            if (line.StartsWith("@@count="))
            {
                string[] ss = line.Split('=');
                currentLotCount = Int32.Parse(ss[1]);
                return;
            }

            if (line.StartsWith("@@getItemFlagId"))
            {
                string[] ss = line.Split('=');
                currentGetItemFlagId = Int32.Parse(ss[1]);
                return;
            }


            if (line.StartsWith("#"))
            {
                return;
            }

            if (currentEquipType == -1)
            {
                return;
            }
            else
            {
                string[] ss = line.Split(';');
                if (ss.Length < 2)
                    return;
                string rowId = ss[0];
                int equipId = Int32.Parse(ss[1]);
                int lotIndex = 1;


                if (ss.Length >= 3)
                    lotIndex = Int32.Parse(ss[2]);

                int lotCount = currentLotCount;

                if (ss.Length >= 4)
                {
                    lotCount = Int32.Parse(ss[3]);
                }

                int lotPoint = 1000;

                if (ss.Length >= 5)
                {
                    lotPoint = Int32.Parse(ss[4]);
                }

                if (currentLotIndex == 0)
                {
                    procOneItem(updateCommand, rowId, 1, equipId + "", currentEquipType + "", lotCount, 1000);
                    procOneItem(updateCommand, rowId, 2, "0", "0", 0, 0);

                }
                else
                {
                    procOneItem(updateCommand, rowId, lotIndex, equipId + "", currentEquipType + "", lotCount, lotPoint);
                }




            }
        }

        public void procOneItem(UpdateCommand updateCommand, string rowId, int index, string equipId, string equipType, int count, int point)
        {
            UpdateCommandItem item = CreateItem(rowId);
            item.Key = string.Format("lotItemId0{0}", index);
            item.Value = equipId;
            updateCommand.AddItem(item);

            item = CreateItem(rowId);
            item.Key = string.Format("lotItemCategory0{0}", index);
            item.Value = equipType;
            updateCommand.AddItem(item);


            item = CreateItem(rowId);
            item.Key = string.Format("lotItemNum0{0}", index);
            item.Value = count + "";
            updateCommand.AddItem(item);

            item = CreateItem(rowId);
            item.Key = string.Format("lotItemBasePoint0{0}", index);
            item.Value = point + "";
            updateCommand.AddItem(item);

            if (currentGetItemFlagId > 0)
            {
                item = CreateItem(rowId);
                item.Key = string.Format("getItemFlagId");
                item.Value = currentGetItemFlagId + "";
                updateCommand.AddItem(item);
                currentGetItemFlagId = -1;
            }
        }


        public static void LoadLotSpecUpdate(string paramName, string updateName, UpdateCommand updateCommand)
        {

            UpdateItemLot itemLot = new(paramName);
            var lines = UpdateFile.Load(updateCommand.GetProject(), updateName);
            foreach (string line in lines)
            {
                itemLot.Proc(line, updateCommand);
            }
        }
    }



    public class UpateLotBatch
    {

        string paramName;
        string currentType = "";
        int index = 1;
        int point = 1000;
        int count = 1;
        public UpateLotBatch(string paramName)
        {
            this.paramName = paramName;
        }

        private UpdateCommandItem CreateItem(string rowId)
        {
            UpdateCommandItem item = new();
            item.ParamName = paramName;
            item.RowId = int.Parse(rowId);
            return item;
        }

        public void Proc(string line, UpdateCommand updateCommand)
        {


            if (line.StartsWith("@@index="))
            {

                string[] ss = line.Split('=');
                index = Int32.Parse(ss[1]);
                return;
            }
            if (line.StartsWith("@@point="))
            {
                string[] ss = line.Split('=');
                point = Int32.Parse(ss[1]);
                return;
            }

            if (line.StartsWith("#"))
                return;

            {
                string[] ss = line.Split('|');
                if (ss.Length < 3)
                {
                    return;
                }
                string[] enemys = ss[0].Split(',');
                string[] equips = ss[1].Split(',');
                string[] types = ss[2].Split(',');

                string[] counts = { };
                if (ss.Length >= 4)
                    counts = ss[3].Split(',');

                if (enemys.Length < 1 || equips.Length < 1 || types.Length < 1)
                    return;
                int lastCount = count;
                string lastEquip = equips[0];
                for (int i = 0; i < enemys.Length; i++)
                {

                    string rowId = enemys[i];

                    string equip = lastEquip;

                    if (i < equips.Length)
                    {
                        equip = equips[i];
                        lastEquip = equip;
                    }

                    if (i < types.Length)
                    {
                        currentType = types[i];
                    }

                    if (i < counts.Length)
                    {
                        lastCount = Int32.Parse(counts[i]);
                    }

                    if (index == 0)
                    {
                        procOneItem(updateCommand, rowId, 1, equip, currentType, lastCount, 1000);
                        procOneItem(updateCommand, rowId, 2, "0", currentType, 0, 0);
                        break;
                    }
                    procOneItem(updateCommand, rowId, index, equip, currentType, lastCount, point);

                    /*
                    UpdateCommandItem item = CreateItem(rowId);
                    item.Key = string.Format("lotItemId0{0}", index);
                    item.Value = equip;
                    updateCommand.AddItem(item);

                    //newLines.Add(
                    //string.Format("{0};{1};lotItemId0{2};{3};s32",
                    //  paramName, rowId, index, equip)
                    //);
                    item = CreateItem(rowId);
                    item.Key = string.Format("lotItemCategory0{0}", index);
                    item.Value = currentType;
                    updateCommand.AddItem(item);

                    //newLines.Add(
                    //    string.Format("{0};{1};lotItemCategory0{2};{3};s32",
                    //      paramName, rowId, index, currentType)
                    //    );

                    item = CreateItem(rowId);
                    item.Key = string.Format("lotItemNum0{0}", index);
                    item.Value = lastCount + "";
                    updateCommand.AddItem(item);

                    //newLines.Add(
                    //    string.Format("{0};{1};lotItemNum0{2};{3};u8",
                    //     paramName, rowId, index, lastCount)
                    //    );
                    item = CreateItem(rowId);
                    item.Key = string.Format("lotItemBasePoint0{0}", index);
                    item.Value = point + "";
                    updateCommand.AddItem(item);

                    if (point > 0) {
                        item = CreateItem(rowId);
                        item.Key = string.Format("enableLuck0{0}", index);
                        item.Value = "1";
                        updateCommand.AddItem(item);
                    }
                    */
                }
                index = 2;
                count = 1;
                point = 1000;
            }
        }

        public void procOneItem(UpdateCommand updateCommand, string rowId, int index, string equipId, string equipType, int count, int point)
        {
            UpdateCommandItem item = CreateItem(rowId);
            item.Key = string.Format("lotItemId0{0}", index);
            item.Value = equipId;
            updateCommand.AddItem(item);

            //newLines.Add(
            //string.Format("{0};{1};lotItemId0{2};{3};s32",
            //  paramName, rowId, index, equip)
            //);
            item = CreateItem(rowId);
            item.Key = string.Format("lotItemCategory0{0}", index);
            item.Value = equipType;
            updateCommand.AddItem(item);

            //newLines.Add(
            //    string.Format("{0};{1};lotItemCategory0{2};{3};s32",
            //      paramName, rowId, index, currentType)
            //    );

            item = CreateItem(rowId);
            item.Key = string.Format("lotItemNum0{0}", index);
            item.Value = count + "";
            updateCommand.AddItem(item);

            //newLines.Add(
            //    string.Format("{0};{1};lotItemNum0{2};{3};u8",
            //     paramName, rowId, index, lastCount)
            //    );
            item = CreateItem(rowId);
            item.Key = string.Format("lotItemBasePoint0{0}", index);
            item.Value = point + "";
            updateCommand.AddItem(item);

            if (point > 0)
            {
                item = CreateItem(rowId);
                item.Key = string.Format("enableLuck0{0}", index);
                item.Value = "1";
                updateCommand.AddItem(item);
            }
        }


        public static void LoadLotBatchUpdate(string paramName, string updateName, UpdateCommand updateCommand)
        {


            UpateLotBatch itemLot = new(paramName);
            var lines = UpdateFile.Load(updateCommand.GetProject(), updateName);

            foreach (string line in lines)
            {
                itemLot.Proc(line, updateCommand);
            }
        }

    }

    //
    //  change lot count and replace equip type
    // 
    public class ItemLotChangeReplace
    {



        //
        // change lot count only
        //
        private static void SetItemLotCount(int itemId, int itemType, int itemCount, int itemIndex,
            SoulsParam.Param.Row row, UpdateCommand updateCommand)
        {

            int newItemCount = 20;
            int specLotCount = SpecEquipConfig.GetSpec(itemId, (EquipType)itemType);

            if (!(specLotCount > 0
                //|| SpecEquipConfig.IsRune(itemId, (EquipType)itemType)
                //|| SpecEquipConfig.IsSmithingStone(itemId, (EquipType)itemType)
                || SpecEquipConfig.IsRemnant(itemId, (EquipType)itemType)
                || SpecEquipConfig.IsPhysickRemnant(itemId, (EquipType)itemType)
                //|| SpecEquipConfig.IsArrow(itemId, (EquipType)itemType)
                || SpecEquipConfig.IsBoluses(itemId, (EquipType)itemType)
                || SpecEquipConfig.IsPot(itemId, (EquipType)itemType)
                || SpecEquipConfig.IsAromatic(itemId, (EquipType)itemType)
                || SpecEquipConfig.IsMaterial(itemId, (EquipType)itemType)
                || SpecEquipConfig.IsMeat(itemId, (EquipType)itemType)
                ))
            {
                return;
            }

            if (SpecEquipConfig.IsRemnant(itemId, (EquipType)itemType)
                || SpecEquipConfig.IsPhysickRemnant(itemId, (EquipType)itemType))
            {
                newItemCount = 3;
            }

            if (SpecEquipConfig.IsArrow(itemId, (EquipType)itemType)
                || SpecEquipConfig.IsAromatic(itemId, (EquipType)itemType)
                )
            {
                newItemCount = 99;
            }

            if (specLotCount > 0)
                newItemCount = specLotCount;

            if (itemCount >= newItemCount)
            {
                return;
            }

            var value = newItemCount + "";
            var key = "lotItemNum0" + itemIndex;
            updateCommand.AddItem(row, key, value);

        }

        private static void SetLotCountRow(SoulsParam.Param.Row row, UpdateCommand updateCommand)
        {

            for (int i = 1; i < 8; i++)
            {
                string key = "lotItemId0" + i;
                int itemId = ParamRowUtils.GetCellInt(row, key, 0);
                if (itemId < 1)
                {
                    if (i >= 2)
                        return;
                    continue;
                }
                key = "lotItemCategory0" + i;
                int itemType = ParamRowUtils.GetCellInt(row, key, 0);
                key = "lotItemNum0" + i;
                int itemCount = ParamRowUtils.GetCellInt(row, key, 0);


                SetItemLotCount(itemId, itemType, itemCount, i, row, updateCommand);

            }
        }

        public static void SetItemLotCount(ParamProject project, UpdateCommand updateCommand)
        {
            string[] paramNames = { ParamNames.ItemLotParamMap, ParamNames.ItemLotParamEnemy };
            foreach (string paramName in paramNames)
            {
                var param = project.FindParam(paramName);
                if (param == null)
                    continue;

                UpdateLogger.Begin(paramName);

                foreach (var row in param.Rows)
                {
                    SetLotCountRow(row, updateCommand);
                }
            }
        }


        //
        // replace by options
        //
        private static void SetLotReplaceRow(SoulsParam.Param.Row row, UpdateCommand updateCommand)
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
                EquipType itemEquipType = (EquipType)itemType;
                int incLotItemNum = 0;

                if (updateCommand.HaveOption(UpdateParamOptionNames.ReplaceBellBearing))
                    if (SpecEquipConfig.isBellBearing(itemId, (EquipType)itemType))
                    {
                        updateCommand.AddItem(row, "lotItemId0" + i, 2919);
                        incLotItemNum = 10;
                    }


                if (updateCommand.HaveOption(UpdateParamOptionNames.ReplaceFinger))
                    if (SpecEquipConfig.IsFinger(itemId, (EquipType)itemType))
                    {
                        updateCommand.AddItem(row, "lotItemId0" + i, 2919);
                        incLotItemNum = 5;
                    }

                if (updateCommand.HaveOption(UpdateParamOptionNames.ReplaceCookbook))
                    if (SpecEquipConfig.IsCookBook(itemId, (EquipType)itemType))
                    {
                        updateCommand.AddItem(row, "lotItemId0" + i, 2919);
                        incLotItemNum = 5;
                    }

                if (updateCommand.HaveOption(UpdateParamOptionNames.ReplaceMapPiece))
                    if (SpecEquipConfig.IsMapPiece(itemId, (EquipType)itemType))
                    {
                        updateCommand.AddItem(row, "lotItemId0" + i, 2919);
                    }

                //for cer mod
                if (updateCommand.HaveOption(UpdateParamOptionNames.IncRemnant))
                {
                    if (SpecEquipConfig.IsRemnant(itemId, (EquipType)itemType))
                    {
                        updateCommand.AddItem(row, "lotItemNum0" + i, 5);
                    }
                    if (SpecEquipConfig.IsPhysickRemnant(itemId, (EquipType)itemType))
                    {
                        updateCommand.AddItem(row, "lotItemNum0" + i, 5);
                    }

                }

                if (updateCommand.GetOption(UpdateParamOptionNames.ReplaceGoldenRune) > 0)
                {
                    var value = updateCommand.GetOption(UpdateParamOptionNames.ReplaceGoldenRune);
                    var eqId = ReplaceGoldenRune.ValueToEquipId(value);
                    var runeValue = ReplaceGoldenRune.GetRuneValue(eqId);
                    if (eqId > 0 && SpecEquipConfig.IsRune(itemId, (EquipType)itemType))
                    {
                        if (itemId < eqId )
                        {
                            updateCommand.AddItem(row, "lotItemId0" + i, eqId);
                        }
                        if (itemId >= 2002951)
                        {
                            var itemRuneValue = ReplaceGoldenRune.GetRuneValue(itemId);
                            if (itemRuneValue < runeValue)
                            {
                                updateCommand.AddItem(row, "lotItemId0" + i, eqId);
                            }
                        }
                        updateCommand.AddItem(row, "lotItemNum0" + i, 10);
                    }

                }
                //10010;Golden Seed;黄金种子
                //10020; Sacred Tear; 圣杯露滴
                if (updateCommand.HaveOption(UpdateParamOptionNames.ReplaceGoldenSeedSacredTear))
                    if ((itemId == 10010
                        || itemId == 10020 ) && itemType == (int)EquipType.Good)
                    {

                        updateCommand.AddItem(row, "lotItemId0" + i, 2919);
                        incLotItemNum = 10;
                    }

                //2010000;Scadutree Fragment;幽影树碎片
                //2010100;Revered Spirit Ash;灵灰
                if (updateCommand.HaveOption(UpdateParamOptionNames.ReplaceScadutreeFragmentSpiritAsh))
                    if ((
                         itemId == 2010000
                        || itemId == 2010100
                        ) && itemType == (int)EquipType.Good)
                    {

                        updateCommand.AddItem(row, "lotItemId0" + i, 2002960);
                        incLotItemNum = 10;

                    }


                //10030;Memory Stone;记忆石
                if (updateCommand.HaveOption(UpdateParamOptionNames.ReplaceMemoryStone))
                    if ((itemId == 10030) && itemType == (int)EquipType.Good)
                    {
                        updateCommand.AddItem(row, "lotItemId0" + i, 2919);
                        incLotItemNum = 10;
                    }


                if (updateCommand.HaveOption(UpdateParamOptionNames.ReplaceStoneswordKey))
                    //8000; Stonesword Key; 石剑钥匙
                    if ((itemId == 8000) && itemType == (int)EquipType.Good)
                    {
                        updateCommand.AddItem(row, "lotItemId0" + i, 2919);
                        incLotItemNum = 5;

                    }

                //10040; Talisman Pouch; 护符皮袋  
                if (updateCommand.HaveOption(UpdateParamOptionNames.ReplaceTalismanPouch))
                    if ((itemId == 10040) && itemType == (int)EquipType.Good)
                    {
                        updateCommand.AddItem(row, "lotItemId0" + i, 2919);
                        incLotItemNum = 10;
                    }
                //2090;Deathroot;死根
                if (updateCommand.HaveOption(UpdateParamOptionNames.ReplaceDeathroot))
                    if ((itemId == 2090) && itemType == (int)EquipType.Good)
                    {
                        updateCommand.AddItem(row, "lotItemId0" + i, 2919);
                        incLotItemNum = 10;
                    }
                //10060; Dragon Heart; 龙心脏
                if (updateCommand.HaveOption(UpdateParamOptionNames.ReplaceDragonHeart))
                    if ((itemId == 10060) && itemType == (int)EquipType.Good)
                    {
                        updateCommand.AddItem(row, "lotItemId0" + i, 2919);
                        incLotItemNum = 5;

                    }

                //190;Rune Arc;卢恩弯弧
                if (updateCommand.HaveOption(UpdateParamOptionNames.ReplaceRuneArc))
                    if ((itemId == 190) && itemType == (int)EquipType.Good)
                    {
                        updateCommand.AddItem(row, "lotItemId0" + i, 2919);
                    }

                //
                if (updateCommand.HaveOption(UpdateParamOptionNames.CrimsonAmberMedallionRestore)) {
                    if ((itemId == 5020 || itemId== 8000) && itemType == (int)EquipType.Accessory)
                    {
                        updateCommand.AddItem(row, "lotItemId0" + i, 2919);
                        incLotItemNum = 10;
                    }
                }

                //for cer mod
                if (itemEquipType == EquipType.Good && 
                    updateCommand.HaveOption(UpdateParamOptionNames.ReplaceRemnant))
                {
                    if (SpecEquipConfig.IsRemnant(itemId, EquipType.Good)
                        || SpecEquipConfig.IsPhysickRemnant(itemId, EquipType.Good))
                    {
                        updateCommand.AddItem(row, "lotItemId0" + i, 2919);
                    }

                }
                if (incLotItemNum > 1) {
                    updateCommand.AddItem(row, "lotItemNum0" + i, incLotItemNum);
                }
            }
        }



        public static void SetLotReplace(ParamProject project, UpdateCommand updateCommand)
        {
            string[] paramNames = { ParamNames.ItemLotParamMap, ParamNames.ItemLotParamEnemy };
            foreach (string paramName in paramNames)
            {
                var param = project.FindParam(paramName);
                if (param == null)
                    continue;

                UpdateLogger.Begin(paramName);

                foreach (var row in param.Rows)
                {
                    SetLotReplaceRow(row, updateCommand);
                }
            }
        }
    }
}
