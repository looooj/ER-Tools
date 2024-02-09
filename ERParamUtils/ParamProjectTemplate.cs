using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class ParamProjectTemplate
    {
        static List<string> templateList = new();
        static string defaultName = "empty";

        public static string GetDefault() {
            return defaultName;
        }
        public static void Init()
        {
            List<string> list = new();

            var dirs = Directory.GetDirectories(GlobalConfig.GetTemplateDir(), "*.*");

            foreach (string d in dirs)
            {

                string name = Path.GetFileName(d);
                if (name != "commons")
                {
                    list.Add(name);
                }
            }
            templateList = list;

        }
        /*
        public static void CopyTemplate(ParamProject project, string templateName)
        {
            if (templateName == "" || templateName == null)
                templateName = "empty";

            var templateDir = GlobalConfig.GetTemplateDir() + "\\" + templateName;

            var files = Directory.GetFiles(templateDir + "\\update", "*.txt");

            var destDir = project.GetUpdateDir();
            Directory.CreateDirectory(destDir);
            foreach (string fn in files)
            {
                File.Copy(fn, destDir + "\\" + Path.GetFileName(fn));
            }

            var optFile = templateDir + "\\update-opt.txt";

            File.Copy(optFile, project.GetDir()+ "\\" + Path.GetFileName(optFile));

        }
        */
        public static List<string> GetList()
        {

            return templateList;
        }
    }
}
