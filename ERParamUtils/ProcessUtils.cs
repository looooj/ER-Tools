using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{

    public  class ProcessUtils
    {
        //ref https://studiofreya.com/2011/01/22/open-and-select-file-or-folder-with-explorer-in-c/
        public static void OpenInExplorer(string path)
        {
            string cmd = "explorer.exe";
            string arg = "/select, " + path;
            Process.Start(cmd, arg);
        }
    }
}
