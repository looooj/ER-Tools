using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public  class PathNameUtils
    {
        public static string ConvertName(string name) {

            char[] invalid = Path.GetInvalidFileNameChars();
            string newName = "";

            foreach (char c in name) {

                if ( invalid.Contains(c)) { 
                    newName = newName + '_';
                }
                else
                   newName = newName + c;
            }
            return newName;
        }
    }
}
