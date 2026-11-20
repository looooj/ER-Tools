using ERParamUtils.UpdateParam;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERParamUtils.UpateParam
{
     class UpdateShopFile
    {

        public static void GenShopFileLines(string idFile, List<string> result)
        {


            int eqType = 0;
            int price = 1000;
            int amount = 1;
            Dictionary<string, string> varMap = new Dictionary<string, string>();
            var idLines = UpdateFile.LoadIdFile(null, idFile, true);
            foreach (var idLine in idLines)
            {

                if (UpdateFile.ParseVar(idLine.Line, varMap))
                {

                    if (varMap.ContainsKey("price"))
                    {
                        price = int.Parse(varMap["price"]);
                    }
                    if (varMap.ContainsKey("type"))
                    {
                        eqType = int.Parse(varMap["type"]);
                    }
                    if (varMap.ContainsKey("amount"))
                    {
                        amount = int.Parse(varMap["amount"]);
                    }
                    continue;
                }

                if (idLine.Line.StartsWith("@@"))
                    continue;

                //if (idLine == null ) 
                //    continue;
                //if ( int.Parse(idLine.Id) < startId || int.Parse(idLine.Id) > endId) {
                //    continue;
                //}
                var items = idLine.Line.Split(';');
                string name = items[1];
                string tmp = string.Format("0;{0};{1};{2};{3};{4}", idLine.Id, name, eqType, price, amount);

                result.Add(tmp);
            }
        }

        public static void GenShopFile(string[] files, string target)
        {

            var lines = new List<string>();
            foreach (string name in files)
                GenShopFileLines(name, lines);

            File.WriteAllLines(target, lines.ToArray());

        }

        public static void GenShopFiles()
        {

            string t = "";
            t = GlobalConfig.GetCommonsDir() + "\\shop\\shop-grace.txt";
            GenShopFile(new string[] { "shop-grace-t.txt" }, t);

            t = GlobalConfig.GetCommonsDir() + "\\shop\\shop-grace-cer.txt";
            GenShopFile(new string[] { "shop-grace-t.txt" }, t);

            t = GlobalConfig.GetCommonsDir() + "\\shop\\shop-kale.txt";
            GenShopFile(new string[] { "shop-kale-t.txt" }, t);

            t = GlobalConfig.GetCommonsDir() + "\\shop\\shop-kale-cer.txt";
            GenShopFile(new string[] { "shop-kale-t.txt" }, t);


            t = GlobalConfig.GetCommonsDir() + "\\shop\\shop-table.txt";
            GenShopFile(new string[] { "shop-table-t.txt" }, t);

            t = GlobalConfig.GetCommonsDir() + "\\shop\\shop-table-cer.txt";
            GenShopFile(new string[] { "shop-table-t-cer.txt" }, t);


        }
    }
}
