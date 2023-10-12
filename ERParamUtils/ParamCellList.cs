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
        public string? DisplayName { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? Type { get; set; }
        public string? Comment { get; set; }
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
                ShopLineParamCellProc shopLineParamCellProc = new ShopLineParamCellProc();

                for (int i = 0; i < row.Cells.Count; i++)
                {

                    FSParam.Param.Cell cell = row.Cells[i];
                    ParamCellItem item = new ParamCellItem();
                    item.ColIndex = i;
                    item.DisplayName = cell.Def.DisplayName;
                    item.Key = cell.Def.InternalName;
                    item.Type = cell.Def.InternalType;
                    var v = cell.Value;
                    var str = v.ToString();
                    if (str != null)
                        item.Value = str;

                    //if (item.Type.StartsWith("dummy"))
                    //    continue;

                    shopLineParamCellProc.Proc(param, row, item);
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

    class ShopLineParamCellProc : IParamCellItemProc
    {
        public List<ParamCellItem> End(Param param, Param.Row row, List<ParamCellItem> items)
        {
            //throw new NotImplementedException();

            return items;
        }

        public bool Proc(FSParam.Param param, Param.Row row, ParamCellItem item)
        {
            if (param.Name != ParamNames.ShopLineupParam)
                return true;
            

            if (item.Key == "equipId")
            {

                int shopEquipType = ParamRowUtils.GetCellInt(row, ERShopCol.EquipType, (int)ShopEquipType.None);

                int id = ParamRowUtils.GetCellInt(row, ERShopCol.EquipId, 0);

                string name = RowNamesManager.FindShopEquipName(id, shopEquipType);

                item.Comment = name;
            }
            return true;
        }
    }
}
