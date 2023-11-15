using ERParamUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class GlobalConfig
    {
        public static string BaseDir=@".";
        public static string AssetsDir = "assets";
        public static string RegulationFileName = "regulation.bin";
        public static bool Debug = false;
        private static string ConfigFile="config.txt";


        private static ParamProject? CurrentProject=null;

        public static bool UseParamNameFilter { get; internal set; }

        public static ParamProject? GetCurrentProject() {
            return CurrentProject;
        }

        public static void SetCurrentProject(ParamProject paramProject) {
            CurrentProject = paramProject;
        }

        public static string GetProjectDir(string name) {

            return GetProjectsDir() + @"\" + name;
        }

        public static string GetProjectsDir()
        {
            return BaseDir + @"\projects";
        }

        public static void Load() {

            DictConfig config = new();

            if (!File.Exists(ConfigFile)) {
                return;
            }

            config.Load(ConfigFile);

            BaseDir = config.GetString("BaseDir", BaseDir);
            AssetsDir = config.GetString("AssetsDir", AssetsDir);
            Debug = (config.GetInt("Debug", 0) != 0);
            UseParamNameFilter = (config.GetInt("UseParamNameFilter", 1) != 0);
        }

        public static void Save() { 
        
        }

        internal static string GetTemplateDir()
        {
            return BaseDir + @"\templates";
        }
    }
}
