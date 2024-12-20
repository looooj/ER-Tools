using ERParamUtils.UpdateParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static SoulsFormats.FFXDLSE;

namespace ERParamUtils
{
    public class ParamProjectManager
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        public static void InitConfig() {

            ParamNameFilter.LoadParamNames();
            SpecEquipConfig.LoadConfig();
            RowNamesManager.Load();
            ParamFieldMetaManager.Load();
        }

        public static bool CheckProject(string projectName)
        {
            if (projectName == null || projectName.Length < 3)
                return false;

            string fn = GlobalConfig.GetProjectDir(projectName) + @"\" + ParamProject.ConfigName;

            return File.Exists(fn);

        }


        public static ParamProject? LoadProject(string projectName, bool initCopy)
        {
            if (!CheckProject(projectName))
                return null;

            ParamProject paramProject = new(projectName);


            paramProject.LoadConfig();

            paramProject.InitCopy(initCopy);

            paramProject.LoadParams();

            return paramProject;
        }

        public static ParamProject? OpenProject(string projectName, bool initCopy)
        {

            ParamProject? paramProject = LoadProject(projectName, initCopy);

            if (paramProject == null)
                return paramProject;

            GlobalConfig.SetCurrentProject(paramProject);
            SaveLastProject();
            return paramProject;
        }

        public static List<string> GetParamNames(string? projectName) {
            var project = GlobalConfig.GetCurrentProject();
            var filter = GlobalConfig.UseParamNameFilter;
            if (project != null ) {
                return project.GetParamNameList(filter);
            }
            if (projectName == null) {
                var projectList = GetProjectList();
                if (projectList.Count == 0)
                    return new List<string>();
                projectName = projectList[0];
            }

            project = LoadProject(projectName,false);
            if ( project != null )
                return project.GetParamNameList(filter);
            return new List<string>();
        }

        private static Regex projectNameRegex = new Regex("^[a-z]{3,}[0-9]?");
        public static string? CheckProjectName(string name)
        {

            if (projectNameRegex.Match(name).Success)
            {
                if (CheckProject(name))
                {
                    return "Project Exists!";
                }
                return null;
            }

            return "Invalid Project Name, Only Include a-z,0-9 \nExamples abc, abc1, abcd ";
        }

        public static ParamProject CreateProject(string name, string modRegulationPath, string templateName)
        {

            ParamProject paramProject = new(name);

            paramProject.SetTemplateName(templateName);
            paramProject.Create(modRegulationPath);
            Directory.CreateDirectory(paramProject.GetDir());
            paramProject.SaveConfig();
            paramProject.LoadParams();
            paramProject.Init();
            GlobalConfig.SetCurrentProject(paramProject);
            SaveLastProject();

            return paramProject;
        }



        public static List<ParamProject> GetProjectList2() {

            List<ParamProject> list = new();


            var dirs = Directory.GetDirectories(GlobalConfig.GetProjectsDir(), "*.*");

            foreach (string d in dirs)
            {

                if (Path.GetFileName(d).StartsWith("_"))
                {
                    continue;
                }

                string fn = d + @"\" + ParamProject.ConfigName;

                if (File.Exists(fn))
                {
                    string name = Path.GetFileName(d);
                    var proj = new ParamProject(name);
                    if (proj == null)
                        continue;

                    proj.LoadConfig();
                    list.Add(proj);
                }
            }

            return list;


        }
        public static List<string> GetProjectList()
        {

            List<string> list = new();


            var dirs = Directory.GetDirectories(GlobalConfig.GetProjectsDir(), "*.*");

            foreach (string d in dirs)
            {

                if (Path.GetFileName(d).StartsWith("_")) {
                    continue;
                }

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

            OpenProject(GetLastProject(),false);
        }

        public static string GetLastProject()
        {

            string fn = GlobalConfig.GetProjectsDir() + @"\lastproject.txt";
            if (!File.Exists(fn))
            {
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

        public static string DeleteProject(string projectName)
        {
            if (GlobalConfig.GetCurrentProject().GetName() == projectName) {
                return "Can not delete current project";
            }
            string oldDir = GlobalConfig.GetProjectsDir() + "\\" + projectName;
            string timeId = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            
            string newDir = GlobalConfig.GetProjectsDir() + "\\_" + projectName +"_"+timeId;
            Directory.Move(oldDir, newDir);
            return "";
        }
    }
}
