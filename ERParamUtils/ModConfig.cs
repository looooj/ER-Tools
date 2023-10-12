using ErParamTool;
using SoulsFormats.XmlExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

//not impl
namespace ERParamUtils
{
    //mod default config

    /*
    class ModType {

        static string Normal="normal";
        static string Rand="rand";
        static string Convergence = "convergence";
    }
    */

    class ModConfigItem {

        public string Type="";
        public string Name ="";
        public string RegulationPath = "";



    }

    class ModConfig
    {
        Dictionary<string, ModConfigItem> itemDict=new();



        public void Load(string configName) {

            itemDict.Clear();

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(configName);

            var modElements = xmlDoc.GetElementsByTagName("mod");

            for (int i = 0; i < modElements.Count; i++) {

                var ele = modElements[i];
                ModConfigItem item = new();
                item.Type = ele.ReadString("type");
                item.Name = ele.ReadString("name");
                item.RegulationPath = ele.ReadString("RegulationPath");

                itemDict.Add(item.Type, item);
            }
           
        }

        public string GetRegulationPath(string modType) {

            if (itemDict.ContainsKey(modType)) {

                return itemDict[modType].RegulationPath;
            }
            return "";
        }
    }


}
