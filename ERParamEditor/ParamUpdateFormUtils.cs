using ERParamUtils;
using ERParamUtils.UpateParam;
using ERParamUtils.UpdateParam;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamEditor
{
    public class ParamUpdateFormUtils
    {
        static ParamProject paramProject;

        public static void Init(ParamProject project) { 
            paramProject = project;
        }
        
        //static string optionsFile = "update-opt-2.txt";
        //static string optionsFilePrefix = "update-opt-2";


        //public static void SetOptionsTag(string tag) { 
        //    optionsFile = optionsFilePrefix+tag + ".txt";
        //}

        //public static string GetOptionsFileName() {
        //
        //   string fn = paramProject.GetDir() + @"\" + optionsFile;
        //
        //    return fn;
        //}

        public static DictConfig loadOptions()
        {
            DictConfig dictConfig = new DictConfig();

            string fn = ParamOptionsFile.GetFullName();//  GetOptionsFileName(); //paramProject.GetDir() + @"\" + optionsFile;

            if (!File.Exists(fn))
            {
                return dictConfig;
            }
            dictConfig.Load(fn);
            return dictConfig;
        }

        public static void applyOptions(List<CustomTablePanel> customTablePanels, DictConfig currentConfig) {

            for (int i = 0; i < customTablePanels.Count; i++)
            {
                customTablePanels[i].SetValues(currentConfig);
            }
        }

        public static DictConfig saveOptions(List<CustomTablePanel> customTablePanels)
        {

            DictConfig config = new();
            for (int i = 0; i < customTablePanels.Count; i++)
            {
                customTablePanels[i].GetValues(config);
            }
            saveOptions(config);
            return config;
        }


        public static void saveOptions(DictConfig dictConfig)
        {

            string fn = ParamOptionsFile.GetFullName();// GetOptionsFileName(); //paramProject.GetDir() + @"\" + optionsFile;
            dictConfig.Save(fn);

            paramProject.SetLastOptionsTag(ParamOptionsFile.GetCurrentTag());
            paramProject.SaveConfig();
        }



        public static void ExecUpdatePublish(Form form,string? msg, bool publish,DictConfig dictConfig)
        {

            Tools.CleanUpdateLog(30);

            if (msg != null)
            {
                DialogResult r = MessageBox.Show(msg, "", MessageBoxButtons.YesNo);
                if (r != DialogResult.Yes)
                {
                    return;
                }
            }

            var updateParamTasks = UpdateParamExector.GetTaskList();
            var updateExecOptions = new UpdateParamExecOptions();
            for (int i = 0; i < updateParamTasks.Count; i++)
            {
                if (dictConfig.Contains(updateParamTasks[i].UpdateName)) {
                    updateExecOptions.AddTask(updateParamTasks[i]);
                }
            }
            updateExecOptions.AddUpdateCommandOption(dictConfig);            

            saveOptions(dictConfig);

            updateExecOptions.Restore = true;
            updateExecOptions.Publish = publish;
            try
            {
                form.Cursor = Cursors.WaitCursor;
                UpdateParamExector.Exec(paramProject, updateExecOptions);

                form.Close();
            }
            catch (Exception ex)
            {
                //logger.Error(ex);
                MessageBox.Show(ex.Message);
            }
            form.Cursor = Cursors.Default;
        }


    }
}
