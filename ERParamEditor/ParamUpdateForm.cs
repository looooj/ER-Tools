using ERParamUtils;
using ERParamUtils.UpateParam;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERParamEditor
{
    public partial class ParamUpdateForm : Form
    {
        public ParamUpdateForm()
        {
            InitializeComponent();
        }

        List<UpdateParamTask> updateParamTasks = new();
        List<string> updateCommandOptions = new();

        public ParamProject _paramProject;


        const string optionsFile = "update-opt.txt";

        List<string>? loadOptions()
        {
            string fn = _paramProject.GetDir() + @"\" + optionsFile;

            List<string> lines = new();
            if (!File.Exists(fn))
            {
                return null;
            }

            lines.AddRange(File.ReadLines(fn));
            return lines;
        }

        void saveOptions(List<string> lines)
        {

            string fn = _paramProject.GetDir() + @"\" + optionsFile;
            File.WriteAllLines(fn, lines);

        }

        private void ParamUpdateForm_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            checkBoxRestore.Checked = true;
            checkBoxSelectAll.Checked = true;

            updateParamTasks.Add(new DefaultShopParamTask());
            updateParamTasks.Add(new SpecShopParamTask());


            updateParamTasks.Add(new DefaultMapLotParamTask());
            updateParamTasks.Add(new LotParamTask());

            updateParamTasks.Add(new RemoveRequireTask());
            updateParamTasks.Add(new RemoveWeightTask());
            updateParamTasks.Add(new BuddyTask());
            //updateParamTasks.Add(new SmithingStoneTask());


            updateParamTasks.Add(new CharInitParamTask());
            updateParamTasks.Add(new SpEffectParamTask());
            updateParamTasks.Add(new UpdateRowParamTask());
            updateParamTasks.Add(new UpdateItemParamTask());



            updateCommandOptions.Add(UpdateCommandOption.ReplaceGoldenSeedSacredTear);
            updateCommandOptions.Add(UpdateCommandOption.ReplaceTalismanPouch);
            updateCommandOptions.Add(UpdateCommandOption.ReplaceFinger);

            checkBoxListTask.Items.Clear();
            checkeBoxUpdateCommandOption.Items.Clear();


            var saveOptions = loadOptions();

            for (int i = 0; i < updateParamTasks.Count; i++)
            {

                checkBoxListTask.Items.Add(updateParamTasks[i].Description);
                if (saveOptions == null || saveOptions.Contains(updateParamTasks[i].GetType().Name))
                    checkBoxListTask.SetItemChecked(i, true);
            }

            for (int i = 0; i < updateCommandOptions.Count; i++)
            {
                var optionName = updateCommandOptions[i];
                checkeBoxUpdateCommandOption.Items.Add(optionName);
                if (saveOptions == null || saveOptions.Contains(optionName)) {
                    checkeBoxUpdateCommandOption.SetItemChecked(i, true);
                }

            }

        }


        private void exec(string? msg, bool publish)
        {


            if (msg != null)
            {
                DialogResult r = MessageBox.Show(msg, "", MessageBoxButtons.YesNo);
                if (r != DialogResult.Yes)
                {
                    return;
                }
            }

            var updateParamOptions = new UpdateParamOptions();
            List<string> optionLines = new();
            for (int i = 0; i < updateParamTasks.Count; i++)
            {

                if (checkBoxListTask.GetItemChecked(i))
                {
                    updateParamOptions.AddTask(updateParamTasks[i]);
                    optionLines.Add(updateParamTasks[i].GetType().Name);
                }
            }

            for (int i = 0; i < updateCommandOptions.Count; i++)
            {

                if (checkeBoxUpdateCommandOption.GetItemChecked(i))
                {
                    string name = updateCommandOptions[i];
                    updateParamOptions.AddUpdateCommandOption(name);
                    optionLines.Add(name);
                }
            }

            saveOptions(optionLines);

            updateParamOptions.Restore = checkBoxRestore.Checked;
            updateParamOptions.Publish = publish;
            try
            {
                UpdateParamExector.Exec(_paramProject, updateParamOptions);


                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            exec(null, false);
        }

        private void buttonUpdatePublish_Click(object sender, EventArgs e)
        {
            exec("Are you sure", true);
        }


        public void Exec(ParamProject paramProject)
        {

            _paramProject = paramProject;
            ShowDialog();
        }

        private void checkBoxSelectAll_CheckedChanged(object? sender, EventArgs e)
        {
            for (int i = 0; i < checkBoxListTask.Items.Count; i++)
            {
                checkBoxListTask.SetItemChecked(i, checkBoxSelectAll.Checked);
            }
        }
    }
}
