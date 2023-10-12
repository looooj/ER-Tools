using FSParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class RowWrapper {
        public int ID { get => Row.ID; }
        public string Name { get => Row.Name==null?"":Row.Name; }

        public FSParam.Param.Row GetRow() {
            return Row;
        }
        private FSParam.Param.Row Row;

        public RowWrapper(FSParam.Param.Row row) {
            this.Row = row;
        }
    }

    public class ParamRowUtils
    {


        public static object ConvertValue(string value, string valueType)
        {

            switch (valueType)
            {
                case "u8":
                    return byte.Parse(value);
                case "s8":
                    return sbyte.Parse(value);
                case "u16":
                    return UInt16.Parse(value);
                case "s16":
                    return Int16.Parse(value);
                case "f32":
                    return Single.Parse(value);
                case "f64":
                    return Double.Parse(value);
                case "b32":
                    return Boolean.Parse(value);
                case "u32":
                    return UInt32.Parse(value);
                case "s32":
                case "x32":
                    return Int32.Parse(value);
                case "fixstr":
                case "fixstrW":
                    return value;
            }
            return value;
        }



        public static void SetRowValue(FSParam.Param.Row? row, string keyName, string value)
        {


            if (row == null)
            {
                return;
            }

            for (int i = 0; i < row.Cells.Count; i++)
            {

                FSParam.Param.Cell cell = row.Cells[i];
                if (cell.Def.InternalName == keyName)
                {

                    cell.Value = ConvertValue(value, cell.Def.InternalType);
                    return;
                }
            }

        }


        public static FSParam.Param.Row? FindRow(FSParam.Param param, int rowId)
        {

            for (int i = 0; i < param.Rows.Count; i++)
            {
                if (param.Rows[i].ID == rowId)
                {
                    return param.Rows[i];
                }
            }
            return null;
        }

        public static int GetCellInt(FSParam.Param.Row row, int col, int defVal)
        {

            var v = row.Cells[col].Value;
            if (v == null)
                return defVal;
            var str = v.ToString();
            if (str == null)
                return defVal;

            return int.Parse(str);
        }

        public static int GetCellInt(FSParam.Param.Row row, string key, int defVal)
        {
            for (int i = 0; i < row.Cells.Count; i++)
            {
                if (row.Cells[i].Def.InternalName == key)
                {
                    return GetCellInt(row, i, defVal);
                }
            }
            return defVal;
        }




        public static string GetCellString(FSParam.Param.Row row, int col, string defVal)
        {

            var v = row.Cells[col].Value;

            if (v == null)
                return defVal;
            var str = v.ToString();
            if (str == null)
                return defVal;

            return str;
        }

        public static string GetCellString(FSParam.Param.Row row, string key, string defVal)
        {
            for (int i = 0; i < row.Cells.Count; i++)
            {
                if (row.Cells[i].Def.InternalName == key)
                {
                    return GetCellString(row, i, defVal);
                }
            }
            return defVal;
        }


        public static List<RowWrapper> ConvertToRowWrapper(FSParam.Param param, RowFilter[] filters) {

            List<RowWrapper> rows = new();
            for (int i = 0; i < param.Rows.Count; i++) {
                FSParam.Param.Row row = param.Rows[i];

                bool ok = true;
                foreach (RowFilter filter in filters) {
                    if (!filter.DoFilter(param,row)) {
                        ok = false;
                        break;
                    }
                }
                if (!ok)
                    continue;
                RowWrapper rowWrapper = new(row);

                rows.Add(rowWrapper);
            }
            return rows;
        }


    }
    public interface RowFilter
    {
        bool DoFilter(FSParam.Param param, FSParam.Param.Row row);
    }

    public class RowBlankNameFiler : RowFilter
    {
        public bool DoFilter(Param param, Param.Row row)
        {
            if (row.Name == null || row.Name.Length < 1)
                return false;
            return true;
        }
    }
}
