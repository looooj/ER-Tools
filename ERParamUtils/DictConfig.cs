using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class ConfigUtils
    {
        public static void LineToDict(string s, Dictionary<string, string> dict)
        {
            s = s.Trim();
            if (s.StartsWith("#"))
                return;
            s = s.Replace("\x0d", string.Empty);
            s = s.Replace("\x0a", string.Empty);
            int pos = s.IndexOf("=");
            if (pos < 1)
                return;

            
            {
                string key = s.Substring(0, pos);
                string v = s.Substring(pos+1, (s.Length - pos-1));

                v = ReplaceVar(v, dict);
                dict[key] = v;
                dict[key.ToLower()] = v;
            }
        }

        static string ReplaceVar(string val, Dictionary<string, string> dict) {

            foreach (string key in dict.Keys) {
                var v = "${" + key + "}";

                val = val.Replace(v, dict[key]);
            }
            return val;
        }

        public static Dictionary<string, string> LoadDict(string filename)
        {
            Dictionary<string, string> dict = new();
            return LoadDict(filename,dict);
        }

        public static Dictionary<string, string> LoadDict(string filename, Dictionary<string, string> dict)
        {
            string[] lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                LineToDict(lines[i], dict);
            }
            return dict;
        }
    }
    public class DictConfig
    {

        protected Dictionary<string, string> Dict = new();

        public Dictionary<string, string> GetDict() {
            return Dict;
        }

        public void Load(string filename)
        {
            Dict = ConfigUtils.LoadDict(filename,Dict);
        }

        public void Save(string filename) {

            List<string> lines = new();
            foreach(string key in  Dict.Keys) {
                string line = string.Format("{0}={1}", key, Dict[key]);
                lines.Add(line);
            }
            File.WriteAllLines(filename, lines.ToArray());
        }

        public void SetString(string key, string value)
        {
            Dict[key] = value;
            //Dict[key.ToLower()] = value;
        }

        public bool Contains(string key) {
            return Dict.ContainsKey(key);
        }

        public string GetString(string key, string def)
        {
            if (Dict.TryGetValue(key, out string? v1))
            {
                return v1;
            }
            if (Dict.TryGetValue(key.ToLower(), out string? v2))
            {
                return v2;
            }

            return def;
        }
        public int GetInt(string key, int def)
        {
            string v = GetString(key, def + "");

            if (int.TryParse(v, out int i))
            {
                return i;
            }
            return def;
        }

        public string GetKeyValueString() {
            var keys = Dict.Keys;
            var str = "";
            foreach (string key in keys) {
                if (str.Length > 0) {
                    str = str + ";";
                }
                str = str + key;
                str = str + "=";
                str = str + Dict[key];                
            }
            return str;
        }

    }
}
