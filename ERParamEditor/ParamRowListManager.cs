using ERParamUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamEditor
{

    public class RowListItem
    {

        public List<RowWrapper> Rows;
        public int Mode;
        public int RowIndex;
        public RowWrapper CurrentRow;
    }

    public class RowListViewMode {

        public const int DEFAULT = 0;
        public const int FIND = 1;
        public const int REF = 2;
    }

    public class RowListManager
    {

        //static RowListItem? current = null;
        static int index = 0;
        static List<RowListItem> items = new();
        public static RowListItem? GetCurrent()
        {
            if ( index < items.Count)
                 return items[index];
            return null;
        }

        public static void SetCurrentRow(RowWrapper row) {

            RowListItem? item = GetCurrent();
            if ( item != null )
                 item.CurrentRow = row;
        }

        public static RowWrapper? GetCurrentRow() {
            RowListItem? item = GetCurrent();
            if (item != null)
                return item.CurrentRow;
            return null;
        }

        public static void Add(int mode, List<RowWrapper> rows)
        {
            index++;
            RowListItem item;
            if (index >= items.Count)
            {
                item = new();
                items.Add(item);
                index = (items.Count - 1);
            }
            else
            {
                item = items[index];
                //index = index > (items.Count - 1) ? (items.Count - 1) : index;
            }

            item.Mode = mode;
            item.Rows = rows;
        }

        //ParamRowForm form;

        public static bool GoBack()
        {
            if (index < 1)
                return false;
            index--;
            return true;
        }

        internal static void Clear()
        {
            items.Clear();
            index = 0;
            //current = null;
            //throw new NotImplementedException();
        }
    }

}
