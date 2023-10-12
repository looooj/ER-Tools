using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErParamEditor
{
    class ListViewUtils
    {
        static public void AddItem(ListView lv, params object[] args)
        {

            if (args.Length > 0)
            {
                object arg = args[0];
                if (arg == null)
                    return;

                ListViewItem item = new ListViewItem(arg.ToString());
                for (int i = 1; i < args.Length; i++)
                {
                    arg = args[i];
                    if (arg == null)
                        arg = "";
                    item.SubItems.Add(arg.ToString());
                }
                lv.Items.Add(item);
            }
        }
    }
}
