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
        public bool initCopy()
        {
            return initCopyCheckBox.Checked;
        }
        List<ParamProject> paramProjects = new List<ParamProject>();

        private void OpenProjectForm_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;

            var currentProj = GlobalConfig.GetCurrentProject();
            paramProjects = ParamProjectManager.GetProjectList2();
            int index = 0;
            foreach (var proj in paramProjects)
            {
                var s = proj.GetName() + " (" + proj.GetModRegulationPath() + ")";
                listBoxProject.Items.Add(s);

                if (currentProj != null)
                    if (proj.GetName() == currentProj.GetName())
                    {
                        listBoxProject.SelectedIndex = index;
                    }

                index++;
            }
            buttonOk.Enabled = false;
            if (listBoxProject.Items.Count > 0)
            {
                buttonOk.Enabled = true;
                if ( listBoxProject.SelectedIndex < 0 )
                    listBoxProject.SelectedIndex = 0;
            }
        }

        public string ProjectName = "";
        private void buttonOk_Click(object sender, EventArgs e)
        {

            if (listBoxProject.SelectedItem == null)
                return;
            int itemIndex = listBoxProject.SelectedIndex;

            ProjectName = paramProjects[itemIndex].GetName();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {

            if (listBoxProject.SelectedItem == null)
                return;

            int itemIndex = listBoxProject.SelectedIndex;

            ProjectName = paramProjects[itemIndex].GetName();

            //ProjectName = listBoxProject.SelectedItem.ToString();
            var qmsg = "Are you sure delete " + ProjectName;

            var ret = MessageBox.Show(qmsg, "", MessageBoxButtons.YesNo);
            if (ret == DialogResult.No)
            {
                return;
            }

            string msg = ParamProjectManager.DeleteProject(ProjectName);
            if (msg != "")
            {

                MessageBox.Show(msg);
                return;
            }
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
