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
using ERParamUtils.UpateParam;

namespace MultiLangLib
{
    

    public class MultiLang
    {

        static Dictionary<string, Dictionary<string, string>> MessageDict = new();

        static string CurrentLangId = "eng";

        public static void SwitchLanguage(string langId) {



            if (CurrentLangId == langId)
                return;

            MessageDict.Clear();

            CurrentLangId = langId;



            var dirs = Directory.GetDirectories(".\\locales","*.*");
            foreach (string d in dirs) {

                LoadDir(d);
               
            }           

        }

        public static void LoadDir(string dir) {

            var files = Directory.GetFiles(dir);
            foreach (string file in files) {

                var dict = ConfigUtils.LoadDict(file);
                var id = Path.GetFileNameWithoutExtension(file);
                MessageDict.Add(id, dict);
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


        public static void Default() {

            string langId = CultureInfo.CurrentCulture.ThreeLetterISOLanguageName;
            SwitchLanguage(langId);
        }

        public static void ApplyMessage(List<UpdateParamOption> updateParamOptions)
        {
            var id = "UpdateParamTask";
            if (!MessageDict.ContainsKey(id))
                return;
            var dict = MessageDict[id];

            foreach(UpdateParamOption option in updateParamOptions) {
                if (dict.TryGetValue(option.Name, out string? text)) {

                    option.Description = text;
                }
            }
        }

        public static void ApplyMessage(List<UpdateParamTask> updateParamTasks)
        {
            var id = "UpdateParamTask";
            if (!MessageDict.ContainsKey(id))
                return;
            var dict = MessageDict[id];
            
            foreach (UpdateParamTask task in updateParamTasks) {

                var name = task.GetType().Name;
                if (dict.TryGetValue(name, out string? text)) {
                    task.Description = text;
                }
            }
        }
    }
}
