using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace ERParamUtils.UpateParam
{
    public class UpdateItemLot
    {

        string paramName = "";
        int currentEquipType = -1;

        public UpdateItemLot(string paramName)
        {
            this.paramName = paramName;
        }

        public UpdateCommandItem CreateItem() {
            UpdateCommandItem item = new();
            item.ParamName = paramName;
            return item;
        }

        public void Proc(string line, UpdateCommand updateCommand)
        {

            if (line.StartsWith("@@et="))
            {
                string[] ss = line.Split('=');

                currentEquipType = Int32.Parse(ss[1]);
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
        
                UpdateCommandItem item;
                if (equipId > 0) {
                    item = CreateItem();
                    item.RowId = int.Parse(rowId);
                    item.Key = string.Format("lotItemId0{0}", lotIndex);
                    item.Value = equipId+"";
                    updateCommand.AddItem(item);
                }

                item = CreateItem();                
                item.RowId = int.Parse(rowId);
                item.Key = string.Format("lotItemCategory0{0}", lotIndex);
                item.Value = currentEquipType+"";

                int lotCount = 1;

                if (ss.Length >= 4)
                {
                    lotCount = Int32.Parse(ss[3]);
                }
                item = CreateItem();
                item.RowId = int.Parse(rowId);
                item.Key = string.Format("lotItemNum0{0}", lotIndex);
                item.Value = lotCount + "";

                int lotPoint = 1000;

                if (ss.Length >= 5)
                {
                    lotPoint = Int32.Parse(ss[4]);
                }
                item = CreateItem();
                item.RowId = int.Parse(rowId);
                item.Key = string.Format("lotItemBasePoint0{0}", lotIndex);
                item.Value = lotPoint + "";

            }
        }


        public static void LoadLotSpecUpdate(string paramName, string updateName, UpdateCommand updateCommand)
        {

            UpdateItemLot itemLot = new(paramName);
            var lines = UpateFile.Load(updateCommand.GetProject(), updateName);
            foreach (string line in lines)
            {
                itemLot.Proc(line, updateCommand);
            }            
        }
    }



    public class UpateLotBatch
    {

        string paramName;
        string currentType;
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
                    item.Value = lastCount+"";
                    updateCommand.AddItem(item);

                    //newLines.Add(
                    //    string.Format("{0};{1};lotItemNum0{2};{3};u8",
                    //     paramName, rowId, index, lastCount)
                    //    );
                    item = CreateItem(rowId);
                    item.Key = string.Format("lotItemBasePoint0{0}", index);
                    item.Value = point + "";
                    updateCommand.AddItem(item);

                    //newLines.Add(
                    //   string.Format("{0};{1};lotItemBasePoint0{2};{3};u16",
                    //      paramName, rowId, index, point)
                    //    );


                }
                index = 2;
                count = 1;
                point = 1000;
            }
        }


        public static void LoadLotBatchUpdate(string paramName, string updateName, UpdateCommand updateCommand)
        {


            UpateLotBatch itemLot = new(paramName);
            var lines = UpateFile.Load(updateCommand.GetProject(), updateName);

            foreach (string line in lines)
            {
                itemLot.Proc(line, updateCommand);
            }
        }

    }

    //
    // change lot count only
    //
    public class DefautItemLot
    {


        private static void SetDefaultLot(string paramName, SoulsParam.Param.Row row, UpdateCommand updateCommand)
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


                //52000000;Bolt;弩箭
                //50000000;Arrow;箭矢
                if (itemId == 52000000 && itemType == (int)EquipType.Weapon ) {
                    updateCommand.AddItem(row, "lotItemId0"+i, 50000000);
                }


                if (SpecEquipConfig.IsFinger(itemId,(EquipType)itemType) )
                {
                    updateCommand.AddItem(row, "lotItemId0" + i, 2909);
                }

                //
                //10010;Golden Seed;黄金种子
                //10020; Sacred Tear; 圣杯露滴
                if ((itemId == 10010 || itemId==10020) && itemType == (int)EquipType.Good)
                {
                    updateCommand.AddItem(row, "lotItemId0" + i, 2919);
                }


                SetItemLotCount(itemId, itemType, itemCount, i, paramName, row, updateCommand);

            }
        }

        private static void SetItemLotCount(int itemId, int itemType, int itemCount, int itemIndex,
            string paramName, SoulsParam.Param.Row row, UpdateCommand updateCommand) {

            int newItemCount = 20;
            int specLotCount = SpecEquipConfig.GetSpec(itemId, (EquipType)itemType);

            if (!(SpecEquipConfig.IsRune(itemId,(EquipType)itemType)
                || SpecEquipConfig.IsSmithingStone(itemId, (EquipType)itemType)
                || (specLotCount > 0)
                || SpecEquipConfig.IsRemnant(itemId, (EquipType)itemType)
                || SpecEquipConfig.IsPhysickRemnant(itemId, (EquipType)itemType)
                || SpecEquipConfig.IsArrow(itemId, (EquipType)itemType)
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
                newItemCount = 2;
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

            UpdateCommandItem item = new();
            item.ParamName = paramName;
            item.RowId = row.ID;
            item.Value = newItemCount + "";
            item.Key = "lotItemNum0"+itemIndex;

            updateCommand.AddItem(item);

        }


        public static void SetDefaultLot(ParamProject project, UpdateCommand updateCommand)
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
                    SetDefaultLot(paramName, row, updateCommand);
                }
            }
        }
    }
}
