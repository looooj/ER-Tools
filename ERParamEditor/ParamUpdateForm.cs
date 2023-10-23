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

        public ParamProject _paramProject;


        const string optionsFile = "update-opt.txt";

        List<string>? loadOptions()
        {

            List<string> lines = new();
            if (!File.Exists(optionsFile))
            {
                return null;
            }

            lines.AddRange(File.ReadLines(optionsFile));
            return lines;
        }

        void saveOptions(List<string> lines)
        {

            File.WriteAllLines("update-opt.txt", lines);

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
            updateParamTasks.Add(new SmithingStoneTask());


            updateParamTasks.Add(new CharInitParamTask());
            updateParamTasks.Add(new SpEffectParamTask());
            updateParamTasks.Add(new UpdateRowParamTask());
            updateParamTasks.Add(new UpdateItemParamTask());

            checkBoxListTask.Items.Clear();


            var saveOptions = loadOptions();

            for (int i = 0; i < updateParamTasks.Count; i++)
            {

                checkBoxListTask.Items.Add(updateParamTasks[i].Description);
                if (saveOptions == null || saveOptions.Contains(updateParamTasks[i].GetType().Name))
                    checkBoxListTask.SetItemChecked(i, true);
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

            var options = new UpdateParamOptions();
            List<string> lines = new();
            for (int i = 0; i < updateParamTasks.Count; i++)
            {

                if (checkBoxListTask.GetItemChecked(i))
                {
                    options.Add(updateParamTasks[i]);
                    lines.Add(updateParamTasks[i].GetType().Name);
                }
            }

            saveOptions(lines);

            options.Restore = checkBoxRestore.Checked;
            options.Publish = publish;

            try
            {
                UpdateParamExector.Exec(_paramProject, options);


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
