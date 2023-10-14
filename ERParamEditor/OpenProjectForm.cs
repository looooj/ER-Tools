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

namespace ERParamEditor
{
    public partial class OpenProjectForm : Form
    {
        public OpenProjectForm()
        {
            InitializeComponent();
        }

        private void OpenProjectForm_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;

            List<string> projectList = ParamProjectManager.GetProjectList();
            foreach (string name in projectList)
            {
                listBoxProject.Items.Add(name);

            }
            buttonOk.Enabled = false;
            if (listBoxProject.Items.Count > 0)
            {
                buttonOk.Enabled = true;
                listBoxProject.SelectedIndex = 0;
            }
        }

        public string ProjectName = "";
        private void buttonOk_Click(object sender, EventArgs e)
        {
            ProjectName = listBoxProject.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
