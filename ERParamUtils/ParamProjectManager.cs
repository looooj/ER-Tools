using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class ParamProjectManager
    {

        public static bool CheckProject(string projectName)
        {
            if (projectName == null || projectName.Length < 3)
                return false;

            string fn = GlobalConfig.GetProjectDir(projectName) + @"\" + ParamProject.ConfigName;

            return File.Exists(fn);

        }

        public static ParamProject? OpenProject(string projectName)
        {
            if (!CheckProject(projectName))
                return null;

            ParamProject paramProject = new(projectName);

            paramProject.LoadConfig();
            paramProject.LoadParams();

            GlobalConfig.SetCurrentProject(paramProject);
            SaveLastProject();
            return paramProject;
        }

        private static Regex projectNameRegex = new Regex("^[a-z]{3,}[0-9]?");
        public static string? CheckProjectName(string name)
        {

            if (projectNameRegex.Match(name).Success)
            {
                if (CheckProject(name)) {
                    return "Project Exists!";
                }
                return null;
            }
            
            return "Invalid Project Name, Only Include a-z,0-9 \nExamples abc, abc1, abcd ";
        }

        public static ParamProject CreateProject(string name, string modRegulationPath)
        {

            ParamProject paramProject = new(name);

            paramProject.Create(modRegulationPath);
            Directory.CreateDirectory(paramProject.GetDir());
            paramProject.SaveConfig();
            paramProject.LoadParams();
            paramProject.Init();
            GlobalConfig.SetCurrentProject(paramProject);
            SaveLastProject();

            return paramProject;
        }



        public static List<string> GetProjectList()
        {

            List<string> list = new();


            var dirs = Directory.GetDirectories(GlobalConfig.GetProjectsDir(), "*.*");

            foreach (string d in dirs)
            {

                string fn = d + @"\" + ParamProject.ConfigName;
                if (File.Exists(fn))
                {

                    list.Add(Path.GetFileName(d));
                }
            }
            return list;
        }


        public static void OpenLastProject()
        {

            OpenProject(GetLastProject());
        }

        public static string GetLastProject()
        {

            string fn = GlobalConfig.GetProjectsDir() + @"\lastproject.txt";
            if (!File.Exists(fn)) {
                return "";
            }
            string lastProjectName = File.ReadAllText(fn);

            return lastProjectName;
        }

        public static void SaveLastProject()
        {
            var project = GlobalConfig.GetCurrentProject();
            if (project == null)
            {
                return;
            }
            string lastProjectName = GlobalConfig.GetProjectsDir() + @"\lastproject.txt";

            File.WriteAllText(lastProjectName, project.GetName());
        }

    }
}
