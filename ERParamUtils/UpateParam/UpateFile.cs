using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
{
    public class UpateFile
    {
        public static string UpdateShopSpec = "shop-spec.txt";

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

        public static List<string> Load(ParamProject project, string updateName) {

            List<string> result = new();
            string path = project.GetUpdateFile(updateName);

            if (!File.Exists(path)) {
                UpdateLogger.Info("{0} {1} not found", updateName, path);
                return result;
            }

            string[] lines = File.ReadAllLines(path);

            UpdateLogger.InfoTime("load {0} {1}", updateName, path);

            string baseDir = Path.GetDirectoryName(path);
            foreach (string line in lines)
            {
                //
                //  Examples 
                //  lot-enemy-b.txt
                //  @@include=arrow.txt  => lot-enemy-b-arrow.txt
                //   
                //
                if (line.StartsWith("@@include="))
                {
                    string[] ss = line.Split("=");
                    string includeName = Path.GetFileNameWithoutExtension(path) + "-" + ss[1];
                    string includePath = Path.Combine(baseDir, includeName);

                    if (!File.Exists(includePath))
                    {
                        UpdateLogger.Info("include {0} {1} {2} not found", 
                            updateName, includeName,
                            includePath);
                        continue;
                    }

                    result.AddRange(File.ReadLines(includePath));
                    continue;
                }
                result.Add(line.Trim());
            }

            return result;
        }
        
    }
}
