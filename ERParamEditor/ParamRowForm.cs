using ERParamUtils;
using NLog.Time;
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


        //public FSParam.Param? CurrentParam = null;

        private void ParamRowForm_Load(object sender, EventArgs e)
        {


        }

        ContextMenuStrip? menuCell;
        ContextMenuStrip? menuRow;

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

            InitMenuRow();
            InitMenuCell();
            InitRows();

        }

        void InitMenuRow()
        {
            if (menuRow == null)
            {
                menuRow = new();
            }
            menuRow.Items.Add(new ToolStripMenuItem("GoBack", null, goBack_Handler));
            menuRow.Items.Add(new ToolStripSeparator());


            menuRow.Items.Add(new ToolStripMenuItem("FindRowByName", null, findRowName_Handler));
            menuRow.Items.Add(new ToolStripSeparator());

            menuRow.Items.Add(new ToolStripMenuItem("GotoRowId", null, gotoRowId_Handler));
            menuRow.Items.Add(new ToolStripSeparator());


            menuRow.Items.Add(new ToolStripMenuItem("CopyRow", null, copyRow_Handler));
            menuRow.Items.Add(new ToolStripSeparator());
            menuRow.Items.Add(new ToolStripMenuItem("CopyRowName", null, copyRowName_Handler));
            menuRow.Items.Add(new ToolStripSeparator());
            menuRow.Items.Add(new ToolStripMenuItem("CopyRowId", null, copyRowId_Handler));
            menuRow.Items.Add(new ToolStripSeparator());
            menuRow.Items.Add(new ToolStripMenuItem("Export", null, exportRowButton_Click));


        }

        void changeViewMode()
        {



        }


        private void goBack_Handler(object? sender, EventArgs e)
        {

            if (!RowListManager.GoBack()) {
                return;
            }

            var item = RowListManager.GetCurrent();

            changeGridViewRow(item.Mode, item.Rows, item.CurrentRow);
            //bindingSourceRow.DataSource = item.Rows;
            //dataGridViewRow.DataSource = bindingSourceRow.DataSource;
        }


        void exportRow(RowWrapper row, string fn)
        {

            var cells = ParamCellList.Build(row.GetParam(), row.GetRow());
            var lines = new List<string>();
            foreach (var cell in cells)
            {

                lines.Add(string.Format("{0},{1},{2},{3},{4}",
                    cell.ColIndex, cell.DisplayName, cell.Key, cell.Value, cell.Comment));


            }
            File.WriteAllLines(fn, lines);
        }

        private void exportRowButton_Click(object? sender, EventArgs e)
        {
            if (dataGridViewRow.SelectedRows.Count < 1)
                return;
            string dir = GlobalConfig.GetCurrentProject().GetDir() + @"\exp";
            Directory.CreateDirectory(dir);

            for (int i = 0; i < dataGridViewRow.SelectedRows.Count; i++)
            {
                object obj = dataGridViewRow.SelectedRows[i].DataBoundItem;
                RowWrapper row = (RowWrapper)obj;
                string fn = dir + @"\" + row.GetParam().Name + "-" + row.ID + "-" + row.Name + ".txt";

                exportRow(row, fn);

            }

        }


        private void copyRow(bool rowId, bool rowName)
        {

            if (dataGridViewRow.SelectedRows.Count < 1)
                return;
            string text = "";
            for (int i = 0; i < dataGridViewRow.SelectedRows.Count; i++)
            {
                object obj = dataGridViewRow.SelectedRows[i].DataBoundItem;
                RowWrapper row = (RowWrapper)obj;
                if (rowId)
                    text = text + row.ID;
                if (rowName)
                    text = text + row.Name;
                text = text + " ";

            }
            Clipboard.SetData(DataFormats.Text, (Object)text);
            ClipTextFile.Add(text);
        }

        private void copyRowId_Handler(object? sender, EventArgs e)
        {
            copyRow(true, false);
        }

        private void copyRowName_Handler(object? sender, EventArgs e)
        {
            copyRow(false, true);

        }

        private void copyRow_Handler(object? sender, EventArgs e)
        {

            if (dataGridViewRow.SelectedRows.Count < 1)
                return;
            copyRow(true, true);
        }

        private void scrollToRow(int index)
        {

            dataGridViewRow.FirstDisplayedScrollingRowIndex = 0;
            dataGridViewRow.FirstDisplayedScrollingRowIndex = index;

            dataGridViewRow.ClearSelection();
            dataGridViewRow.Rows[index].Selected = true;

            //dataGridViewRow.DisplayedRowCount 
            /*
            while (index < dataGridViewRow.Rows.Count) {
                if (dataGridViewRow.Rows[index].Visible)
                    return;

                if (dataGridViewRow.FirstDisplayedScrollingRowIndex < index) {
                    dataGridViewRow.FirstDisplayedScrollingRowIndex++;
                }
            }*/
        }

        void gotoRowId(int rowId)
        {

            for (int i = 0; i < dataGridViewRow.Rows.Count; i++)
            {
                var item = (RowWrapper)dataGridViewRow.Rows[i].DataBoundItem;
                if (item.ID == rowId)
                {
                    scrollToRow(i);
                    return;
                }
            }
        }

        private void gotoRowId_Handler(object? sender, EventArgs e)
        {
            string text = "";
            if (!InputDialog.InputBox("RowId", "RowId", ref text))
            {
                return;
            }

            text = text.Trim();
            int rowId = int.Parse(text);
            gotoRowId(rowId);
        }



        private void findRowName_Handler(object? sender, EventArgs e)
        {
            string text = "";
            if (!InputDialog.InputBox("Find", "FindText", ref text))
            {
                return;
            }
            text = text.Trim();
            var item = RowListManager.GetCurrent();

            List<RowWrapper> rows = new();
            foreach (RowWrapper row in item.Rows)
            {
                if (row.Name.Length < 1)
                    continue;

                if (row.Name.Contains(text))
                {
                    rows.Add(row);
                    continue;
                }

            }
            bindingSourceRow.DataSource = rows;
            dataGridViewRow.DataSource = bindingSourceRow.DataSource;
        }

        //public List<RowWrapper>? CurrentRowWrappers;
        //public List<RowWrapper>? JumpRowWrappers;
        //public int ViewMode = 0;
        public string ParamName = "";
        void InitRows()
        {
            var rowListItem = RowListManager.GetCurrent();
            if (rowListItem == null)
                return;


            bindingSourceRow.DataSource = rowListItem.Rows;

            dataGridViewRow.AutoGenerateColumns = true;
            dataGridViewRow.DataSource = bindingSourceRow.DataSource;
            dataGridViewRow.RowHeadersVisible = false;
            dataGridViewRow.AllowUserToResizeRows = false;
            dataGridViewRow.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewRow.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewRow.MultiSelect = true;
            dataGridViewRow.ContextMenuStrip = menuRow;
            dataGridViewRow.SelectionChanged += RowSelectionChanged;

            if (rowListItem.Mode == 0)
            {
                dataGridViewRow.Columns[0].Visible = false;
                Text = ParamName;
            }
        }



        void InitMenuCell()
        {
            if (menuCell == null)
                menuCell = new();
            menuCell.Items.Add(new ToolStripMenuItem("CopyCell", null, copyCell_Handler));
            menuCell.Items.Add(new ToolStripSeparator());
            menuCell.Items.Add(new ToolStripMenuItem("CopyValue", null, copyCellValue_Handler));
            menuCell.Items.Add(new ToolStripSeparator());
            menuCell.Items.Add(new ToolStripMenuItem("JumpRef", null, jumpRef_Handler));

            if (GlobalConfig.Debug)
            {
                menuCell.Items.Add(new ToolStripSeparator());
                menuCell.Items.Add(new ToolStripMenuItem("ChangValue", null, changeValue_Handler));
            }
        }

        private void changeValue_Handler(object? sender, EventArgs e)
        {
            if (dataGridViewCell.SelectedRows.Count < 1)
                return;
            string value = "";
            ParamCellItem cell = (ParamCellItem)dataGridViewCell.SelectedRows[0].DataBoundItem;

            var ret = InputDialog.InputBox("ChangeValue " + cell.DisplayName + "," + cell.Key, "NewValue", ref value);

            if (ret)
            {
                var row = RowListManager.GetCurrentRow();
                ParamRowUtils.SetCellValue(row.GetRow(), cell.Key, value);
            }
        }


        private void copyCell_Handler(object? sender, EventArgs e)
        {
            if (dataGridViewCell.SelectedRows.Count < 1)
                return;

            var row = RowListManager.GetCurrentRow();

            string text = "";
            for (int i = 0; i < dataGridViewCell.SelectedRows.Count; i++)
            {
                ParamCellItem cell = (ParamCellItem)dataGridViewCell.SelectedRows[i].DataBoundItem;

                string line = string.Format("{0};{1};{2};{3}",
                    row.ID,
                    row.Name,
                    cell.Key,
                    cell.Value
                    );

                //lines.Add(line);
                ClipTextFile.Add(line);
                text = text + line + " ";
            }
            Clipboard.SetData(DataFormats.Text, (Object)text);

        }

        private void copyCellValue_Handler(object? sender, EventArgs e)
        {
            if (dataGridViewCell.SelectedRows.Count < 1)
                return;

            string text = "";
            for (int i = 0; i < dataGridViewCell.SelectedRows.Count; i++)
            {
                ParamCellItem cell = (ParamCellItem)dataGridViewCell.SelectedRows[i].DataBoundItem;

                if (i > 0)
                    text = text + " ";

                string line = cell.Value == null ? "" : cell.Value;
                ClipTextFile.Add(line);
                text = text + line;
            }
            Clipboard.SetData(DataFormats.Text, (Object)text);

        }

        private void jumpRef_Handler(object? sender, EventArgs e)
        {
            if (dataGridViewCell.SelectedRows.Count < 1)
                return;


            ParamCellItem cell = (ParamCellItem)dataGridViewCell.SelectedRows[0].DataBoundItem;
            var row = RowListManager.GetCurrentRow();
            var refRows = ParamCellRefUtils.GetRowWrappers(row, cell);
            if (refRows.Count > 0)
            {
                changeRows(1, refRows);
            }
        }

        void changeRows(int mode, List<RowWrapper> rows)
        {

            if (rows.Count < 1)
                return;
            RowListManager.Add(mode, rows);
            changeGridViewRow(mode, rows, null);
        }

        void changeGridViewRow(int mode, List<RowWrapper> rows, RowWrapper currentRow)
        {


            bindingSourceRow.DataSource = rows;
            dataGridViewRow.DataSource = bindingSourceRow.DataSource;
            dataGridViewRow.Columns[0].Visible = true;
            switch (mode)
            {
                case RowListViewMode.DEFAULT:
                    dataGridViewRow.Columns[0].Visible = false;
                    break;
            }
            if (currentRow == null)
            {
                FillCells(rows[0]);
            }
            else
            {
                FillCells(currentRow);
                gotoRowId(currentRow.ID);
            }
        }


        void FillCells(RowWrapper row)
        {

            RowListManager.SetCurrentRow(row);

            var cells = ParamCellList.Build(row.GetParam(), row.GetRow());

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

            FillCells(selection);
        }

        private void ParamRowForm_Shown(object sender, EventArgs e)
        {
            InitControls();
        }

        private void ParamRowForm_ResizeEnd(object sender, EventArgs e)
        {
            dataGridViewRow.Width = (Width / 2) - 30;

        }

        private void ParamRowForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            RowListManager.Clear();
        }
    }
}
