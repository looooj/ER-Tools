using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErParamTool
{
    public class ConfigUtils
    {
        public static void LineToDict(string s, Dictionary<string, string> dict, bool keyNameToLower)
        {
            s = s.Trim();
            if (s.StartsWith("#"))
                return;
            s = s.Replace("\x0d", string.Empty);
            s = s.Replace("\x0a", string.Empty);
            var items = s.Split('=');
            if (items.Length == 2)
            {
                string key = items[0];
                if (keyNameToLower)
                    key = items[0].ToLower();
                dict[key] = items[1];
            }
        }

    }

    public class DictConfig
    {

        protected Dictionary<string, string> Dict = new Dictionary<string, string>();

        public void Load(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                ConfigUtils.LineToDict(lines[i], Dict, true);
            }

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
        }

        public string GetString(string key, string def)
        {
            if (Dict.TryGetValue(key.ToLower(), out string? v))
            {
                return v;
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

    }
}
