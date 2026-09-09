using ERParamUtils.UpdateParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils.UpateParam
{
    public class AutoRecoverConfig
    {

        public static string GetNames() {


            return "none,recover mp(+2),recover hp(+4),recover hp(+4) and mp(+2)";
        }

        public static string GetValues() {

            return "0,1,2,3";
        }

        static int current=0;
        public static void SetCurrent(UpdateCommand updateCommand)
        {
            current = updateCommand.GetOption(UpdateParamOptionNames.AutoRecover);
        }

        public static bool RecoverMp() { 
            if ( current == 1 || current == 3 )
                return true;
            return false;
        }

        public static bool RecoverHp()
        {
            if ( current == 2 || current == 3)
                return true;
            return false;
        }


        public static string GetSpEffectKeyValues(int v) {

            switch (v) {
                case 1:
                    return "changeMpPoint;-2";
                case 2:
                    return "changeHpPoint;-4";
                case 3:
                    return "changeHpPoint;-4;changeMpPoint;-2";
            }
            return "";
        }

    }
}
