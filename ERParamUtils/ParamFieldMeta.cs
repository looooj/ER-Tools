using ERParamUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ERParamUtils
{

    public class ParamFieldEnum
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        public string ParamName = "";
        public string Name = "";
        public string ValueType = "";
        public Dictionary<string, string> ValueNameDict = new();
        public List<string> Values = new();


        public static List<ParamFieldEnum> ParseEnums(XmlDocument doc, string filename)
        {
            List<ParamFieldEnum> enums = new();
            XmlElement? root = doc.DocumentElement;
            if (root == null)
                return enums;

            var enumsEle = root["Enums"];

            if (enumsEle == null)
                return enums;

            var enumList = enumsEle.GetElementsByTagName("Enum");

            for (int i = 0; i < enumList.Count; i++)
            {

                var child = enumList[i];
                if (child == null)
                    continue;


                try
                {
                    ParamFieldEnum paramFieldEnum = ParseEnum((XmlElement)child);



                    if (paramFieldEnum.ValueNameDict.Count > 0)
                        enums.Add(paramFieldEnum);
                }
                catch (Exception ex) {

                    logger.Error( ex, string.Format("{0} Enum {1}", filename,i));

                }
            }

            return enums;
        }
        /*
           <Enums>
    <Enum Name="WORLD_MAP_POINT_TEXT_TYPE" type="u8">
        <Option Value="0" Name="Type 0" />
        <Option Value="1" Name="Type 1" />
    </Enum>
  </Enums> 
         */
        public static ParamFieldEnum ParseEnum(XmlElement ele)
        {
            ParamFieldEnum paramFieldEnum = new();
            paramFieldEnum.Name = ele.GetAttribute("Name");
            paramFieldEnum.ValueType = ele.GetAttribute("type");
            var optionEles = ele.GetElementsByTagName("Option");
            for (int i = 0; i < optionEles.Count; i++)
            {
                var child = optionEles[i];
                if (child == null)
                    continue;

                var optionEle = (XmlElement)child;

                var name = optionEle.GetAttribute("Name");
                var value = optionEle.GetAttribute("Value");
                paramFieldEnum.ValueNameDict.Add(value,name);
                paramFieldEnum.Values.Add(value);
            }
            return paramFieldEnum;
        }
    }

    public class ParamFieldMeta
    {

        Dictionary<string, string> fieldNameDict;


        public ParamFieldMeta(Dictionary<string, string> fieldNameDict)
        {
            this.fieldNameDict = fieldNameDict;
        }

        public string GetDisplayName(string internalName,string displayName)
        {

            //string value = "";
            if (this.fieldNameDict.TryGetValue(internalName, out string? value))
            {
                return value;
            }

            return displayName;
        }

        //string internalName;
        //string displayName;

        //public string DisplayName { get => displayName; set => displayName = value; }


        //public static ParamFieldMeta Parse(string xml)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(xml);
        //}

        public static ParamFieldMeta Parse(XmlDocument doc)
        {
            Dictionary<string, string> fieldNameDict = new Dictionary<string, string>();

            XmlElement? root = doc.DocumentElement;
            if (root == null)
                return new ParamFieldMeta(fieldNameDict);

            XmlElement? field = root["Field"];

            if (field == null)
                return new ParamFieldMeta(fieldNameDict);

            for (int i = 0; i < field.ChildNodes.Count; i++)
            {
                //    <equipId AltName="Reference ID" Wiki="ID of the equipment for sale" Refs="EquipParamAccessory(equipType=2),EquipParamGem(equipType=4),EquipParamGoods(equipType=3),EquipParamProtector(equipType=1),EquipParamWeapon(equipType=0)" />
                var node = field.ChildNodes[i];
                if (node == null)
                    continue;
                if (node.NodeType != XmlNodeType.Element)
                    continue;

                XmlAttributeCollection? v = node.Attributes;
                string? internalName = node.Name;
                string? displayName = node.Name;

                if (v != null)
                {
                    var altNameItem = v.GetNamedItem("AltName");
                    if (altNameItem != null && altNameItem.Value != null)
                        displayName = altNameItem.Value;
                }
                fieldNameDict[internalName] = displayName;
            }


            return new ParamFieldMeta(fieldNameDict);
        }
    }


    public class ParamFieldMetaManager
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        static Dictionary<string, ParamFieldEnum> EnumsDict = new();

        static Dictionary<string, ParamFieldMeta> MetaDict = new();

        public static void Load()
        {
            EnumsDict.Clear();

            logger.Info("===LoadMeta");


            string dir = ParamdexConfig.Get().GetParamMetaDir();
            var files = Directory.GetFiles(dir, "*.xml");
            foreach (var file in files)
            {

                string xml = File.ReadAllText(file);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                var paramEnums = ParamFieldEnum.ParseEnums(doc,file);
                foreach (var v in paramEnums) {
                    //logger.Info("AddEnum " + v.Name+ "," + Path.GetFileNameWithoutExtension(file));
                    if (!EnumsDict.ContainsKey(v.Name))
                    {
                        EnumsDict.Add(v.Name, v);
                    }
                }


                var meta = ParamFieldMeta.Parse(doc);
                var metaKey = Path.GetFileNameWithoutExtension(file);
                MetaDict.TryAdd(metaKey, meta);

            }          

        }


        public static string GetEnumText(string valueType) {

            string text = "";
            if (EnumsDict.ContainsKey(valueType))
            {
                var e = EnumsDict[valueType];
                foreach (var v in e.Values) {
                    text = text + string.Format("{0}={1};", v, e.ValueNameDict[v]);
                }
            }
            return text;
        }
        public static string FindEnumValueText(string valueType, string value) {

            if (EnumsDict.ContainsKey(valueType)) {

                var e = EnumsDict[valueType];
                if (e.ValueNameDict.ContainsKey(value)) {
                    return e.ValueNameDict[value];
                }
            }
            return value;
        }

        static string ConvertParamName(string paramName) {
            if (paramName.StartsWith("ItemLotParam"))
            {
                return "ItemLotParam";
            }

            if (paramName.StartsWith("ShopLineupParam"))
            {
                return "ShopLineupParam";
            }
            if (paramName.StartsWith("BehaviorParam"))
            {
                return "BehaviorParam";
            }

            if (paramName == "Bullet" || paramName == "Magic")
            {
                return paramName + "Param";
            }
            return paramName;
        }
        public static ParamFieldMeta? FindFieldMeta(string paramName) {


            paramName = ConvertParamName(paramName);

            if (MetaDict.ContainsKey(paramName)) {
                return MetaDict[paramName];
            }
            return null;
        }
    }

}
