using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpdateParam
{
    public class EquipIdUtilsx
    {

        public static bool IsRune(int itemId)
        {
            if (itemId >= 2900 && itemId <= 2919)
                return true;
            return false;
        }

        public static bool IsSmithingStone(int itemId)
        {

            if (itemId >= 10100 && itemId <= 10200)
                return true;
            return false;
        }

        public static bool IsGlovewort(int itemId)
        {

            if (itemId >= 10900 && itemId <= 10919)
                return true;
            return false;
        }

        public static bool IsArrow(int itemId)
        {

            if (itemId >= 50000000 && itemId <= 53030000)
            {
                return true;
            }
            return false;
        }

        public static bool IsPot(int itemId) {

            if (itemId >= 300 && itemId < 700)
            {
                return true;
            }
            return false;
        }

        //for mod cer
        public static bool IsRemnant(int itemId)
        {

            if (itemId >= 20950 && itemId <= 20956)
                return true;
            return false;

        }
    }

   
}
