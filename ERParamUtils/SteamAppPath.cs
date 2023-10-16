using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VdfFile;

namespace ERParamUtils
{
    public class SteamAppPath
    {

       public  static string GetSteamPath() {

            var key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");

            var path = key.GetValue("SteamPath");

            return path.ToString();
       }


        public static ICollection<string> GetSteamLibPathList() {

            string steamPath = GetSteamPath();
            VdfLibraryfolders libraryfolders = VdfLibraryfolders.Create(steamPath);
            return libraryfolders.GetPaths();

        }

        public static VdfAppManifest GetEldenRingAppManifest() {
            string steamPath = GetSteamPath();
            VdfAppManifest manifest = VdfAppManifest.Create(steamPath, eldenRingAppId);
            return manifest;
        }

        

        static string eldenRingName= @"\steamapps\common\ELDEN RING\Game\eldenring.exe";
        static string eldenRingAppId = "1245620";

        public static string FindEldenRing() {

            var paths = GetSteamLibPathList();
            foreach (var p in paths) {
                var fn = p + eldenRingName;
                if (File.Exists(fn)) {
                    return fn;
                }
            }
            return "";
        }


    }
}
