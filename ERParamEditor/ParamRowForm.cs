using ERParamUtils;
using NLog.Time;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

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
            menuRow.Items.Add(new ToolStripMenuItem("GotoRowName", null, gotoRowName_Handler));
            menuRow.Items.Add(new ToolStripMenuItem("NextRow", null, nextRowName_Handler, Keys.F3));
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

            menuRow.Items.Add(new ToolStripSeparator());
            menuRow.Items.Add(new ToolStripMenuItem("Compare", null, compareRow_Handler));

        }

        void changeViewMode()
        {



        }


        private void goBack_Handler(object? sender, EventArgs e)
        {

            if (!RowListManager.GoBack())
            {
                return;
            }

            var item = RowListManager.GetCurrent();
            if (item != null)
                changeGridViewRow(item.Mode, item.Rows, item.CurrentRow);
            //bindingSourceRow.DataSource = item.Rows;
            //dataGridViewRow.DataSource = bindingSourceRow.DataSource;
        }


        void exportRow(RowWrapper row, string fn)
        {
            var fieldMeta = row.GetParam().FieldMeta;
            var cells = ParamCellList.Build(row.GetParam(), row.GetRow(),fieldMeta);
            var lines = new List<string>();
            lines.Add("#" + row.GetParam().Name);
            lines.Add("#" + row.ID);
            lines.Add("#" + row.Name);
            foreach (var cell in cells)
            {

                lines.Add(string.Format("{0},{1}={2}  {3}({4}) {5} {6}",
                    cell.ColIndex, 
                    cell.Key, cell.Value, 
                    cell.DisplayName, cell.GetCell().Def.DisplayName,
                    cell.GetValueType(), cell.Comment
                    ));


            }

            File.WriteAllLines(fn, lines);
        }

        private void exportRowButton_Click(object? sender, EventArgs e)
        {
            if (dataGridViewRow.SelectedRows.Count < 1)
                return;
            var project = GlobalConfig.GetCurrentProject();
            if (project == null)
                return;

            string dir = project.GetDir() + @"\exp";
            Directory.CreateDirectory(dir);

            for (int i = 0; i < dataGridViewRow.SelectedRows.Count; i++)
            {
                object obj = dataGridViewRow.SelectedRows[i].DataBoundItem;
                RowWrapper row = (RowWrapper)obj;
                string fn = row.GetParam().Name + "-" + row.ID + "-" + row.Name + ".txt";
                fn = PathNameUtils.ConvertName(fn);
                fn = dir + @"\" + fn;
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
                if ( i > 0 )
                    text = text + ",";

                if (rowId)
                    text = text + row.ID;
                if (rowName)
                    text = text + row.Name;

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
            //int rowId = 
            if (!int.TryParse(text, out int rowId))
                return;
            gotoRowId(rowId);
        }

        string gotoRowNameText = "";
        int gotoRowNameIndex = 0;

        void gotoRowName(string rowName)
        {

            
            for (int i = gotoRowNameIndex; i < dataGridViewRow.Rows.Count; i++)
            {
                var item = (RowWrapper)dataGridViewRow.Rows[i].DataBoundItem;
                if (item.Name != null && item.Name.Contains(rowName))
                {
                    scrollToRow(i);
                    gotoRowNameIndex = i+1;
                    return;
                }
            }
            gotoRowNameIndex = 0;
        }

        private void gotoRowName_Handler(object? sender, EventArgs e) {
            string text = "";
            if (!InputDialog.InputBox("RowName", "RowName", ref text))
            {
                return;
            }

            text = text.Trim();
            gotoRowNameText = text;
            gotoRowNameIndex = 0;
            gotoRowName(gotoRowNameText);
        }

        private void nextRowName_Handler(object? sender, EventArgs e)
        {
            gotoRowName(gotoRowNameText);

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
            if (item == null)
                return;

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

            changeRows(RowListViewMode.FIND, rows);

        }


        private void compareRow_Handler(object? sender, EventArgs e)
        {

            var project = GlobalConfig.GetCurrentProject();
            if (project == null)
                return;

            string dir = project.GetDir() + @"\exp";
            Directory.CreateDirectory(dir);

            if (dataGridViewRow.SelectedRows.Count !=2 )
                return;


            RowWrapper row1 = (RowWrapper)dataGridViewRow.SelectedRows[0].DataBoundItem;
            RowWrapper row2 = (RowWrapper)dataGridViewRow.SelectedRows[1].DataBoundItem;
            List<string> result = new();
            result.Add("#" + row1.ParamName + ","+ row1.ParamName);
            result.Add(string.Format("{0} {1}  {2} {3}", 
                row1.ID,row1.Name,row2.ID,row2.Name));
            for (int i = 0; i < row1.GetRow().Cells.Count; i++) {
                var cell1 = row1.GetRow().Cells[i];
                var cell2 = row2.GetRow().Cells[i];
                if (cell1.Value.Equals(cell2.Value)) {
                    continue;
                }
                result.Add(
                    string.Format("{0}_{1} {2} {3}", i, cell1.Def.InternalName, cell1.Value, cell2.Value)
                    );
            }
            string name = dir + @"\" + string.Format("row-cmp-{0}-{1}-{2}", row1.GetParam().Name,
                row1.ID, row2.ID);
            File.WriteAllLines(name, result);

        }



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
            menuCell.Items.Add(new ToolStripMenuItem("CopyKey", null, copyCellKey_Handler));
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
                if (row != null)
                {
                    if (cell.Key != null)
                        ParamRowUtils.SetCellValue(row.GetRow(), cell.Key, value);
                }
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

                string line = string.Format("{0};{1};{2}-{3};{4};{5}",
                    row.ID,
                    row.Name,
                    cell.ColIndex,
                    cell.Key,
                    cell.Value,
                    cell.Comment
                    );

                //lines.Add(line);
                ClipTextFile.Add(line);
                text = text + line + " ";
            }
            Clipboard.SetData(DataFormats.Text, (Object)text);

        }

        private void copyCellKey_Handler(object? sender, EventArgs e) {

            if (dataGridViewCell.SelectedRows.Count < 1)
                return;

            string text = "";
            for (int i = 0; i < dataGridViewCell.SelectedRows.Count; i++)
            {
                ParamCellItem cell = (ParamCellItem)dataGridViewCell.SelectedRows[i].DataBoundItem;

                if (i > 0)
                    text = text + " ";

                string line = cell.Key == null ? "" : cell.Key;
                ClipTextFile.Add(line);
                text = text + line;
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
            if (row == null)
                return;
            var project = GlobalConfig.GetCurrentProject();
            if (project == null)
                return;
            var refRows = ParamCellRefUtils.GetRowWrappers(project, row, cell);
            if (refRows.Count > 0)
            {
                changeRows(RowListViewMode.REF, refRows);
            }
        }

        void changeRows(int mode, List<RowWrapper> rows)
        {

            if (RowListManager.GetCurrent().Mode == RowListViewMode.FIND)
            {
                RowListManager.GoBack();
            }

            RowListManager.Add(mode, rows);
            changeGridViewRow(mode, rows, null);
        }

        void changeGridViewRow(int mode, List<RowWrapper> rows, RowWrapper? currentRow)
        {

            ClearCells();

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
                if (rows.Count > 0)
                    FillCells(rows[0]);
            }
            else
            {
                FillCells(currentRow);
                gotoRowId(currentRow.ID);
            }
        }

        void ClearCells() {
            var cells = new List<ParamCellItem>();
            bindingSourceCell.DataSource = cells;
            dataGridViewCell.DataSource = bindingSourceCell.DataSource;
        }
        void FillCells(RowWrapper row)
        {

            RowListManager.SetCurrentRow(row);
            var fieldMeta = row.GetParam().FieldMeta;
            var cells = ParamCellList.Build(row.GetParam(), row.GetRow(),fieldMeta);

            dataGridViewCell.AutoGenerateColumns = true;

            bindingSourceCell.DataSource = cells;
            dataGridViewCell.DataSource = bindingSourceCell.DataSource;

            dataGridViewCell.RowHeadersVisible = false;
            dataGridViewCell.AllowUserToResizeRows = false;
            dataGridViewCell.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCell.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCell.MultiSelect = true;
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

        private void dataGridViewCell_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (dataGridViewCell.SelectedRows.Count < 1)
            {
                toolTip1.Active = false;
                return;
            }

            ParamCellItem cell = (ParamCellItem)dataGridViewCell.SelectedRows[0].DataBoundItem;

            if (cell.IsEnumType())
            {
                toolTip1.Active = true;
                e.ToolTipText = "abcd";
            }
            else
            {
                toolTip1.Active = false;
            }

        }

        private void dataGridViewCell_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (dataGridViewCell.SelectedRows.Count < 1)
            {
                toolTip1.Active = false;
                return;
            }

            ParamCellItem cell = (ParamCellItem)dataGridViewCell.SelectedRows[0].DataBoundItem;
            if (cell.IsEnumType())
            {
                toolTip1.Active = true;
            }
            else
            {
                toolTip1.Active = false;
            }
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            while (RowListManager.GoBack()) { 
            }
            var item = RowListManager.GetCurrent();
            if (item != null)
                changeGridViewRow(item.Mode, item.Rows, item.CurrentRow);
        }
    }
}
