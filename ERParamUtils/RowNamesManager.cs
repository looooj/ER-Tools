﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class NameItem {
        public int Id=0;
        public string Name="";

        public NameItem() { 
        }
        public NameItem(int id, string name) {

            Id = id;
            Name = name;
        }
    }


    public class RowNamesManager
    {
        //static bool LoadFlag = false;
  

        static Dictionary<int, Dictionary<int, string>> EquipNameDict = new();



        public static Dictionary<int, string> LoadNames(string paramName) {

            string fn = ParamdexConfig.Get().GetParamNamesDir() + @"\" + paramName + ".txt";
            Dictionary<int, string> names = new(); 
            if (!File.Exists(fn)) {
                return names;
            }            
            string[] array = File.ReadAllLines(fn);// Regex.Split(File.ReadAllText(fn), "\\s*[\\r\\n]+\\s*");
            foreach (string line1 in array)
            {
                string line = line1.Trim();
                if (line.Length > 0)
                {
                    Match match = Regex.Match(line, "^(\\d+) (.+)$");
                    int id = int.Parse(match.Groups[1].Value);
                    names[id] = match.Groups[2].Value;
                }
            }
            return names;
        }


        static void LoadEquipNames() {

            /*
        public static readonly int None = 0;
        public static readonly int Good = 1;
        public static readonly int Weapon = 2;
        public static readonly int Protector = 3;
        public static readonly int Accessory = 4;
        public static readonly int Gem = 5;             */


            EquipNameDict.Add((int)EquipType.Accessory,
                LoadNames("EquipParamAccessory"));
            EquipNameDict.Add((int)EquipType.Gem,
                LoadNames("EquipParamGem"));
            EquipNameDict.Add((int)EquipType.Weapon,
                LoadNames("EquipParamWeapon"));
            EquipNameDict.Add((int)EquipType.Protector,
                LoadNames("EquipParamProtector"));
            EquipNameDict.Add((int)EquipType.Good,
                LoadNames("EquipParamGoods"));

        }

        public static void Load() {
            LoadEquipNames();
        }


        static public string FindEquipNameX(int id) {


            foreach (var k in EquipNameDict.Keys) {

                if (EquipNameDict[k].ContainsKey(id)) {
                    return EquipNameDict[k][id];
                }
            }
            return "";
        }

        static public string FindEquipName(int id, int equipType)
        {

            if (EquipNameDict.ContainsKey(equipType)) {
                if (EquipNameDict[equipType].ContainsKey(id))
                {
                    return EquipNameDict[equipType][id];
                }
            }
            return "";
        }

        static public string FindEquipName(int id, int[] equipTypes)
        {

            foreach (int et in equipTypes)
            {
                string name = FindEquipName(id, et);
                if (name.Length > 0)
                    return name;
            }
            return "";
        }

        static public string FindShopEquipName(int id, int shopEquipType) {

          
            var equipType = EquipTypeUtils.ConvertShopEquipType((ShopEquipType)shopEquipType);

            List<int> equipTyes = new();
            switch (equipType) {

                case EquipType.Good:
                    {
                        equipTyes.Add((int)EquipType.Good);
                        equipTyes.Add((int)EquipType.Weapon);
                        equipTyes.Add((int)EquipType.Accessory);
                        break;
                    }
                default: {
                        equipTyes.Add((int)equipType);
                    } break;
            }

            string name = FindEquipName(id, equipTyes.ToArray());
            return name;
        }
    }
}
