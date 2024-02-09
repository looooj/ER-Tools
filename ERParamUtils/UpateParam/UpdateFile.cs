using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpdateParam
{
    public class UpdateFile
    {
        public static string UpdateShopSpec = "shop-spec.txt";
        public static string UpdateRecipeSpec = "recipe-spec.txt";

        public static string UpdateLotMapSpec = "lot-map-spec.txt";
        public static string UpdateLotMapBatch = "lot-map-b.txt";

        public static string UpdateLotEnemySpec = "lot-enemy-spec.txt";
        public static string UpdateLotEnemyBatch = "lot-enemy-b.txt";

        public static string UpdateCharaInit = "chara-init.txt";
        public static string UpdateSpEffect = "sp-effect.txt";

        public static string UpdateItemFile = "update-item.txt";
        public static string UpdateRowFile = "update-row.txt";


        public static void Init(ParamProject project) {

            string dir = project.GetUpdateDir();
            Directory.CreateDirectory(dir);          
        }

        static bool checkVar(string line, Dictionary<string, string> varMap) {

            if (line.StartsWith("@@var@@"))
            {
                string[] items = line.Substring(7).Split("=");

                string varName = items[0].Trim();
                string varValue = items[1].Trim();
                if (varName.Length < 1)
                {
                    return false;
                }
                if (varValue.Length < 1)
                {
                    return false;
                }
                varMap.TryAdd(varName, varValue);
                return true;
            }
            return false;
        }

        static string GetIncludePath(string baseDir, string currentName, string includeName) {
            if (!includeName.EndsWith(".txt"))
                includeName = includeName + ".txt";

            string includePath = Path.Combine(baseDir, includeName);

            if (File.Exists(includePath))
                return includePath;

            includePath = Path.Combine(baseDir, currentName + "-"+ includeName);

            if (File.Exists(includePath))
                return includePath;

            includePath = Path.Combine(GlobalConfig.GetTemplateDir() + "\\commons", includeName);
            if (File.Exists(includePath))
                return includePath;

            includePath = Path.Combine(GlobalConfig.GetTemplateDir() + "\\commons", currentName + "-" + includeName);
            if (File.Exists(includePath))
                return includePath;

            return "";
        }

        public static List<string> Load(ParamProject project, string updateName) {

            List<string> result = new();
            Dictionary<string, string> varMap = new();
            string path = project.GetUpdateFile(updateName);

            if (!File.Exists(path)) {
                UpdateLogger.Info("{0} {1} not found", updateName, path);
                return result;
            }

            string[] lines = File.ReadAllLines(path);

            UpdateLogger.InfoTime("load {0} {1}", updateName, path);

            string? baseDir = Path.GetDirectoryName(path);
            if (baseDir == null)
                baseDir = ".";
            foreach (string lineTmp in lines)
            {
                //
                //  Examples 
                //  lot-enemy-b.txt
                //                      
                //  @@include=arrow.txt  =>  arrow.txt | lot-enemy-b-arrow.txt | commons/arrow.txt
                //   
                //
                string line = lineTmp.Trim();
                if (line.StartsWith("@@include="))
                {
                    string[] ss = line.Split("=");
                    string currentName = Path.GetFileNameWithoutExtension(path);
                    string includeName = ss[1];

                    string includePath = GetIncludePath(baseDir, currentName, includeName);

                    if (includePath.Length < 1 || !File.Exists(includePath))
                    {
                        UpdateLogger.Info("include {0} {1} {2} not found",
                            updateName, includeName,
                            includePath);
                        continue;
                    }
                    else {
                        UpdateLogger.Info("include {0} {1} {2} found",
                            updateName, includeName,
                            includePath);
                    }

                    var includeLines = File.ReadLines(includePath);
                    foreach (string includeLine in includeLines)
                    {
                        if (checkVar(includeLine, varMap)) {
                            continue;
                        }
                        result.Add(includeLine);
                    }
                    continue;
                }

                if (checkVar(line, varMap))
                    continue;

                result.Add(line);
            }
            if (varMap.Count > 0 )
                result = ProcLineVar(result, varMap);
            return result;
        }

        static List<string> ProcLineVar(List<string> lines, Dictionary<string, string> varMap) {

            List<string> result = new();
            foreach (string line in lines) {
                string newLine = ReplaceVar(line, varMap);
                result.Add(newLine);
            }
            return result;
        }

        static string ReplaceVar(string line, Dictionary<string, string> dict)
        {

            foreach (string key in dict.Keys)
            {
                var v = "{{" + key + "}}";
                if ( line.Contains(v))
                     line = line.Replace(v, dict[key]);
            }
            return line;
        }

        public static void ProcIdRange(string idRange, List<int> ids)
        {
            string[] items = idRange.Split('-');
            if (items.Length < 2)
            {
                return;
            }
            if (!int.TryParse(items[0], out int beginId))
            {
                return;
            }
            if (!int.TryParse(items[1], out int endId))
            {
                return;
            }
            for (int id = beginId; id <= endId; id++)
                ids.Add(id);
        }

        public static void ProcId(string line, List<int> ids)
        {

            string[] ss = line.Split(',');
            foreach (string s in ss)
            {
                if (s.Contains("-"))
                {
                    ProcIdRange(s, ids);
                }
                else
                {
                    if (int.TryParse(s, out int id)) {
                        ids.Add(id);
                    }
                }

            }
        }

    }

}
