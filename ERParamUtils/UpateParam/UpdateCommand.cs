using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
{
    public class UpdateCommandItem
    {

        public string ParamName = "?";
        public int RowId = 0;
        public string RowName = "";
        public string Key = "";
        public string Value = "";

        public static void Proc(string line, UpdateCommand updateCommand)
        {

            line = line.Trim();
            if (line.StartsWith("#"))
                return;

            string[] fields = line.Split(";");

            if (fields.Length < 5)
            {
                //throw new Exception("invalid update command item format");
                return;
            }

            UpdateCommandItem item = new();
            item.ParamName = fields[0].Trim();
            item.RowId = int.Parse(fields[1]);
            item.RowName = fields[2].Trim();
            item.Key = fields[3].Trim();
            item.Value = fields[4].Trim();

            updateCommand.AddItem(item);

        }


        public static void LoadUpdateItem(string updateName, UpdateCommand updateCommand)
        {

            var lines = UpateFile.Load(updateCommand.GetProject(), updateName);
            foreach (var line in lines)
            {
                Proc(line, updateCommand);
            }
        }
    }

    /*
     * 
     * #@id=3007
     * [#@param=]
     * fieldName;fieldValue
     * fieldName;fieldValue
     * ....
     */
    public class UpdateRow
    {
        static string? _customParamName = null;
        static string _currentParamName = "";
        static List<int> _currentRowIds = new();

        
        public static void SetParamName(string paramName)
        {
            _customParamName = paramName;
        }

        static void ProcId(string line)
        {

            string[] ss = line.Split(',');
            foreach (string s in ss)
            {
                int id = int.Parse(s);
                if (id > 0)
                {
                    _currentRowIds.Add(id);
                }
            }
        }

        public static void Proc(string line, UpdateCommand updateCommand)
        {
            if (line.StartsWith("#@id="))
            {
                _currentRowIds.Clear();
                string[] ss = line.Split('=');
                if (ss.Length >= 2)
                    ProcId(ss[1]);
                return;
            }

            if (line.StartsWith("#@param=") && _customParamName == null )
            {
                _currentRowIds.Clear();
                string[] ss = line.Split('=');
                if (ss.Length >= 2)
                {
                    _currentParamName = ss[1];
                }
                return;
            }

            if (line.StartsWith("#"))
            {
                return;
            }

            string paramName = _customParamName == null ? _currentParamName : _customParamName;

            if (!ParamNames.CheckValid(paramName)) {
                return;
            }

            foreach (int currentRowId in _currentRowIds)
            {
                if (currentRowId > 0)
                {
                    string[] ss = line.Split(';');
                    if (ss.Length < 2)
                        return;
                    string fieldName = ss[0];
                    string value = ss[1];                  

                    UpdateCommandItem item = new();
                    item.ParamName = paramName;
                    item.RowId = currentRowId;
                    item.Key = fieldName;
                    item.Value = value;

                    updateCommand.AddItem(item);
                }
            }
        }

        public static void LoadUpdateRow(string paramName,string updateName, UpdateCommand updateCommand) {

            var lines = UpateFile.Load(updateCommand.GetProject(), updateName);
            foreach (var line in lines) {
                SetParamName(paramName);
                Proc(line, updateCommand);
            }
        }

        public static void LoadUpdateRow(string updateName, UpdateCommand updateCommand)
        {

            var lines = UpateFile.Load(updateCommand.GetProject(), updateName);
            foreach (var line in lines)
            {
                Proc(line, updateCommand);
            }
        }
    }


    public class UpdateCommand
    {

        private List<UpdateCommandItem> _items = new();
        private ParamProject _paramProject;

        
        public UpdateCommand(ParamProject p) {
            _paramProject = p;
        }


        public void AddItem(UpdateCommandItem item)
        {
            _items.Add(item);
        }        

        /*
        public void AddFile(ParamProject project, string filename)
        {

            string path = project.GetUpdateFile(filename);
            var lines = File.ReadAllLines(path);

            foreach (string line in lines)
                AddLine(line);
        }

        
        public void AddLine(string line)
        {
            line = line.Trim();
            if (line.StartsWith("#"))
            {
                return;
            }
            UpdateCommandItem item = UpdateCommandItem.Parse(line);
            if (item == null)
            {
                return;
            }
            items.Add(item);
        }
        */

        public void Exec(ParamProject project)
        {
            if (project == null)
                return;

            FSParam.Param? currentParam = null;
            string currentParamName = "";

            foreach (UpdateCommandItem item in _items)
            {

                if (item.ParamName != currentParamName)
                {
                    currentParam = project.FindParam(item.ParamName);
                    if (currentParam != null)
                    {
                        currentParamName = item.ParamName;
                        UpdateLogger.Begin(currentParamName);
                    }
                    else
                    {
                        continue;
                    }
                }
                if (currentParam == null)
                    continue;

                FSParam.Param.Row? row = ParamRowUtils.FindRow(currentParam, item.RowId);

                if (row == null)
                {
                    continue;
                }

                if (item.RowName.Length > 1)
                {
                    row.Name = item.RowName;
                }
                ParamRowUtils.SetCellValue(row, item.Key, item.Value);

                UpdateLogger.InfoRow(row, item.Key, item.Value);
            }

            _items.Clear();
        }


      
        public ParamProject GetProject()
        {
            return _paramProject;
        }
    }


}
