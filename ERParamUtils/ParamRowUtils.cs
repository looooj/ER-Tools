﻿
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;
using static SoulsFormats.PARAMDEF;
using static SoulsFormats.PMDCL;

namespace ERParamUtils
{
    public class RowWrapper
    {
        public string ParamName { get => _param.Name; }
        public int ID { get => _row.ID; }
        public string Name { get => _name; }

        public SoulsParam.Param GetParam()
        {
            return _param;
        }
        public SoulsParam.Param.Row GetRow()
        {
            return _row;
        }
        private SoulsParam.Param.Row _row;
        private SoulsParam.Param _param;
        private string _name;
        public RowWrapper(SoulsParam.Param.Row row, SoulsParam.Param param)
        {
            _row = row;
            _name = row.Name == null ? "" : row.Name;
            _param = param;
        }

        public void SetName(string name)
        {
            _name = name;
        }
    }

    public class ParamRowUtils
    {


        public static object ConvertValue(string value, DefType defType)
        {
            switch (defType)
            {
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
                default:
                    {
                        if (valueType.Length > 6)
                            return byte.Parse(value);
                        break;
                    }
            }
            return value;
        }

        public static object GetCellValue(ParamProject paramProject, string paramName, int rowId, string key, object defVal)
        {
            var param = paramProject.FindParam(paramName);
            if (param == null)
                return defVal;
            var row = FindRow(param, rowId);
            if (row == null)
                return defVal;
            var cell = FindCell(row, key);
            if (cell == null)
                return defVal;
            return cell.Value;
        }

        public static int GetCellInt(ParamProject paramProject, string paramName, int rowId, string key, int defVal)
        {
            var cell = FindCell(paramProject, paramName, rowId, key);
            return GetCellInt(cell, defVal);
        }

        public static int GetCellInt(SoulsParam.Param.Row row, string keyName, string key, int defVal)
        {
            var cell = FindCell(row, key);
            return GetCellInt(cell, defVal);
        }

        public static int GetCellInt(SoulsParam.Param.Cell? cell, int defVal)
        {
            if (cell == null)
                return defVal;
            var s = cell.Value.ToString();
            if (s != null)
                return int.Parse(s);
            return defVal;
        }

        public static void SetCellValue(ParamProject paramProject, string paramName, int rowId, string keyName, int value)
        {

            var param = paramProject.FindParam(paramName);
            if (param == null)
                return;
            var row = FindRow(param, rowId);
            if (row == null)
                return;
            SetCellValue(row, keyName, value);
        }



        public static void SetCellValue(SoulsParam.Param.Row? row, string keyName, int value)
        {
            SetCellValue(row, keyName, "" + value);
        }

        public static void SetCellValue(SoulsParam.Param.Row? row, string keyValues)
        {

            var a = keyValues.Split(",");
            for (int i = 0; i < a.Length;) {
                string key = a[i];
                i++;
                if (i >= a.Length)
                    break;
                string value = a[i];
                i++;
                SetCellValue(row, key, value);
            }            
        }

        public static void SetCellValue(SoulsParam.Param.Row? row, int col, string value)
        {

            if (row == null)
            {
                return;
            }
            if (col >= row.Cells.Count)
                return;

            SoulsParam.Param.Cell cell = row.Cells[col];
            cell.SetValue(ConvertValue(value, cell.Def.DisplayType));

        }

        public static void SetCellValue(SoulsParam.Param.Row? row, string keyName, string value)
        {
            if (row == null)
            {
                return;
            }

            var cell = FindCell(row, keyName);
            if (cell == null)
                return;

            cell.SetValue(ConvertValue(value, cell.Def.DisplayType));

        }


        public static SoulsParam.Param.Cell? FindCell(ParamProject paramProject, string paramName, int rowId, string key)
        {
            var param = paramProject.FindParam(paramName);
            if (param == null)
                return null;
            var row = FindRow(param, rowId);
            if (row == null)
                return null;
            var cell = FindCell(row, key);
            return cell;
        }


        public static SoulsParam.Param.Cell? FindCell(SoulsParam.Param.Row row, string key)
        {
            int cellIndex = row.GetParam().GetCellIndex(key);
            if (cellIndex >= 0)
                return row.Cells[cellIndex];
            return null;
        }

        public static SoulsParam.Param.Row? FindRow(SoulsParam.Param param, int rowId)
        {

            var row = param.FindRow(rowId);
            return row;
            /*
            for (int i = 0; i < param.Rows.Count; i++)
            {
                if (param.Rows[i].ID == rowId)
                {
                    return param.Rows[i];
                }
            }
            return null;
            */
        }


        public static int GetCellInt(SoulsParam.Param.Row row, int col, int defVal)
        {

            var v = row.Cells[col].Value;
            if (v == null)
                return defVal;
            var str = v.ToString();
            if (str == null)
                return defVal;

            return int.Parse(str);
        }

        public static int GetCellInt(SoulsParam.Param.Row row, string key, int defVal)
        {
            int cellIndex = row.GetParam().GetCellIndex(key);
            if (cellIndex < 0)
                return defVal;
            return GetCellInt(row, cellIndex, defVal);
        }

        public static string GetCellString(SoulsParam.Param.Row row, int col, string defVal)
        {

            var v = row.Cells[col].Value;

            if (v == null)
                return defVal;
            var str = v.ToString();
            if (str == null)
                return defVal;

            return str;
        }

        public static string GetCellString(SoulsParam.Param.Row row, string key, string defVal)
        {
            int cellIndex = row.GetParam().GetCellIndex(key);
            if (cellIndex < 0)
                return defVal;
            return GetCellString(row, cellIndex, defVal);
        }


        public static List<RowWrapper> ConvertToRowWrapper(ParamProject project, SoulsParam.Param param, List<RowFilter> filters,
            List<RowBuilder> builders)
        {

            List<RowWrapper> rows = new();
            for (int i = 0; i < param.Rows.Count; i++)
            {
                SoulsParam.Param.Row row = param.Rows[i];

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
                RowWrapper rowWrapper = new(row, param);
                foreach (RowBuilder builder in builders)
                {
                    builder.Proc(project, rowWrapper);
                }
                rows.Add(rowWrapper);
            }
            return rows;
        }


    }


    public interface RowBuilder
    {

        void Proc(ParamProject paramProject, RowWrapper rowWrapper);
    }

    public class SpEffectSetParamRowBuilder : RowBuilder
    {
        public void Proc(ParamProject paramProject, RowWrapper rowWrapper)
        {
            if (rowWrapper.GetParam().Name != ParamNames.SpEffectSetParam)
                return;

            var param = paramProject.FindParam(ParamNames.SpEffectParam);
            if (param == null)
                return;

            var text = "";
            for (int i = 0; i < 3; i++)
            {

                int spId = ParamRowUtils.GetCellInt(rowWrapper.GetRow(), i, 0);
                if (spId > 0)
                {
                    var row = ParamRowUtils.FindRow(param, spId);
                    if (row == null)
                        continue;
                    if (row.Name != null)
                    {
                        text = text + row.Name + " ";
                    }
                }
            }
            rowWrapper.SetName(text);
        }
    }

    public interface RowFilter
    {
        bool DoFilter(SoulsParam.Param param, SoulsParam.Param.Row row);
    }

    public class RowBlankNameFiler : RowFilter
    {
        public bool DoFilter(SoulsParam.Param param, SoulsParam.Param.Row row)
        {
            if (row.Name == null || row.Name.Length < 1)
                return false;
            return true;
        }
    }

    public class CharaInitFilter : RowFilter
    {
        public bool DoFilter(SoulsParam.Param param, SoulsParam.Param.Row row)
        {
            if ( param.Name == ParamNames.CharaInitParam)
               if (row.Name == null || row.Name.Length < 1)
                   return false;
            return true;
        }
    }
}
