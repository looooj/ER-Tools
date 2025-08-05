namespace ERParamEditor
{
    partial class ParamUpdateForm2
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
            tabControl1 = new TabControl();
            ggg = new TabPage();
            checkedListBoxOptions = new CheckedListBox();
            tabPageOthers = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            checkBoxSelectAll = new CheckBox();
            buttonPublish = new Button();
            buttonUpdate = new Button();
            tabControl1.SuspendLayout();
            ggg.SuspendLayout();
            tabPageOthers.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(ggg);
            tabControl1.Controls.Add(tabPageOthers);
            tabControl1.Location = new Point(33, 26);
            tabControl1.Margin = new Padding(3, 2, 3, 2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(872, 421);
            tabControl1.TabIndex = 0;
            // 
            // ggg
            // 
            ggg.Controls.Add(checkedListBoxOptions);
            ggg.Location = new Point(4, 22);
            ggg.Margin = new Padding(3, 2, 3, 2);
            ggg.Name = "ggg";
            ggg.Padding = new Padding(3, 2, 3, 2);
            ggg.Size = new Size(864, 395);
            ggg.TabIndex = 0;
            ggg.Text = "General";
            ggg.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxOptions
            // 
            checkedListBoxOptions.FormattingEnabled = true;
            checkedListBoxOptions.Location = new Point(73, 84);
            checkedListBoxOptions.Name = "checkedListBoxOptions";
            checkedListBoxOptions.Size = new Size(608, 244);
            checkedListBoxOptions.TabIndex = 0;
            // 
            // tabPageOthers
            // 
            tabPageOthers.Controls.Add(tableLayoutPanel1);
            tabPageOthers.Location = new Point(4, 26);
            tabPageOthers.Margin = new Padding(3, 2, 3, 2);
            tabPageOthers.Name = "tabPageOthers";
            tabPageOthers.Padding = new Padding(3, 2, 3, 2);
            tabPageOthers.Size = new Size(864, 391);
            tabPageOthers.TabIndex = 1;
            tabPageOthers.Text = "Others";
            tabPageOthers.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Location = new Point(44, 37);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(628, 39);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(checkBoxSelectAll);
            panel1.Controls.Add(buttonPublish);
            panel1.Controls.Add(buttonUpdate);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 607);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1095, 45);
            panel1.TabIndex = 1;
            // 
            // checkBoxSelectAll
            // 
            checkBoxSelectAll.AutoSize = true;
            checkBoxSelectAll.Location = new Point(112, 17);
            checkBoxSelectAll.Name = "checkBoxSelectAll";
            checkBoxSelectAll.Size = new Size(78, 16);
            checkBoxSelectAll.TabIndex = 2;
            checkBoxSelectAll.Text = "SelectAll";
            checkBoxSelectAll.UseVisualStyleBackColor = true;
            checkBoxSelectAll.CheckedChanged += checkBoxSelectAll_CheckedChanged;
            // 
            // buttonPublish
            // 
            buttonPublish.Location = new Point(879, 11);
            buttonPublish.Margin = new Padding(3, 2, 3, 2);
            buttonPublish.Name = "buttonPublish";
            buttonPublish.Size = new Size(132, 23);
            buttonPublish.TabIndex = 1;
            buttonPublish.Text = "Publish";
            buttonPublish.UseVisualStyleBackColor = true;
            buttonPublish.Click += buttonPublish_Click;
            // 
            // buttonUpdate
            // 
            buttonUpdate.Location = new Point(730, 11);
            buttonUpdate.Margin = new Padding(3, 2, 3, 2);
            buttonUpdate.Name = "buttonUpdate";
            buttonUpdate.Size = new Size(132, 23);
            buttonUpdate.TabIndex = 0;
            buttonUpdate.Text = "Update";
            buttonUpdate.UseVisualStyleBackColor = true;
            buttonUpdate.Click += buttonUpdate_Click;
            // 
            // ParamUpdateForm2
            // 
            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1095, 652);
            Controls.Add(panel1);
            Controls.Add(tabControl1);
            Font = new Font("SimSun", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ParamUpdateForm2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ParamUpdateForm2";
            Load += ParamUpdateForm2_Load;
            tabControl1.ResumeLayout(false);
            ggg.ResumeLayout(false);
            tabPageOthers.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage ggg;
        private TabPage tabPageOthers;
        private Panel panel1;
        private Button buttonUpdate;      
        private Button buttonPublish;
        private CheckedListBox checkedListBoxOptions;
        private TableLayoutPanel tableLayoutPanel1;
        private CheckBox checkBoxSelectAll;
    }
}