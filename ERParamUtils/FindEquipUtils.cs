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

    public interface IFindEquipHandler
    {

       void Find(int equipId, int equipType, List<FindEquipLocation> result);


    }

    public class FindInLotMap : IFindEquipHandler
    {
        void IFindEquipHandler.Find(int equipId, int equipType, List<FindEquipLocation> result)
        {

            var project = GlobalConfig.GetCurrentProject();
            if (project == null)
                return;

            var param = project.FindParam(ParamNames.ItemLotParamMap);
            if (param == null) {
                return;
            }


            foreach (var row in param.Rows) {

                //0,１：アイテムID,lotItemId01,190,
                for (int i = 0; i < 3; i++) {
                    int itemId = ParamRowUtils.GetCellInt(row, i, 0);
                    if (itemId == equipId) {

                        FindEquipLocation loc = new();
                        loc.ParamName = param.Name;
                        loc.RowId = row.ID;
                        loc.Key = "lotItemId0"+i;
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

        public static void Find(int equipId, int equipType, List<FindEquipLocation> result)
        {
            findHandlers.Add(new FindInLotMap());
            for (int i = 0; i < findHandlers.Count; i++) {
                findHandlers[i].Find(equipId, equipType, result);
            }
            foreach (var loc in result) {

                logger.Info(" {0} {1} {2}", loc.ParamName, loc.RowId, loc.Key);
            }
    }
}
}