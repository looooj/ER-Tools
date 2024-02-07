namespace ERParamEditor
{
    partial class CompareProjectForm
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
            comboBoxProject1 = new ComboBox();
            comboBoxProject2 = new ComboBox();
            buttonOk = new Button();
            buttonCancel = new Button();
            label1 = new Label();
            label2 = new Label();
            checkedListBoxParam = new CheckedListBox();
            checkBoxSelectAll = new CheckBox();
            SuspendLayout();
            // 
            // comboBoxProject1
            // 
            comboBoxProject1.FormattingEnabled = true;
            comboBoxProject1.Location = new Point(101, 22);
            comboBoxProject1.Name = "comboBoxProject1";
            comboBoxProject1.Size = new Size(396, 25);
            comboBoxProject1.TabIndex = 0;
            // 
            // comboBoxProject2
            // 
            comboBoxProject2.FormattingEnabled = true;
            comboBoxProject2.Location = new Point(101, 62);
            comboBoxProject2.Name = "comboBoxProject2";
            comboBoxProject2.Size = new Size(395, 25);
            comboBoxProject2.TabIndex = 1;
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(304, 363);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(75, 23);
            buttonOk.TabIndex = 2;
            buttonOk.Text = "Ok";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(435, 363);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(29, 26);
            label1.Name = "label1";
            label1.Size = new Size(55, 17);
            label1.TabIndex = 4;
            label1.Text = "Project1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(29, 66);
            label2.Name = "label2";
            label2.Size = new Size(55, 17);
            label2.TabIndex = 5;
            label2.Text = "Project2";
            // 
            // checkedListBoxParam
            // 
            checkedListBoxParam.FormattingEnabled = true;
            checkedListBoxParam.Location = new Point(28, 119);
            checkedListBoxParam.Name = "checkedListBoxParam";
            checkedListBoxParam.Size = new Size(760, 220);
            checkedListBoxParam.TabIndex = 6;
            // 
            // checkBoxSelectAll
            // 
            checkBoxSelectAll.AutoSize = true;
            checkBoxSelectAll.Location = new Point(45, 351);
            checkBoxSelectAll.Name = "checkBoxSelectAll";
            checkBoxSelectAll.Size = new Size(75, 21);
            checkBoxSelectAll.TabIndex = 7;
            checkBoxSelectAll.Text = "SelectAll";
            checkBoxSelectAll.UseVisualStyleBackColor = true;
            checkBoxSelectAll.CheckedChanged += checkBoxSelectAll_CheckedChanged;
            // 
            // CompareProjectForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(813, 400);
            Controls.Add(checkBoxSelectAll);
            Controls.Add(checkedListBoxParam);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOk);
            Controls.Add(comboBoxProject2);
            Controls.Add(comboBoxProject1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CompareProjectForm";
            Text = "CompareProjectForm";
            Load += CompareProjectForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBoxProject1;
        private ComboBox comboBoxProject2;
        private Button buttonOk;
        private Button buttonCancel;
        private Label label1;
        private Label label2;
        private CheckedListBox checkedListBoxParam;
        private CheckBox checkBoxSelectAll;
    }
}