﻿using ERParamUtils;
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

        public string ParamName = "";
        public string Name = "";
        public string ValueType = "";
        public Dictionary<string, string> NameValueDict = new();
        public Dictionary<string, string> ValueNameDict = new();


        public static List<ParamFieldEnum> ParseEnums(XmlDocument doc)
        {
            //

            List<ParamFieldEnum> enums = new();
            XmlElement? root = doc.DocumentElement;
            if (root == null)
                return enums;

            var enumsEle = root["enums"];

            if (enumsEle == null)
                return enums;

            var enumList = enumsEle.GetElementsByTagName("enum");

            for (int i = 0; i < enumList.Count; i++)
            {

                var child = enumList[i];
                if (child == null)
                    continue;

                ParamFieldEnum paramFieldEnum = ParseEnum((XmlElement)child);

                if (paramFieldEnum.NameValueDict.Count > 0)
                    enums.Add(paramFieldEnum);
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
                paramFieldEnum.NameValueDict.Add(name, value);
                paramFieldEnum.ValueNameDict.Add(value,name);
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

        public string GetDisplayName(string internalName)
        {

            //string value = "";
            if (this.fieldNameDict.TryGetValue(internalName, out string? value))
            {
                return value;
            }

            return internalName;
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

        static Dictionary<string, ParamFieldEnum> EnumsDict = new();

        public static void Load()
        {
            EnumsDict.Clear();

            string dir = ParamdexConfig.Get().GetParamMetaDir();
            var files = Directory.GetFiles(dir, "*.xml");
            foreach (var file in files)
            {

                string xml = File.ReadAllText(file);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                var paramEnums = ParamFieldEnum.ParseEnums(doc);
                foreach (var v in paramEnums) {

                    EnumsDict.Add(v.Name, v);
                }

            }
            //GetParamMetaDir

        }

        public static string FindEnumValueText(string valueType, string value) {

            if (EnumsDict.ContainsKey(valueType)) {

                var e = EnumsDict[valueType];
                if (e.ValueNameDict.ContainsKey(value)) {
                    return e.ValueNameDict[value] + "(" + value + ")";
                }
            }
            return value;
        }
    }

}
