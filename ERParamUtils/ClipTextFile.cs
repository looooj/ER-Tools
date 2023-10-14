using ERParamUtils.UpateParam;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class ClipTextFile
    {
        static readonly string Name = "clip-text.txt";

        static List<string> lines=new();

        public static string GetFileName() {

            string dir = ".";
            ParamProject? paramProject = GlobalConfig.GetCurrentProject();
            if (paramProject != null)
                dir = paramProject.GetUpdateDir();
            string fn = dir + @"\" + Name;
            if (!File.Exists(fn)) {
                File.WriteAllText(fn, "");
            }
            return fn;
        }

        public static void Clear() {
            string fn = GetFileName();
            File.WriteAllText(fn, "");
        }

        public static void Load() {
            string fn = GetFileName();
            lines.AddRange(File.ReadLines(fn));
        }

        public static void Save() {
            string fn = GetFileName();
            File.AppendAllLines(fn,lines);
            lines.Clear();
        }

        public static void Add(string s) {
            lines.Add(s);
            Save();
        }
    }
}
