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
    public partial class SearchEquipForm : Form
    {
        public SearchEquipForm()
        {
            InitializeComponent();
        }

        private void SearchEquipForm_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
 
            FindEquipOptions options = new FindEquipOptions();


            if (!int.TryParse(textBoxId.Text, out int id))
            {
                id = 0;
            }

            options.EquipType = 0;
            options.Id = id;
            options.Name = textBoxName.Text.Trim();

            List<FindEquipLocation> result = new();
            ParamProject? project = GlobalConfig.GetCurrentProject();
            if (project == null)
                return;
            FindEquipUtils.Find(project,options, result);

            if (result.Count < 1)
            {
                MessageBox.Show("Not found");
                return;
            }

            result.Sort(delegate (FindEquipLocation item1, FindEquipLocation item2)
            {
                return item1.ParamName.CompareTo(item2.ParamName);
            });

            List<RowWrapper> rowMappers = new();
            for (int i = 0; i < result.Count; i++)
            {

                var item = result[i];

                FSParam.Param param = project.FindParam(item.ParamName);
                if (param == null)
                    continue;
                var row = ParamRowUtils.FindRow(param, item.RowId);
                if (row == null)
                    continue;

                RowWrapper rowWrapper = new(row, param);

                rowMappers.Add(rowWrapper);

            }

            ParamRowForm paramRowForm = new ParamRowForm();
            RowListManager.Add(1, rowMappers);

            paramRowForm.ShowDialog();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
