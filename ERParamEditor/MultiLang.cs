using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using System.Resources;
using SoulsFormats.KF4;
using System.Runtime.InteropServices;

namespace ERParamEditor
{
    public class MultiLang
    {

        //Dictionary<string, CultureInfo> CultureInfo
        static CultureInfo? currentCultureInfo;
        static ResourceManager? resourceManager;
        static string cultureName = "default";
        public static bool Need() {
            if (cultureName == "default")
                return false;
            return true;
        }
        public static void SwitchLanguage() {

            if (CultureInfo.CurrentCulture.Name == "zh-CN")
                cultureName = "zho";

            currentCultureInfo = new CultureInfo(cultureName);


            resourceManager = new ResourceManager("ERParamEditor.Resource.lang.zho",
                typeof(MultiLang).Assembly);
        }

        public static string GetString(string text) {

            if (!Need())
                return text;

            return resourceManager.GetString(text, currentCultureInfo);
        }
    }
}
