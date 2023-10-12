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
    public partial class ParamRowForm : Form
    {
        public ParamRowForm()
        {
            InitializeComponent();
        }


        public FSParam.Param? CurrentParam = null;

        private void ParamRowForm_Load(object sender, EventArgs e)
        {


        }

        ContextMenuStrip menuCell;
        ContextMenuStrip menuRow;

        void InitControls()
        {
            menuRow = new();
            menuCell = new();

            WindowState = FormWindowState.Maximized;

            panelClient.Dock = DockStyle.Fill;
            panelClient.BringToFront();
            dataGridViewCell.Dock = DockStyle.Fill;
            dataGridViewCell.BringToFront();
            panelCell.BringToFront();

            if (CurrentParam == null)
                return;

            Text = CurrentParam.ParamType;

            InitMenuRow();

            InitRows();


        }

        void InitMenuRow() {

            menuRow.Items.Add(new ToolStripMenuItem("FindRowName", null, findRowName_Handler));
            menuRow.Items.Add(new ToolStripSeparator());
            menuRow.Items.Add(new ToolStripMenuItem("CopyRow", null, copyRow_Handler));
            menuRow.Items.Add(new ToolStripSeparator());
            menuRow.Items.Add(new ToolStripMenuItem("CopyRowName", null, copyRowName_Handler));
            menuRow.Items.Add(new ToolStripSeparator());
            menuRow.Items.Add(new ToolStripMenuItem("CopyRowId", null, copyRowId_Handler));
            menuRow.Items.Add(new ToolStripSeparator());
            menuRow.Items.Add(new ToolStripMenuItem("Export", null, exportRowButton_Click));

        }

        private void exportRowButton_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void copyRowId_Handler(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void copyRowName_Handler(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void copyRow_Handler(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void findRowName_Handler(object? sender, EventArgs e)
        {
            string text = "";
            if (InputDialog.InputBox("Find", "FindText", ref text) != DialogResult.OK)
            {
                return;
            }
            text = text.Trim();
            List<RowWrapper> rows = new();
            foreach (RowWrapper row in CurrentRowWrappers)
            {
                if (row.Name.Length < 1)
                    continue;
                if (!row.Name.Contains(text))
                    continue;
                rows.Add(row);
            }
            bindingSourceRow.DataSource = rows;
            dataGridViewRow.DataSource = bindingSourceRow.DataSource;
        }

        List<RowWrapper> CurrentRowWrappers;
        void InitRows()
        {
            if (CurrentParam == null)
                return;


            RowFilter[] rowFilers = { new RowBlankNameFiler()  };
            CurrentRowWrappers = ParamRowUtils.ConvertToRowWrapper(CurrentParam, rowFilers);
            bindingSourceRow.DataSource = CurrentRowWrappers;

            dataGridViewRow.AutoGenerateColumns = true;
            dataGridViewRow.DataSource = bindingSourceRow.DataSource;
            dataGridViewRow.RowHeadersVisible = false;
            dataGridViewRow.AllowUserToResizeRows = false;
            dataGridViewRow.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewRow.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewRow.MultiSelect = true;
            dataGridViewRow.ContextMenuStrip = menuRow;
            dataGridViewRow.SelectionChanged += RowSelectionChanged;

            if (CurrentRowWrappers.Count > 0)
            {
                //FillCells(CurrentRowWrappers[0].)
            }
        }

        void FillCells(FSParam.Param.Row row)
        {
            if (CurrentParam == null)
                return;

            var cells = ParamCellList.Build(CurrentParam, row);

            bindingSourceCell.DataSource = cells;

            dataGridViewCell.AutoGenerateColumns = true;
            dataGridViewCell.DataSource = bindingSourceCell.DataSource;
            dataGridViewCell.RowHeadersVisible = false;
            dataGridViewCell.AllowUserToResizeRows = false;
            dataGridViewCell.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCell.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCell.MultiSelect = false;
            dataGridViewCell.ContextMenuStrip = menuCell;


        }

        void RowSelectionChanged(object? sender, EventArgs e)
        {

            if (dataGridViewRow.SelectedRows.Count < 1)
                return;

            RowWrapper selection = (RowWrapper)dataGridViewRow.SelectedRows[0].DataBoundItem;

            FillCells(selection.GetRow());
        }

        private void ParamRowForm_Shown(object sender, EventArgs e)
        {
            InitControls();
        }

        private void ParamRowForm_ResizeEnd(object sender, EventArgs e)
        {
            dataGridViewRow.Width = (Width / 2) - 30;

        }
    }
}
