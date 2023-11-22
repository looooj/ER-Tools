using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace ERParamUtils
{
    public class ParamProjectCompare
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static string outputDir="";
        private static string outputName = "";

        static Dictionary<int, SoulsParam.Param.Row> MakeRowDict(SoulsParam.Param param)
        {

            Dictionary<int, SoulsParam.Param.Row> dict = new();
            for (int rowIndex = 0; rowIndex < param.Rows.Count; rowIndex++)
            {
                var row = param.Rows[rowIndex];
                if (!dict.ContainsKey(row.ID))
                {
                    dict.Add(row.ID, row);
                }
                else
                {
                    logger.Info("dup id {0} {1}", rowIndex, row.ID);
                }
            }
            return dict;
        }

        static void CompareParam(SoulsParam.Param param1, SoulsParam.Param param2, List<string> result)
        {
            logger.Info("CompareParam {0}", param1.Name);


            var rowDict1 = MakeRowDict(param1);
            TextFile updateFile = new();
            int changeCount = 0;
            updateFile.Add("@@param={0}", param1.Name);
            for (int rowIndex = 0; rowIndex < param2.Rows.Count; rowIndex++)
            {

                var row2 = param2.Rows[rowIndex];

                bool firstCellFlag = true;


                if (!rowDict1.TryGetValue(row2.ID, out SoulsParam.Param.Row? row1))
                {         
                    updateFile.Add("@@id={0}", row2.ID);
                    changeCount++;
                    updateFile.Add("#new row {0}  {1} {2}", param2.Name, row2.ID, row2.Name);
                    continue;
                }

                //
                for (int i = 0; i < row2.Cells.Count; i++)
                {

                    var cell1 = row1.Cells[i];
                    var cell2 = row2.Cells[i];
                    if (cell1.Def.DisplayType == SoulsFormats.PARAMDEF.DefType.dummy8)
                        continue;
                    if (cell1.Def.InternalName.StartsWith("PAD"))
                        continue;

                    if (cell2.Value.Equals(cell1.Value))
                    {
                        continue;
                    }
                    changeCount++;
                    if (firstCellFlag)
                    {
                        firstCellFlag = false;
                        updateFile.Add("@@id={0}", row2.ID);
                    }
                    updateFile.Add("#diff {0}  {1} {2} {3} {4}->{5}",
                        param2.Name,
                        row2.ID,
                        row2.Name,
                        cell1.Def.InternalName,
                        cell1.Value,
                        cell2.Value
                        );


                    updateFile.Add("{0};{1}",
                        cell2.Def.InternalName,
                        cell2.Value
                        );
                }
            }
            if (changeCount > 0) {
                string fn = outputDir + outputName + "-update-" + param2.Name + ".txt";
                updateFile.Save(fn);

                result.Add(param2.Name);
            }
        }

        public static void CompareProject(string name1, string name2, List<string>? paramNames)
        {


            var proj1 = ParamProjectManager.LoadProject(name1);
            var proj2 = ParamProjectManager.LoadProject(name2);

            if (proj1 == null || proj2 == null)
            {
                return;
            }

            var paramDict1 = proj1.GetParamDict();
            var paramDict2 = proj2.GetParamDict();

            outputDir = proj1.GetDir() + @"\cmp\" + proj2.GetName();
            outputName = @"\cmp-" + proj2.GetName();
            Directory.CreateDirectory(outputDir);

            List<string> result = new();
            foreach (string key in paramDict1.Keys)
            {
                if (paramNames == null || paramNames.Contains(key) )
                {
                    //result.Add(string.Format("#{0}", key));
                    var param1 = paramDict1[key];
                    if (paramDict2.ContainsKey(key))
                    {
                        var param2 = paramDict2[key];
                        CompareParam(param1, param2, result);
                    }
                }

            }


            string fn = proj1.GetDir() + @"\cmp\cmp-" + proj2.GetName() + ".txt";
            Directory.CreateDirectory(Path.GetDirectoryName(fn));
            File.WriteAllLines(fn, result);

        }


    }
}
