using ERParamUtils;
using ERParamUtils.UpdateParam;
using MultiLangLib;
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
        List<UpdateParamOption> updateParamOptions = new();

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

            updateParamTasks.Add(new UnlockCraftingTask());
            updateParamTasks.Add(new SpecRecipeParamTask());

            updateParamTasks.Add(new UnlockGraceTask());
            updateParamTasks.Add(new DefaultShopParamTask());
            updateParamTasks.Add(new SpecShopParamTask());


            updateParamTasks.Add(new DefaultMapLotParamTask());
            updateParamTasks.Add(new LotParamTask());

            updateParamTasks.Add(new RemoveRequireTask());
            updateParamTasks.Add(new RemoveWeightTask());
            updateParamTasks.Add(new BuddyTask());


            updateParamTasks.Add(new CharInitParamTask());
            updateParamTasks.Add(new SpEffectParamTask());
            updateParamTasks.Add(new UpdateRowParamTask());
            updateParamTasks.Add(new UpdateItemParamTask());

            updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.ReplaceGoldenSeedSacredTear));
            updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.ReplaceTalismanPouch));
            updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.ReplaceRune));
            updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.ReplaceStoneswordKey));
            updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.ReplaceMemoryStone));
            updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.ReplaceFinger));
            updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.ReplaceDeathroot));
            updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.ReplaceCookbook));
            updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.AddMapPiece));
            //updateParamOptions.Add(new UpdateParamOption(UpdateParamOption.AddWhetblade));


            checkListBoxTask.Items.Clear();
            checkListBoxUpdateCommandOption.Items.Clear();


            MultiLang.ApplyMessage(updateParamTasks);
            MultiLang.ApplyMessage(updateParamOptions);
            MultiLang.ApplyForm(this, "ParamUpdateForm");

            var saveOptions = loadOptions();

            for (int i = 0; i < updateParamTasks.Count; i++)
            {

                checkListBoxTask.Items.Add(updateParamTasks[i].Description);
                if (saveOptions == null || saveOptions.Contains(updateParamTasks[i].GetType().Name))
                    checkListBoxTask.SetItemChecked(i, true);
            }

            for (int i = 0; i < updateParamOptions.Count; i++)
            {
                var option = updateParamOptions[i];
                checkListBoxUpdateCommandOption.Items.Add(option.Description);
                if (saveOptions == null || saveOptions.Contains(option.Name)) {
                    checkListBoxUpdateCommandOption.SetItemChecked(i, true);
                }

            }

        }


        private void ExecUpdatePublish(string? msg, bool publish)
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

                if (checkListBoxTask.GetItemChecked(i))
                {
                    updateParamOptions.AddTask(updateParamTasks[i]);
                    optionLines.Add(updateParamTasks[i].GetType().Name);
                }
            }

            for (int i = 0; i < this.updateParamOptions.Count; i++)
            {

                if (checkListBoxUpdateCommandOption.GetItemChecked(i))
                {
                    var option = this.updateParamOptions[i];
                    updateParamOptions.AddUpdateCommandOption(option.Name);
                    optionLines.Add(option.Name);
                }
            }

            saveOptions(optionLines);

            updateParamOptions.Restore = checkBoxRestore.Checked;
            updateParamOptions.Publish = publish;
            try
            {
                Cursor = Cursors.WaitCursor;
                UpdateParamExector.Exec(_paramProject, updateParamOptions);


                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            Cursor = Cursors.Default;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            ExecUpdatePublish(null, false);
        }

        private void buttonUpdatePublish_Click(object sender, EventArgs e)
        {
            string msg = MultiLang.GetText("Are you sure exec publish?");
            ExecUpdatePublish(msg, true);
        }


        public void Exec(ParamProject paramProject)
        {

            _paramProject = paramProject;
            ShowDialog();
        }

        private void checkBoxSelectAll_CheckedChanged(object? sender, EventArgs e)
        {
            for (int i = 0; i < checkListBoxTask.Items.Count; i++)
            {
                checkListBoxTask.SetItemChecked(i, checkBoxSelectAll.Checked);
            }

            for (int i = 0; i < checkListBoxUpdateCommandOption.Items.Count; i++)
            {
                checkListBoxUpdateCommandOption.SetItemChecked(i, checkBoxSelectAll.Checked);
            }

        }
    }
}
