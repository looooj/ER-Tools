using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static SoulsFormats.GRASS;

namespace ERParamUtils.UpateParam
{

    public class UpdateLoggerItem {

        string Name = "";
        List<string> _lines = new();

        internal List<string> Lines { get => _lines; }


        public UpdateLoggerItem(string name) {

            this.Name = name;
        }

        public void Info(string format, params object[] args) {

            string s = string.Format(format, args);
            _lines.Add(s);
        }

        
        public void Add(string s) {
            _lines.Add(s);
        }
    }

    public class UpdateLogger
    {

        static string currentParamName="";
        static string logDir = @".\logs";
        static string defaultName = "Update";

        static Dictionary<string, UpdateLoggerItem> LoggerDict = new();

        static bool Changed = false;


        public static void SetDir(string d) {
            logDir = d;
        }


        
        public static void Begin(string paramName) {

            currentParamName = paramName;

            GetLogger(paramName).Info("---{0} {1}---", paramName, DateTime.Now.ToString());

        }
        

        public static void Save() {
            if (!Changed)
                return;

            Changed = false;

            string timeId = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string dir = logDir + @"\" + timeId;
            Directory.CreateDirectory(logDir);
            Directory.CreateDirectory(dir);

            foreach (string loggerName in LoggerDict.Keys ) {


                UpdateLoggerItem item = LoggerDict[loggerName];

                string fn = string.Format(@"{0}\{1}.txt",
                    dir,
                    loggerName);

                Directory.CreateDirectory(Path.GetDirectoryName(fn));
                File.WriteAllLines(fn, item.Lines);
            }
        }

        public static void End() {

            currentParamName = "";
        }

        public static UpdateLoggerItem GetLogger(string loggerName) {

            Changed = true;
            if (loggerName == "") {
                loggerName = defaultName;
            }

            if (!LoggerDict.ContainsKey(loggerName)) {
                LoggerDict[loggerName] = new UpdateLoggerItem(loggerName);
            }

            return LoggerDict[loggerName];

        }

        
        public static void InfoTime(string format, params object[] args) {

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ");
            string s = time + string.Format(format, args);

            GetLogger("").Add(s);
        }

        public static void Info(string format, params object[] args)
        {

            string s =  string.Format(format, args);

            GetLogger("").Add(s);
        }


        public static void InfoRow(SoulsParam.Param.Row row, string key, object value) {

            GetLogger(currentParamName).Info("{0},{1} {2}={3}", row.ID, row.Name!=null? row.Name:"?", key, value);

        }

        public static void InfoRow(string format, params object[] args) {
            GetLogger(currentParamName).Info(format, args);
        }

        public static void InfoUpdateCommandItem(UpdateCommandItem item) {

            GetLogger(item.ParamName).Info("{0},{1} {2}={3}", 
                item.RowId, item.RowName, item.Key, item.Value);

        }

    }
}
