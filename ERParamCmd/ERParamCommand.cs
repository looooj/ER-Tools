using ERParamUtils;
using ERParamUtils.UpateParam;
using ERParamUtils.UpdateParam;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ERParamCmd
{



    class ERParamCommand
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static string ProgName = "ERParamCmd";

        static string[] HelpLines = {
            "{0} [options] cmd",
            //"{0} --mod mod_path --project=proj_name  [--template templ_name]  create",
            "{0} --project proj_name publish",
        };


        static Dictionary<string, string> options = new();
        static string command = "";

        public static void PrintHelp(string errorMessage) {

            if (errorMessage != null) {
                Console.WriteLine(errorMessage);
            }

            foreach(string line in HelpLines)
            {

                Console.WriteLine( line, ProgName, "[options]", "cmd");

            }

        }


        static void ParseArgs(string[] args) {

            if (args.Length < 1) {
                PrintHelp("");
                return;
            }

            int argState = 0;
            string argName = "";
            foreach(string arg in args) {
                
                if (arg.StartsWith("--")) {
                    if (argState != 0) {
                        PrintHelp("invalid arg");
                        return; 
                    }
                    argState = 1;
                    argName = arg.Substring(2);
                    continue;
                }
                if (argState == 1)
                {
                    argState = 0;
                    options.TryAdd(argName, arg);
                    argName = "";
                }
                else {
                    command = arg;
                }            
            }

            if (command == "create") {

                CreateProject();
                return;
            }

            if (command == "publish")
            {
                PublishProject();
                return;
            }
        }

        static void CreateProject() {

            if (!options.TryGetValue("project", out string? projectName))
            {

                PrintHelp("create need project arg");
                return;

            }

            if (!options.TryGetValue("mod", out string? modPath))
            {
                PrintHelp("creatre need mod arg");
                return;
            }

            if (!options.TryGetValue("template", out string? templateName)) {
                templateName = "empty";
            }




        }


        static string optionsFile = "update-opt.txt";

        static List<string>? loadOptions(ParamProject paramProject)
        {
            string fn = paramProject.GetDir() + @"\" + optionsFile;

            List<string> lines = new();
            if (!File.Exists(fn))
            {
                return null;
            }

            lines.AddRange(File.ReadLines(fn));
            return lines;
        }

        static void PublishProject()
        {

            if (!options.TryGetValue("project", out string? projectName)) {
                PrintHelp("publish need project arg");
                return;
            }


            GlobalConfig.Load();

            ParamProjectManager.InitConfig();

            var paramProj = ParamProjectManager.LoadProject(projectName,false);

            if (paramProj == null) {
                Console.WriteLine("load project [{0}] fail", projectName);
                return;
            }

            List<UpdateParamTask> updateParamTasks = new();
            List<UpdateParamOptionNames> updateParamOptions = new();

            UpdateParamExector.CreateOptionList(updateParamOptions);
            UpdateParamExector.CreateTaskList(updateParamTasks);

            var saveOptions = loadOptions(paramProj);
            if (saveOptions == null) {
                saveOptions = new();
            }

            UpdateParamExecOptions execOptions = new(); ;

            execOptions.Restore = true;
            execOptions.Publish = true;

            List<string> optionsOutput = new();

            for (int i = 0; i < updateParamTasks.Count; i++)
            {
                string name = updateParamTasks[i].GetType().Name;
                optionsOutput.Add(name);
                if (saveOptions.Contains(name))
                {
                    execOptions.AddTask(updateParamTasks[i]);

                    logger.Info("add param task {0}", name);

                }

            }

            for (int i = 0; i < updateParamOptions.Count; i++)
            {
                string name = updateParamOptions[i].Name;
                optionsOutput.Add(name);
                if (saveOptions.Contains(name))
                {
                    execOptions.AddUpdateCommandOption(name);
                    logger.Info("add param option {0}", name);
                }
            }

            try
            {
                File.WriteAllLines(".\\logs\\all-opts.txt",optionsOutput);

                logger.Info("exec...");
                UpdateParamExector.Exec(paramProj, execOptions);
                logger.Info("exec complete");

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }

        public static void Run(string[] args) {


            ParseArgs(args);
        }
    }
}
