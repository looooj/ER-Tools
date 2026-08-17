using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
{
    public class ParamOptionsFile
    {
        static string currentTag="default";
        static string baseDir = ".";
        static string namePrefix = "update-opt-2-";

        public static List<string> GetList() { 

            var list = new List<string>();
            var files = Directory.EnumerateFiles(baseDir);
            foreach (var file in files) {

                var fn = Path.GetFileNameWithoutExtension(file);
                var ext = Path.GetExtension(file);
                if (ext != ".txt")
                    continue;
                if (fn.StartsWith(namePrefix)) {
                    
                    list.Add(fn.Substring(namePrefix.Length));
                }
            }
            if (list.Count == 0)
                list.Add(currentTag);
            return list;
        }

        public static void SetCurrentTag(string tag) { 

            currentTag = tag;
        }

        public static string GetCurrentTag() {
            return currentTag;
        }


        public static string GetFullName() {
            
            string fn = baseDir + "\\" + namePrefix + currentTag+".txt";
            return fn;
        }

        public static void Init(ParamProject paramProject)
        {
            baseDir = paramProject.GetDir();
        }
    }
}
