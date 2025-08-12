using ERParamUtils;
using ERParamUtils.UpateParam;
using ERParamUtils.UpdateParam;
using MultiLangLib;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ERParamEditor
{
    public partial class ParamUpdateForm2 : Form
    {
        public ParamUpdateForm2()
        {
            InitializeComponent();
        }

        private void ParamUpdateForm2_Load(object sender, EventArgs e)
        {
            InitControls();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        List<UpdateParamTask> updateParamTasks = new();
        List<UpdateParamOptionItem> updateParamOptions = new();
        public ParamProject _paramProject;

        const string optionsFile = "update-opt-2.txt";

        DictConfig loadOptions()
        {
            DictConfig dictConfig = new DictConfig();

            string fn = _paramProject.GetDir() + @"\" + optionsFile;

            if (!File.Exists(fn))
            {
                return dictConfig;
            }
            dictConfig.Load(fn);
            return dictConfig;
        }

        void saveOptions(DictConfig dictConfig)
        {

            string fn = _paramProject.GetDir() + @"\" + optionsFile;
            dictConfig.Save(fn);
            //File.WriteAllLines(fn, lines);

        }

        private void InitControls()
        {

            checkedListBoxOptions.Dock = DockStyle.Top;
            tabControl1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Dock = DockStyle.Fill;

            UpdateParamExector.CreateTaskList(updateParamTasks);
            UpdateParamExector.CreateOptionList(updateParamOptions);


            MultiLang.ApplyMessage(updateParamTasks);
            MultiLang.ApplyMessage(updateParamOptions);

            var lastSaveOptions = loadOptions();

            int index = 0;
            for (int i = 0; i < updateParamTasks.Count; i++)
            {

                checkedListBoxOptions.Items.Add(updateParamTasks[i].Description);
                if (lastSaveOptions.Contains(updateParamTasks[i].GetType().Name))
                    checkedListBoxOptions.SetItemChecked(index, true);
                index++;
            }

            for (int i = 0; i < updateParamOptions.Count; i++)
            {
                var option = updateParamOptions[i];
                checkedListBoxOptions.Items.Add(option.Description);
                if (lastSaveOptions.Contains(option.Name))
                {
                    checkedListBoxOptions.SetItemChecked(index, true);
                }
                index++;
            }
            InitOthers(lastSaveOptions);
            initTalisman(lastSaveOptions);
            MultiLang.ApplyForm(this, "ParamUpdateForm");

        }

        //List<ComboBox> optionSelectionList = new List<ComboBox>();
        ComboBox comboBoxReplaceGoldenRune;
        ComboBox comboBoxGetSoulRate;
        ComboBox comboBoxUnlockGrace;
        void InitOthers(DictConfig lastSaveOptions)
        {

            string runeNames = MultiLang.GetText("UpdateParam",UpdateParamOptionNames.ReplaceGoldenRune,
                ReplaceGoldenRune.GetNameList());

            comboBoxReplaceGoldenRune = ControlUtils.AddSelectionNameValue(tableLayoutPanel1,
                UpdateParamOptionNames.ReplaceGoldenRune,
                UpdateParamOptionNames.ReplaceGoldenRune,
                runeNames, ReplaceGoldenRune.GetValueList(),
                lastSaveOptions.GetString(UpdateParamOptionNames.ReplaceGoldenRune, "0")
                );

            //optionSelectionList.Add(cb);
            string rateList = "1,2,3,4,5,10,20,50";
            comboBoxGetSoulRate = ControlUtils.AddSelectionNameValue(tableLayoutPanel1,
                UpdateParamOptionNames.GetRuneRate, UpdateParamOptionNames.GetRuneRate, rateList, rateList,
                                lastSaveOptions.GetString(UpdateParamOptionNames.GetRuneRate, "1"));


            string unlockGraceNames = MultiLang.GetText("UpdateParam", UpdateParamOptionNames.UnlockGrace,
                 UnlockGraceConfig.GetNameList());

            comboBoxUnlockGrace = ControlUtils.AddSelectionNameValue(tableLayoutPanel1,
                UpdateParamOptionNames.UnlockGrace,
                UpdateParamOptionNames.UnlockGrace,
                unlockGraceNames, UnlockGraceConfig.GetValueList(),
                lastSaveOptions.GetString(UpdateParamOptionNames.UnlockGrace, "0")
                );
            //optionSelectionList.Add(cb);
        }
        List<UpdateParamOptionItem> updateTalismanOptions = new();

        private void initTalisman(DictConfig lastSaveOptions) {

            //checkedListBoxCrimsonAmberMedallion.Dock = DockStyle.Fill;
            updateTalismanOptions = UpdateTalisman.GetUpdateParams();
            MultiLang.ApplyMessage(updateTalismanOptions);

            for (int i = 0; i < updateTalismanOptions.Count; i++) { 
                var item = updateTalismanOptions[i];
                
                checkedListBoxTalisman.Items.Add(item.Description);
                var c = (lastSaveOptions.Contains(item.Name));
                checkedListBoxTalisman.SetItemChecked(i, c);                
            }
            //checkedListBoxCrimsonAmberMedallion.Items.Add(  );
        }

        private void ExecUpdatePublish(string? msg, bool publish)
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

            var updateExecOptions = new UpdateParamExecOptions();
            var dictConfig = new DictConfig();
            int index = 0;
            for (int i = 0; i < updateParamTasks.Count; i++)
            {

                if (checkedListBoxOptions.GetItemChecked(index))
                {
                    updateExecOptions.AddTask(updateParamTasks[i]);
                    dictConfig.SetString(updateParamTasks[i].GetType().Name, "1");
                }
                index++;
            }

            for (int i = 0; i < updateParamOptions.Count; i++)
            {

                if (checkedListBoxOptions.GetItemChecked(index))
                {
                    var option = updateParamOptions[i];
                    //updateExecOptions.AddUpdateCommandOption(option.Name);
                    dictConfig.SetString(option.Name, "1");
                }
                index++;
            }

            for (int i = 0; i < updateTalismanOptions.Count; i++) {
                if (checkedListBoxTalisman.GetItemChecked(i)) {
                    dictConfig.SetString(updateTalismanOptions[i].Name, "1"); 
                }
            }

            dictConfig.SetString(UpdateParamOptionNames.ReplaceGoldenRune,
                comboBoxReplaceGoldenRune.SelectedValue.ToString());

            dictConfig.SetString(UpdateParamOptionNames.GetRuneRate,
                comboBoxGetSoulRate.SelectedValue.ToString());


            dictConfig.SetString(UpdateParamOptionNames.UnlockGrace,
                comboBoxUnlockGrace.SelectedValue.ToString());


            updateExecOptions.AddUpdateCommandOption(dictConfig);

            saveOptions(dictConfig);

            updateExecOptions.Restore = true;// checkBoxRestore.Checked;
            updateExecOptions.Publish = publish;
            try
            {
                Cursor = Cursors.WaitCursor;
                UpdateParamExector.Exec(_paramProject, updateExecOptions);

                Close();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                MessageBox.Show(ex.Message);
            }
            Cursor = Cursors.Default;
        }


        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            ExecUpdatePublish(null, false);
        }

        private void buttonPublish_Click(object sender, EventArgs e)
        {
            string msg = MultiLang.GetDefaultText("publish", "Are you sure exec publish?");

            ExecUpdatePublish(msg, true);
        }

        private void checkBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxOptions.Items.Count; i++)
            {
                checkedListBoxOptions.SetItemChecked(i, checkBoxSelectAll.Checked);
            }
        }

        public void Exec(ParamProject paramProject)
        {

            _paramProject = paramProject;
            ShowDialog();
        }
    }
}
