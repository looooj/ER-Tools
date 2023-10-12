using ErParamTool;
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
        public static string AssetsDir = "";
        public static string RegulationFileName = "regulation.bin";
        private static string ConfigFile="config.txt";


        private static ParamProject? CurrentProject=null;

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

            config.Load(ConfigFile);

            BaseDir = config.GetString("BaseDir", BaseDir);
            AssetsDir = config.GetString("AssetsDir", AssetsDir);

        }

        public static void Save() { 
        
        }
    }
}
