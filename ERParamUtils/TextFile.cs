using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    class TextFile
    {
        List<string> lines = new();
        public void Add(string format, params object[] args) {

            lines.Add(string.Format(format, args));
        }

        public void Save(string fn) {

            File.WriteAllLines(fn, lines);
        }

        public void Clear() {
            lines.Clear();
        }

        public List<string> GetLines() {
            return lines;
        }
    }
}
