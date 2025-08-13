using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace ERParamUtils.UpateParam
{
    public class WeaponConfig
    {
        public WeaponConfig() { 
        }

        public static bool IsBow(int eqId) {

            return (eqId >= 40000000 && eqId <= 44000000);
        }

        public static bool IsStaff(int eqId) {

            return (eqId >= 33000000 && eqId < 34000000);
        }

        public static bool IsSeal(int eqId)
        {

            return (eqId >= 34000000 && eqId < 34100000);
        }


        /*
//41000000;Longbow;长弓
//41010000;Albinauric Bow;白金弓
//41020000;Horn Bow;角弓
//41030000;Erdtree Bow;黄金树弓
//41040000;Serpent Bow;蛇弓
//41060000;Pulley Bow;滑轮弓
//41070000;Black Bow;黑弓          
         */
        public static string GetReplaceBowNames() {

            return "None,Longbow,Pulley Bow,Black Bow";
        }
        public static string GetReplaceBowIds()
        {
            return "0,41000000,41060000,41070000";
        }

        //9020000;Hand of Malenia;玛莲妮亚的义手刀
        //17030000;Serpent-Hunter;大蛇狩猎矛
        //3100000;Sacred Relic Sword;神躯化剑
        public static string GetReplaceWeaponRightNames() {

            return "None,Sacred Relic Sword,Hand of Malenia,Serpent-Hunter";
        }

        public static string GetReplaceWeaponRightIds()
        {
            return "None,3100000,9020000,17030000";
        }

        //32140000;Icon Shield;神圣绘画盾

        public static bool IsShield(int eqId) {


            return (eqId <= 32301200 && eqId >=30000000);
        }

        public static string GetShieldNames()
        {
            return "None,Icon Shield";
        }

        public static string GetShieldIds()
        {
            return "None,32140000";
        }

        //34080000;Dragon Communion Seal;龙飨印记
        public static string GetAddWeaponLeftNames()
        {
            return "None,Dragon Communion Seal";
        }

        public static string GetAddWeaponLeftIds()
        {
            return "None,34080000";
        }

        /*
10403;Strength;力气
10404;Dexterity;灵巧
10406;Intelligence;智力
10407;Faith;信仰
10409;Arcane;感应         
         
         */

        //8030000;Bloodhound's Fang;猎犬长牙
        public static string GetReplaceDexterityWeaponNames()
        {
            return "None,Bloodhound's Fang";
        }

        public static string GetReplaceDexterityWeaponIds()
        {
            return "None,8030000";
        }

        //33250000;Meteorite Staff;陨石杖
        //4710;Rock Sling;岩石球

        public static string GetReplaceStaffNames()
        {
            return "None,Meteorite Staff";
        }

        public static string GetReplaceStaffIds()
        {
            return "None,33250000";
        }
    }
}
