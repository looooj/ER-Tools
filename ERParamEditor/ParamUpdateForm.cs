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

        private void ParamUpdateForm_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            checkBoxRestore.Checked = true;
            checkBoxSelectAll.Checked = true;

            updateParamTasks.Add(new DefaultShopParamTask());
            updateParamTasks.Add(new DefaultMapLotParamTask());
            updateParamTasks.Add(new RemoveRequireTask());
            updateParamTasks.Add(new BuddyTask());            
            updateParamTasks.Add(new LotParamTask());
            updateParamTasks.Add(new CharInitParamTask());
            updateParamTasks.Add(new SpEffectParamTask());
            updateParamTasks.Add(new UpdateRowParamTask());
            updateParamTasks.Add(new UpdateItemParamTask());

            checkBoxListTask.Items.Clear();

            for (int i = 0; i < updateParamTasks.Count; i++)
            {

                checkBoxListTask.Items.Add(updateParamTasks[i].Description);
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

            for (int i = 0; i < updateParamTasks.Count; i++)
            {

                if (checkBoxListTask.GetItemChecked(i))
                {
                    options.Add(updateParamTasks[i]);
                }
            }


            options.Restore = checkBoxRestore.Checked;
            options.Publish = publish;

            UpdateParamExector.Exec(_paramProject, options);
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
