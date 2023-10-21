using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{

    public class FindEquipLocation
    {
        public string ParamName;
        public int RowId;
        public string Key;
    }

    public class FindEquipOptions {

        public string Name="";
        public int Id=0;
        public int EquipType;
    }

    public interface IFindEquipHandler
    {

        void Find(FindEquipOptions findEquipOptions, List<FindEquipLocation> result);


    }

    public class FindInLot : IFindEquipHandler
    {
        protected string parmName="";

        protected void SetParamName(string name) {
            parmName = name;
        } 

        void IFindEquipHandler.Find(FindEquipOptions findEquipOptions, List<FindEquipLocation> result)
        {

            var project = GlobalConfig.GetCurrentProject();
            if (project == null)
                return;

            var param = project.FindParam(parmName);
            if (param == null)
            {
                return;
            }

            //if (findEquipOptions.Id > 0)
            foreach (var row in param.Rows)
            {
                for (int i = 0; i < 8; i++)
                {
                    string key = "lotItemId0" + (i + 1);
                    int itemId = ParamRowUtils.GetCellInt(row, key, 0);

                    if (findEquipOptions.Id > 0)
                    if (itemId == findEquipOptions.Id)
                    {
                        FindEquipLocation loc = new();
                        loc.ParamName = param.Name;
                        loc.RowId = row.ID;
                        loc.Key = key;
                        result.Add(loc);
                        return;
                    }

                    //todo fix 
                    if (findEquipOptions.Name.Length > 0)
                        if (row.Name.Contains(findEquipOptions.Name) )
                        {
                            FindEquipLocation loc = new();
                            loc.ParamName = param.Name;
                            loc.RowId = row.ID;
                            loc.Key = "";
                            result.Add(loc);
                            return;
                        }

                }

            }

        }
    }

    public class FindInLotMap : FindInLot {

        public FindInLotMap() {
            SetParamName(ParamNames.ItemLotParamMap);
        }
    }

    public class FindInLotEnemy : FindInLot
    {

        public FindInLotEnemy()
        {
            SetParamName(ParamNames.ItemLotParamEnemy);
        }
    }

    public class FindInShop : IFindEquipHandler
    {
        public void Find(FindEquipOptions findEquipOptions, List<FindEquipLocation> result)
        {
            var project = GlobalConfig.GetCurrentProject();
            if (project == null)
                return;

            var param = project.FindParam(ParamNames.ShopLineupParam);
            if (param == null)
            {
                return;
            }


            foreach (var row in param.Rows)
            {
                int equipId = ParamRowUtils.GetCellInt(row, "equipId",0);
                if (findEquipOptions.Id >0) {
                    if (equipId == findEquipOptions.Id)
                    {
                        FindEquipLocation loc = new();
                        loc.ParamName = param.Name;
                        loc.RowId = row.ID;
                        loc.Key = "equipId";
                        result.Add(loc);
                        return;
                    }
                }
            }
        }
    }


    public class FindEquipUtils
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static List<IFindEquipHandler> findHandlers = new();

        public static void Find(FindEquipOptions findEquipOptions, List<FindEquipLocation> result)
        {
            findHandlers.Add(new FindInLotMap());
            findHandlers.Add(new FindInLotEnemy());
            findHandlers.Add(new FindInShop());

            for (int i = 0; i < findHandlers.Count; i++)
            {
                findHandlers[i].Find(findEquipOptions, result);
            }
            foreach (var loc in result)
            {

                logger.Info(" {0} {1} {2}", loc.ParamName, loc.RowId, loc.Key);
            }
        }


    }
}