using ERParamUtils;
using SoulsFormats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERParamEditor
{
    public partial class CreateProjectForm : Form
    {
        public CreateProjectForm()
        {
            InitializeComponent();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            fileDialogSelect.DefaultExt = ".bin";
            var ret = fileDialogSelect.ShowDialog();
            if (ret == DialogResult.OK)
            {     
                textFilename.Text = fileDialogSelect.FileName;
           
            }

        }

        public string RegulationPath = "";
        public string ProjectName = "";

        private void buttonOk_Click(object sender, EventArgs e)
        {

            RegulationPath = textFilename.Text.Trim();
            ProjectName = textProjectName.Text.Trim();
            try
            {
                string? errorMsg = ParamProjectManager.CheckProjectName(ProjectName);
                if (errorMsg != null) {
                    MessageBox.Show(errorMsg);
                    return;
                }  

                if (Path.GetFileName(RegulationPath) != GlobalConfig.RegulationFileName)
                {
                    MessageBox.Show("Invalid Regulation File");
                    return;
                }

                SFUtil.DecryptERRegulation(RegulationPath);


                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Regulation File");                
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CreateProjectForm_Load(object sender, EventArgs e)
        {
            textProjectName.Text = "proj";  
            
        }
    }
}
