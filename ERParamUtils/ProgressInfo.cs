using ERParamUtils.UpdateParam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class ProgressInfo
    {
        static readonly string tag="current";
        ConcurrentDictionary<string, string> infoTextDict = new();


        static ProgressInfo prog = InitGlobal();
        
        static ProgressInfo InitGlobal() {
            var prog = new ProgressInfo();
            prog.Info("");
            return prog;
        }

        public static ProgressInfo GetGlobal() {
            return prog;
        }

        public void Info(string format, params object[] args) {
            string s = string.Format(format, args);

            infoTextDict[tag] = s;
        }

        public void InfoTime(string format, params object[] args)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ");
            string s = time + string.Format(format, args);

            infoTextDict[tag] = s;
        }

        public string GetText()
        {
            return infoTextDict[tag];
        }
    }
}
