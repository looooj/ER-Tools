
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;
using static SoulsFormats.PARAMDEF;

namespace ERParamUtils
{
    public class RowWrapper
    {
        public int ID { get => _row.ID; }
        public string Name { get => _row.Name == null ? "" : _row.Name; }

        public FSParam.Param GetParam() {
            return _param;
        }
        public FSParam.Param.Row GetRow()
        {
            return _row;
        }
        private FSParam.Param.Row _row;
        private FSParam.Param _param;

        public RowWrapper(FSParam.Param.Row row, FSParam.Param param)
        {
            _row = row;
            _param = param;
        }
    }

    public class ParamRowUtils
    {


        public static object ConvertValue(string value, DefType defType) {
            switch (defType) {
                case DefType.u8:
                    return byte.Parse(value);
                case DefType.s8:
                    return sbyte.Parse(value);
                case DefType.u16:
                    return UInt16.Parse(value);
                case DefType.s16:
                    return Int16.Parse(value);
                case DefType.u32:
                    return UInt32.Parse(value);
                case DefType.s32:         
                    return Int32.Parse(value);
                case DefType.b32:
                    return bool.Parse(value);
                case DefType.angle32:
                case DefType.f32:
                    return float.Parse(value);
                case DefType.f64:
                    return double.Parse(value);
            }
            return value;
        }
        public static object ConvertValueX(string value, string valueType)
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
                default: {
                        if (valueType.Length > 6)
                            return byte.Parse(value);
                        break;
                    }
            }
            return value;
        }


        public static void SetCellValue(FSParam.Param.Row? row, string keyName, int value)
        {
            SetCellValue(row, keyName, "" + value);
        }

        public static void SetCellValue(FSParam.Param.Row? row, int col, string value)
        {

            if (row == null)
            {
                return;
            }
            if (col >= row.Cells.Count)
                return;

            FSParam.Param.Cell cell = row.Cells[col];

            cell.Value = ConvertValue(value, cell.Def.DisplayType);


        }

        public static void SetCellValue(FSParam.Param.Row? row, string keyName, string value)
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

                    cell.Value = ConvertValue(value, cell.Def.DisplayType);
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


        public static List<RowWrapper> ConvertToRowWrapper(FSParam.Param param, RowFilter[] filters)
        {

            List<RowWrapper> rows = new();
            for (int i = 0; i < param.Rows.Count; i++)
            {
                FSParam.Param.Row row = param.Rows[i];

                bool ok = true;
                foreach (RowFilter filter in filters)
                {
                    if (!filter.DoFilter(param, row))
                    {
                        ok = false;
                        break;
                    }
                }
                if (!ok)
                    continue;
                RowWrapper rowWrapper = new(row,param);

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
        public bool DoFilter(FSParam.Param param, FSParam.Param.Row row)
        {
            if (row.Name == null || row.Name.Length < 1)
                return false;
            return true;
        }
    }
}
