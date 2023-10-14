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

            //if (CurrentParam == null)
            //    return;

            //Text = CurrentParam.ParamType;

            InitMenuRow();
            InitMenuCell();

            InitRows();

        }

        void InitMenuRow() {
            if (menuRow == null)
                menuRow = new();

            menuRow.Items.Add(new ToolStripMenuItem("FindRowByName", null, findRowName_Handler));
            menuRow.Items.Add(new ToolStripSeparator());
            menuRow.Items.Add(new ToolStripMenuItem("CopyRow", null, copyRow_Handler));
            menuRow.Items.Add(new ToolStripSeparator());
            menuRow.Items.Add(new ToolStripMenuItem("CopyRowName", null, copyRowName_Handler));
            menuRow.Items.Add(new ToolStripSeparator());
            menuRow.Items.Add(new ToolStripMenuItem("CopyRowId", null, copyRowId_Handler));
            menuRow.Items.Add(new ToolStripSeparator());
            menuRow.Items.Add(new ToolStripMenuItem("Export", null, exportRowButton_Click));

        }


        void exportRow(RowWrapper row, string fn) {

            var cells = ParamCellList.Build(row.GetParam(), row.GetRow());
            var lines = new List<string>();
            foreach(var cell in cells) {

                lines.Add(string.Format("{0},{1},{2},{3},{4}",
                    cell.ColIndex,cell.DisplayName,cell.Key,cell.Value,cell.Comment));
                    

            }
            File.WriteAllLines(fn, lines);
        }

        private void exportRowButton_Click(object? sender, EventArgs e)
        {
            if (dataGridViewRow.SelectedRows.Count < 1)
                return;
            string dir = GlobalConfig.GetCurrentProject().GetDir()+@"\exp";
            Directory.CreateDirectory(dir);

            for (int i = 0; i < dataGridViewRow.SelectedRows.Count; i++)
            {
                object obj = dataGridViewRow.SelectedRows[i].DataBoundItem;
                RowWrapper row = (RowWrapper)obj;
                string fn = dir + @"\" + row.GetParam().Name + "-" + row.ID + "-" + row.Name + ".txt";

                exportRow(row,fn);
                
            }

        }


        private void copyRow(bool rowId, bool rowName) {

            if (dataGridViewRow.SelectedRows.Count < 1)
                return;
            string text = "";
            for (int i = 0; i < dataGridViewRow.SelectedRows.Count; i++)
            {
                object obj = dataGridViewRow.SelectedRows[i].DataBoundItem;
                RowWrapper row = (RowWrapper)obj;
                if ( rowId )
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

                if (row.Name.Contains(text))
                {
                    rows.Add(row);
                    continue;
                }

            }
            bindingSourceRow.DataSource = rows;
            dataGridViewRow.DataSource = bindingSourceRow.DataSource;
        }

        public List<RowWrapper>? CurrentRowWrappers;
        void InitRows()
        {
            if (CurrentRowWrappers == null)
                return;


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

        /*
        void InitCellEditor(FSParam.Param.Row row,DataGridView dataGridView) {

                for (int i = 0; i < dataGridView.RowCount; i++)
                {

                    ParamCellItem cell = (ParamCellItem)
                    

                    if (cell.InternalType.Length > 6)
                    {
                        Dictionary<object, string> dict = 
                        ErValueDict.GetDict(cell1.Def.InternalType);

                        if (dict == null)
                            continue;
                        var items = dict.ToArray();
                        dataGridView.Rows[i].Cells[2] = new DataGridViewComboBoxCell
                        {
                            DataSource = items,
                            DisplayMember = "Value",
                            ValueMember = "Key",
                            ValueType = cell.Value.GetType()
                        };
                    }
                }

            

        } */

        void InitMenuCell()
        {
            if (menuCell == null)
                menuCell = new();
            menuCell.Items.Add(new ToolStripMenuItem("CopyCell", null, copyCell_Handler));

        }

        private void copyCell_Handler(object? sender, EventArgs e)
        {
            if (dataGridViewCell.SelectedRows.Count < 1)
                return;

            //List<string> lines = new();
            string text = "";
            for (int i = 0; i < dataGridViewCell.SelectedRows.Count; i++)
            {
                ParamCellItem cell = (ParamCellItem)dataGridViewCell.SelectedRows[i].DataBoundItem;

                string line = string.Format("{0};{1};{2};{3}", 
                    currentParamRow.ID,
                    currentParamRow.Name,
                    cell.Key,
                    cell.Value
                    );

                //lines.Add(line);
                ClipTextFile.Add(line);
                text = text + line + " ";
            }
            Clipboard.SetData(DataFormats.Text, (Object)text);

        }

        private RowWrapper currentParamRow;
        void FillCells(RowWrapper row)
        {


            currentParamRow = row;
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
    }
}
