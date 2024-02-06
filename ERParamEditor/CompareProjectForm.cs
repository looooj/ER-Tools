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
using static SoulsFormats.BTPB;

namespace ERParamEditor
{
    public partial class CompareProjectForm : Form
    {
        public CompareProjectForm()
        {
            InitializeComponent();
        }

        private void CompareProjectForm_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            var projs = ParamProjectManager.GetProjectList();
            comboBoxProject1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxProject2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxProject1.Items.AddRange(projs.ToArray());
            comboBoxProject2.Items.AddRange(projs.ToArray());
            checkBoxSelectAll.Checked = true;
            List<string> paramNames = ParamProjectManager.GetParamNames(null);
            for (int i = 0; i < paramNames.Count; i++)
            {

                checkedListBoxParam.Items.Add(paramNames[i]);
                checkedListBoxParam.SetItemChecked(i, true);
            }

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            //

            string? proj1 = ComboBoxUtils.GetCurrentText(comboBoxProject1);
            string? proj2 = ComboBoxUtils.GetCurrentText(comboBoxProject2);
            if (proj1 == null || proj2 == null)
            {
                return;
            }

            if (proj1 == proj2)
            {
                return;
            }

            List<string> paramNames = new();
            for (int i = 0; i < checkedListBoxParam.CheckedItems.Count; i++)
            {

                var item = checkedListBoxParam.CheckedItems[i];
                paramNames.Add(checkedListBoxParam.GetItemText(item));
            }


            ParamProjectCompare.CompareProject(proj1, proj2, paramNames);
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {

            for (int i = 0; i < checkedListBoxParam.Items.Count; i++)
            {
                checkedListBoxParam.SetItemChecked(i, checkBoxSelectAll.Checked);
            }

        }
    }
}
