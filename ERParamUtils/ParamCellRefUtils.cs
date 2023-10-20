using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{


    public interface IParamCellRefProc  {
        void Proc(RowWrapper sourceRow,
            ParamCellItem paramCellItem, List<RowWrapper> rowWrappers);
    }

    public class ShopLineupParamCellRefProc : IParamCellRefProc
    {
        public void Proc(RowWrapper sourceRow, ParamCellItem paramCellItem, List<RowWrapper> rowWrappers)
        {
            if (sourceRow.GetParam().Name != ParamNames.ShopLineupParam)
                return;



        }
    }

    public class EquipAccessoryParamCellRefProc : IParamCellRefProc
    {
        public void Proc(RowWrapper sourceRow, ParamCellItem paramCellItem, List<RowWrapper> rowWrappers)
        {
            if (sourceRow.GetParam().Name != ParamNames.EquipParamAccessory)
                return;
            var rowId = ParamRowUtils.GetCellInt(sourceRow.GetRow(),"refId",  0);
            if (rowId < 1)
                return;

            var spParam = GlobalConfig.GetCurrentProject().FindParam(ParamNames.SpEffectParam);
            var row = ParamRowUtils.FindRow(spParam, rowId);

            rowWrappers.Add(new RowWrapper(row, spParam));
        }
    }


    public class ParamCellRefUtils
    {

        static IParamCellRefProc[] paramCellRefProcs = {
            new ShopLineupParamCellRefProc(),
            new EquipAccessoryParamCellRefProc()

        };
        public static List<RowWrapper> GetRowWrappers(RowWrapper sourceRow,
            ParamCellItem paramCellItem)
        {

            List<RowWrapper> rowWrappers = new();

            foreach (var p in paramCellRefProcs) {

                p.Proc(sourceRow, paramCellItem, rowWrappers);
            }
            return rowWrappers;
        }
    }
}
