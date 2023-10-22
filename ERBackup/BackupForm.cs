using ERParamUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERBackup
{
    public partial class BackupForm : Form
    {
        public BackupForm()
        {
            InitializeComponent();
        }

        private void createBackupButton_Click(object sender, EventArgs e)
        {
            string name = "t";
            if (!InputDialog.InputBox("Backup", "Name", ref name))
            {
                return;
            }
            backupTool.ExecBak(name);
            initZipList();
        }

        private BackupTool backupTool = new();

        void initZipList()
        {


            List<string> zipList = backupTool.GetZipList(showAutoBak.Checked);
            zipListBox.Items.Clear();
            zipListBox.Items.AddRange(zipList.ToArray());

        }

        private void BackupForm_Load(object sender, EventArgs e)
        {
            //StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            GlobalConfig.Load();
            backupTool.Init();
            autoBakCheckBox.Checked = true;
            initZipList();

            listBoxConfig.Items.Add("SaveDir: " + backupTool.GetSaveDir());
            listBoxConfig.Items.Add("BakDir: " + backupTool.GetBakDir());

        }

        private void restoreBackupButton_Click(object sender, EventArgs e)
        {
            int index = zipListBox.SelectedIndex;
            if (index >= 0)
            {
                string msg = string.Format("Use {0} overwrite current ?", zipListBox.Items[index]);
                if (MessageBox.Show(msg, "", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
                if (autoBakCheckBox.Checked)
                    backupTool.ExecAutoBak();
                string zipName = zipListBox.Items[index].ToString();
                backupTool.ExecRestore(zipName);

                autoBakCheckBox.Checked = true;
            }
        }

        private void showAutoBak_CheckedChanged(object sender, EventArgs e)
        {
            initZipList();
        }
    }
}
