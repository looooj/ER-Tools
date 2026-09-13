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

        const int MP1 = 1;
        const int MP1_HP2 = 2;
        const int MP2 = 3;
        const int MP2_HP4 = 4;
        


        public static string GetNames() {


            return "none,recover mp(+1),recover hp(+2) and mp(+1),recover mp(+2),recover hp(+4) and mp(+2)";
        }

        public static string GetValues() {

            return "0,1,2,3,4";
        }

        static int current=0;
        public static void SetCurrent(UpdateCommand updateCommand)
        {
            current = updateCommand.GetOption(UpdateParamOptionNames.AutoRecover);
        }

        public static bool RecoverMp() { 
            if ( current > 0)
                return true;
            return false;
        }

        public static bool RecoverHp()
        {
            if ( current == MP1_HP2 || current == MP2_HP4)
                return true;
            return false;
        }


        public static string GetSpEffectKeyValues(int v) {

            switch (v) {
                case MP2:
                    return "changeMpPoint;-2";
                case MP2_HP4:
                    return "changeHpPoint;-4;changeMpPoint;-2";
                case MP1:
                    return "changeMpPoint;-1";
                case MP1_HP2:
                    return "changeHpPoint;-2;changeMpPoint;-1";
            }
            return "";
        }

    }
}
