using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class Tools
    {
        public static void CleanUpdateLog(int minutes) {

            var proj = GlobalConfig.GetCurrentProject();
            if (proj == null) {
                return;
            }
            var d = proj.GetUpdateDir() + "\\logs";

            var logDirList = Directory.GetDirectories(d);

            var now = DateTime.Now;
            for (int i = 0; i < logDirList.Length; i++) { 

                var tmp = logDirList[i];
                DateTime dt = Directory.GetCreationTime(tmp);
                var diff = now - dt;
                if (diff.TotalMinutes > minutes ) { 
                   Directory.Delete(tmp, true );
                }
            }
            
        }
    }
}
