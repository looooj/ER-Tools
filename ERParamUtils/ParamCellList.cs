using FSParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class ParamCellItem
    {
        public int ColIndex { get; set; }
        public string? DisplayName { get => _cell.Def.DisplayName; }
        public string? Key { get => _cell.Def.InternalName; }
        public string? Value { get; set; }
        public string? ValueType { get => GetValueType(); }
        public string? Comment { get; set; }

        private FSParam.Param.Cell _cell;

        public FSParam.Param.Cell GetCell()
        {
            return _cell;
        }

        public void SetCell(FSParam.Param.Cell cell)
        {
            _cell = cell;
        }

        public string GetValueType()
        {
            if (IsEnumType())
            {
                return _cell.Def.InternalType + string.Format("({0})", _cell.Def.DisplayType);
            }
            return _cell.Def.InternalType;
        }

        public bool IsEnumType()
        {
            return (_cell.Def.InternalType.Contains("_"));
        }
    }

    public interface IParamCellItemProc
    {

        bool Proc(FSParam.Param param, FSParam.Param.Row row, ParamCellItem item);
        List<ParamCellItem> End(FSParam.Param param, FSParam.Param.Row row, List<ParamCellItem> items);
    }



    public class ParamCellList
    {

        
        public static List<ParamCellItem> Build(FSParam.Param param, FSParam.Param.Row? row)
        {
            List<ParamCellItem> items = new();
            if (row != null)
            {
                ShopLineParamCellProc shopLineParamCellProc = new();
                LotParamCellProc lotParamCellProc = new();
                for (int i = 0; i < row.Cells.Count; i++)
                {

                    FSParam.Param.Cell cell = row.Cells[i];
                    ParamCellItem item = new ParamCellItem();
                    item.SetCell(cell);
                    item.ColIndex = i;


                    var v = cell.Value;
                    var str = v.ToString();
                    var internalType = cell.Def.InternalType;
                    if (str != null)
                    {
                        item.Value = str;
                        if (item.IsEnumType())
                        {
                            var valueText = ParamFieldMetaManager.FindEnumValueText(internalType, str);
                            if (valueText != str)
                            {
                                item.Value = string.Format("{0}({1})", valueText, str);
                            }
                        }
                    }

                    if (cell.Def.DisplayType == SoulsFormats.PARAMDEF.DefType.dummy8)
                        continue;
                    if (cell.Def.InternalName.StartsWith("PAD"))
                        continue;

                    shopLineParamCellProc.Proc(param, row, item);
                    lotParamCellProc.Proc(param, row, item);
                    items.Add(item);
                }
            }
            return items;

        }



        public static List<ParamCellItem> Build(FSParam.Param param, int rowId)
        {

            FSParam.Param.Row? row = ParamRowUtils.FindRow(param, rowId);
            return Build(param, row);
        }
    }


    public static class ShopCol
    {
        public static readonly int EquipId = 0;
        public static readonly int EquipType = 7;
    }

    class ShopLineParamCellProc : IParamCellItemProc
    {
        public List<ParamCellItem> End(Param param, Param.Row row, List<ParamCellItem> items)
        {

            return items;
        }

        public bool Proc(FSParam.Param param, Param.Row row, ParamCellItem item)
        {
            if (param.Name != ParamNames.ShopLineupParam)
                return true;


            if (item.Key == "equipId")
            {

                int shopEquipType = ParamRowUtils.GetCellInt(row, ShopCol.EquipType, (int)ShopEquipType.None);

                int id = ParamRowUtils.GetCellInt(row, ShopCol.EquipId, 0);

                string name = RowNamesManager.FindShopEquipName(id, shopEquipType);

                item.Comment = name;
            }
            return true;
        }
    }

    class LotParamCellProc : IParamCellItemProc
    {
        public List<ParamCellItem> End(Param param, Param.Row row, List<ParamCellItem> items)
        {
    
            return items;
        }

        public bool Proc(Param param, Param.Row row, ParamCellItem item)
        {

            if ((param.Name != ParamNames.ItemLotParamEnemy && param.Name != ParamNames.ItemLotParamMap))
                return true;

            for (int i = 3; i >= 1; i--)
            {

                //lotItemId01
                if (item.Key == ("lotItemId0" + i))
                {

                    int itemId = ParamRowUtils.GetCellInt(row, "lotItemId0" + i, 0);
                    if (itemId <= 0)
                        return true;

                    int itemType = ParamRowUtils.GetCellInt(row, "lotItemCategory0" + i, 0);

                    string itemName = RowNamesManager.FindEquipName(itemId, itemType);

                    item.Comment = itemName;
                }
            }
            return true;
        }
    }
}
