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

        public static UpdateCommandItem Create(string paramName,int rowId,string key, string value) {

            var item = new UpdateCommandItem
            {
                ParamName = paramName,
                RowId = rowId,
                Key = key,
                Value = value
            };
            return item;
        }
        public static UpdateCommandItem Create(SoulsParam.Param.Row row, string key, string value)
        {
            return Create(row.GetParam().Name, row.ID, key, value);
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
     * @@id=3007
     * [@@param=]
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
            line = line.Trim();

            if (line.StartsWith("#"))
            {
                return;
            }

            if (line.StartsWith("@@id="))
            {
                _currentRowIds.Clear();
                string[] ss = line.Split('=');
                if (ss.Length >= 2)
                    ProcId(ss[1]);
                return;
            }

            if (line.StartsWith("@@param=") && _customParamName == null)
            {
                _currentRowIds.Clear();
                string[] ss = line.Split('=');
                if (ss.Length >= 2)
                {
                    _currentParamName = ss[1];
                }
                return;
            }



            string paramName = _customParamName == null ? _currentParamName : _customParamName;

            if (!ParamNames.CheckValid(paramName))
            {
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

                    UpdateCommandItem item = new()
                    {
                        ParamName = paramName,
                        RowId = currentRowId,
                        Key = fieldName,
                        Value = value
                    };

                    updateCommand.AddItem(item);
                }
            }
        }

        public static void LoadUpdateRow(string paramName, string updateName, UpdateCommand updateCommand)
        {

            var lines = UpateFile.Load(updateCommand.GetProject(), updateName);
            foreach (var line in lines)
            {
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

    class UpdateRowItemDict {

        Dictionary<int, List<UpdateCommandItem>> _dict = new();

        public void Add(UpdateCommandItem item) {

            if (!_dict.TryGetValue(item.RowId, out List<UpdateCommandItem> items)) {
                items = new();
                _dict.Add(item.RowId, items);
            }
            items.Add(item);
        }

        public Dictionary<int, List<UpdateCommandItem>> GetDict() {
            return _dict;
        }


        public int GetCount()
        {
            int count = 0;
            foreach (var items in _dict.Values) {
                count = count + items.Count;
            }
            return count;
        }
    }

    public class UpdateCommandOption
    {

        public string Name;
        public string Description;

        public UpdateCommandOption(string name)
        {
            Name = name;
            Description = name;
        }

        public UpdateCommandOption(string name, string description) {
            Name = name;
            Description = description;
        }

        public static readonly string ReplaceGoldenSeedSacredTear= "ReplaceGoldenSeedSacredTear";
        public static readonly string ReplaceTalismanPouch = "ReplaceTalismanPouch";
        public static readonly string ReplaceFinger = "ReplaceFinger";
        public static readonly string ReplaceCookbook = "ReplaceCookbook";
        public static readonly string ReplaceDeathroot = "ReplaceDeathroot";
        public static readonly string ReplaceRune = "ReplaceRune";
        public static readonly string ReplaceRuneArc = "ReplaceRuneArc";

        public static readonly string ReplaceStoneswordKey = "ReplaceStoneswordKey";
        public static readonly string ReplaceMemoryStone = "ReplaceMemoryStone";

    }

    public class UpdateCommand
    {

        private Dictionary< string, UpdateRowItemDict > _itemDict = new();
        private ParamProject _paramProject;
        private Dictionary< string, int > _updateOptions = new();


        public UpdateCommand(ParamProject p)
        {
            _paramProject = p;
        }

        public void SetOption(string key, int value) {
            _updateOptions[key] = value;
        }

        public void AddOption(List<string> options) {
            foreach (string key in options) {
                SetOption(key, 1);
            }
        }

        public void AddItem(UpdateCommandItem item)
        {
            if (!_itemDict.TryGetValue(item.ParamName, out UpdateRowItemDict rowItemDict) ) {
                rowItemDict = new();
                _itemDict.Add(item.ParamName, rowItemDict);
            }
            rowItemDict.Add(item);
        }

        public void AddItem(SoulsParam.Param.Row row, string key, int value) {
            AddItem(row, key, value + "");
        }

        public void AddItem(SoulsParam.Param.Row row, string key, string value)
        {
            var item = UpdateCommandItem.Create(row, key, value);
            AddItem(item);
        }


        void Exec(SoulsParam.Param currentParam, UpdateRowItemDict rowDict) {

            var dict = rowDict.GetDict();
            for (int i = 0; i < currentParam.Rows.Count; i++) {
                var row = currentParam.Rows[i];
                if (dict.TryGetValue(row.ID, out List<UpdateCommandItem>? items)) {
                    if (items != null) {

                        foreach (var item in items) {

                            int cellIndex = currentParam.GetCellIndex(item.Key);
                            if (cellIndex < 0) {
                                UpdateLogger.InfoRow("row {0} key {1} not found", row.ID, item.Key);
                                continue;
                            }
                            row.Cells[cellIndex].SetValue(item.Value);
                            UpdateLogger.InfoRow(row, item.Key, item.Value);

                        }

                    }
                }
            }            
        }

        public void Exec(ParamProject project)
        {
            if (project == null)
                return;

            UpdateLogger.InfoTime("===Begin");
            foreach (var paramName in _itemDict.Keys)
            {
                var rowDict = _itemDict[paramName];

                SoulsParam.Param? currentParam = project.FindParam(paramName); ;
                string currentParamName = paramName;
                if (currentParam == null)
                    continue;
                currentParam.MakeCellIndex();
                UpdateLogger.InfoTime("{0} {1}", currentParamName, rowDict.GetCount());
                UpdateLogger.Begin(currentParamName);
                Exec(currentParam, rowDict);
            }
            _itemDict.Clear();

            UpdateLogger.InfoTime("===End");
        }



        public ParamProject GetProject()
        {
            return _paramProject;
        }

        internal bool HaveOption(string configName)
        {
            return _updateOptions.ContainsKey(configName);
        }
    }


}
