namespace ERParamEditor
{
    partial class ParamRowForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            splitter1 = new Splitter();
            panelClient = new Panel();
            panelCell = new Panel();
            dataGridViewCell = new DataGridView();
            splitter2 = new Splitter();
            dataGridViewRow = new DataGridView();
            statusStrip1 = new StatusStrip();
            panelBottom = new Panel();
            bindingSourceRow = new BindingSource(components);
            bindingSourceCell = new BindingSource(components);
            toolTip1 = new ToolTip(components);
            buttonReload = new Button();
            panelClient.SuspendLayout();
            panelCell.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCell).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewRow).BeginInit();
            panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)bindingSourceRow).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bindingSourceCell).BeginInit();
            SuspendLayout();
            // 
            // splitter1
            // 
            splitter1.Location = new Point(0, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(3, 450);
            splitter1.TabIndex = 2;
            splitter1.TabStop = false;
            // 
            // panelClient
            // 
            panelClient.Controls.Add(panelCell);
            panelClient.Controls.Add(splitter2);
            panelClient.Controls.Add(dataGridViewRow);
            panelClient.Location = new Point(31, 34);
            panelClient.Name = "panelClient";
            panelClient.Size = new Size(721, 329);
            panelClient.TabIndex = 3;
            // 
            // panelCell
            // 
            panelCell.Controls.Add(dataGridViewCell);
            panelCell.Dock = DockStyle.Fill;
            panelCell.Location = new Point(622, 0);
            panelCell.Name = "panelCell";
            panelCell.Size = new Size(99, 329);
            panelCell.TabIndex = 2;
            // 
            // dataGridViewCell
            // 
            dataGridViewCell.BackgroundColor = SystemColors.Control;
            dataGridViewCell.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCell.Dock = DockStyle.Fill;
            dataGridViewCell.Location = new Point(0, 0);
            dataGridViewCell.Name = "dataGridViewCell";
            dataGridViewCell.RowTemplate.Height = 25;
            dataGridViewCell.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCell.Size = new Size(99, 329);
            dataGridViewCell.TabIndex = 0;
            dataGridViewCell.CellMouseMove += dataGridViewCell_CellMouseMove;
            dataGridViewCell.CellToolTipTextNeeded += dataGridViewCell_CellToolTipTextNeeded;
            // 
            // splitter2
            // 
            splitter2.Location = new Point(619, 0);
            splitter2.Name = "splitter2";
            splitter2.Size = new Size(3, 329);
            splitter2.TabIndex = 1;
            splitter2.TabStop = false;
            // 
            // dataGridViewRow
            // 
            dataGridViewRow.AllowUserToAddRows = false;
            dataGridViewRow.AllowUserToDeleteRows = false;
            dataGridViewRow.BackgroundColor = SystemColors.Control;
            dataGridViewRow.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewRow.Dock = DockStyle.Left;
            dataGridViewRow.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridViewRow.EnableHeadersVisualStyles = false;
            dataGridViewRow.Location = new Point(0, 0);
            dataGridViewRow.Name = "dataGridViewRow";
            dataGridViewRow.ReadOnly = true;
            dataGridViewRow.RowTemplate.Height = 25;
            dataGridViewRow.Size = new Size(619, 329);
            dataGridViewRow.TabIndex = 0;
            // 
            // statusStrip1
            // 
            statusStrip1.Location = new Point(3, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(797, 22);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // panelBottom
            // 
            panelBottom.Controls.Add(buttonReload);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(3, 386);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(797, 42);
            panelBottom.TabIndex = 5;
            // 
            // toolTip1
            // 
            toolTip1.Active = false;
            // 
            // buttonReload
            // 
            buttonReload.Location = new Point(9, 10);
            buttonReload.Name = "buttonReload";
            buttonReload.Size = new Size(75, 23);
            buttonReload.TabIndex = 0;
            buttonReload.Text = "Reload";
            buttonReload.UseVisualStyleBackColor = true;
            buttonReload.Click += buttonReload_Click;
            // 
            // ParamRowForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panelBottom);
            Controls.Add(statusStrip1);
            Controls.Add(panelClient);
            Controls.Add(splitter1);
            Name = "ParamRowForm";
            ShowInTaskbar = false;
            Text = "ParamRowForm";
            FormClosed += ParamRowForm_FormClosed;
            Load += ParamRowForm_Load;
            Shown += ParamRowForm_Shown;
            ResizeEnd += ParamRowForm_ResizeEnd;
            panelClient.ResumeLayout(false);
            panelCell.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewCell).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewRow).EndInit();
            panelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)bindingSourceRow).EndInit();
            ((System.ComponentModel.ISupportInitialize)bindingSourceCell).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Splitter splitter1;
        private Panel panelClient;
        private StatusStrip statusStrip1;
        private Panel panelBottom;
        private Panel panelCell;
        private DataGridView dataGridViewCell;
        private Splitter splitter2;
        private DataGridView dataGridViewRow;
        private BindingSource bindingSourceRow;
        private BindingSource bindingSourceCell;
        private ToolTip toolTip1;
        private Button buttonReload;
    }
}