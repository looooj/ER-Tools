using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using System.Resources;
using SoulsFormats.KF4;
using System.Runtime.InteropServices;
using ERParamUtils;
using static SoulsFormats.MSB.Shape.Composite;
using ERParamUtils.UpdateParam;
using ERParamUtils.UpateParam;

namespace MultiLangLib
{
    

    public class MultiLang
    {

        static string engId = "eng";
        static string zhoId = "zho";

        static Dictionary<string, Dictionary<string, string>> MessageDict = new();

        static string CurrentLangId = "";
        static string CurrentLangName = "";

        public static void SwitchLanguage(string langId,string appName) {
            if (langId != zhoId)
                langId = engId;

            if (CurrentLangId == langId)
                return;

            var info =  CultureInfo.GetCultureInfo(langId);
            var langName = info.DisplayName;

            MessageDict.Clear();

            string localesPath = ".\\locales\\" + appName +"\\"+langId;

            
           if (!Directory.Exists(localesPath)) {
                return;
            }
           
            /* 
            var dirs = Directory.GetDirectories(localesPath, "*.*");
            foreach (string d in dirs) {

                LoadDir(d);               
            }
            */
            LoadDir(localesPath);
            CurrentLangId = langId;
            CurrentLangName = langName;

        }

        public static string GetLangId() {
            return CurrentLangId;
        }

        public static string GetLangName()
        {
            return CurrentLangName;
        }


        public static void LoadDir(string dir) {

            var files = Directory.GetFiles(dir);
            foreach (string file in files) {

                var dict = ConfigUtils.LoadDict(file);
                var id = Path.GetFileNameWithoutExtension(file);
                MessageDict[id]=dict;
            }
        }

        public static void ApplyForm(Control control, string id) {
            if (!MessageDict.ContainsKey(id))
                return;

            var dict = MessageDict[id];
            foreach (var key in dict.Keys) {

                ApplyControl(control, key, dict[key]);
            }
        }

        public static void ApplyControl(Control? control, string id, string text)
        {
            if (control == null)
                return;

            if (control.Name == id) {

                control.Text = text;
                return;
            }
            if (!control.HasChildren)
                return;

            foreach (Control child in control.Controls) {
                ApplyControl(child, id, text);
            }
        }


        public static void InitDefault(string appName) {

            //CultureInfo.GetCultures()
            string langId = CultureInfo.CurrentCulture.ThreeLetterISOLanguageName;
            //string langName = CultureInfo.CurrentCulture.DisplayName;
            //only support zho & eng

            SwitchLanguage(langId,appName);
        }

        public static void ApplyMessage(List<UpdateParamOptionItem> updateParamOptions)
        {
            var id = "UpdateParam";
            if (!MessageDict.ContainsKey(id))
                return;
            var dict = MessageDict[id];

            foreach(UpdateParamOptionItem option in updateParamOptions) {
                if (dict.TryGetValue(option.Name, out string? text)) {

                    option.Description = text.Trim();
                }
            }
        }

        public static void ApplyMessage(List<UpdateParamTask> updateParamTasks)
        {
            var id = "UpdateParam";
            if (!MessageDict.ContainsKey(id))
                return;
            var dict = MessageDict[id];
            
            foreach (UpdateParamTask task in updateParamTasks) {

                var name = task.GetType().Name;
                if (dict.TryGetValue(name, out string? text)) {
                    task.Description = text.Trim(); 
                }
            }
        }

        public static string GetUpdateParamText(string text)
        {
            return GetText2("UpdateParam", text);
        }

        public static string GetText2(string id, string text) {

            if (!MessageDict.ContainsKey(id))
                return text;
            var dict = MessageDict[id];
            if (dict.TryGetValue(text, out string? newText)) {
                return newText;
            }
            return text;
        }

        public static string GetText(string id, string key, string text)
        {

            if (!MessageDict.ContainsKey(id))
                return text;
            var dict = MessageDict[id];
            if (dict.TryGetValue(key, out string? newText))
            {
                return newText;
            }
            return text;
        }

        public static string GetDefaultText(string key,string text)
        {
            return GetText("Default", key, text);
        }
    }
}
