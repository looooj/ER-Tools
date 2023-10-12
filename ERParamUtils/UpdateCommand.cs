using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    class UpdateCommandItem
    {

        public string ParamName="?";
        public int RowId=0;
        public string RowName="";
        public string Key="";
        public string Value="";

        public static UpdateCommandItem Parse(string line)
        {

            line = line.Trim();

            string[] fields = line.Split(";");

            if (fields.Length < 5)
            {
                throw new Exception("invalid update command item format");
            }

            UpdateCommandItem item = new();
            item.ParamName = fields[0].Trim();
            item.RowId = int.Parse(fields[1]);
            item.RowName = fields[2].Trim();
            item.Key = fields[3].Trim();
            item.Value = fields[4].Trim();
            return item;
        }

    }

    class UpdateCommand
    {


        List<UpdateCommandItem> items = new();

        public void AddItem(UpdateCommandItem item)
        {
            items.Add(item);
        }

        public void AddFile(ParamProject project, string filename) {

            string path = project.GetUpdateFile(filename);
            var lines = File.ReadAllLines(path);
            
            foreach(string line in lines)
                 AddLine(line);
        }


        void AddLine(string line) {
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


        public void Exec(ParamProject project)
        {
            if (project == null)
                return;

            FSParam.Param? currentParam = null;
            string currentParamName = "";

            foreach (UpdateCommandItem item in items)
            {

                if (item.ParamName != currentParamName) {
                    currentParam = project.FindParam(item.ParamName);
                    if (currentParam != null)
                    {
                        currentParamName = item.ParamName;
                    }
                    else {
                        continue;
                    }
                }
                if (currentParam == null)
                    continue;

                FSParam.Param.Row? row = ParamRowUtils.FindRow(currentParam, item.RowId);
                
                if (row == null) {
                    continue;
                }

                if (item.RowName.Length > 0) {
                    row.Name = item.RowName;
                }
                ParamRowUtils.SetRowValue(row, item.Key, item.Value);
            }

        }

        

    }


}
