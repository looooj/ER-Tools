namespace ERParamEditor
{
    partial class SearchEquipForm
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
            comboBoxEquipType = new ComboBox();
            textBoxId = new TextBox();
            textBoxName = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            buttonOk = new Button();
            buttonCancel = new Button();
            SuspendLayout();
            // 
            // comboBoxEquipType
            // 
            comboBoxEquipType.FormattingEnabled = true;
            comboBoxEquipType.Location = new Point(113, 23);
            comboBoxEquipType.Name = "comboBoxEquipType";
            comboBoxEquipType.Size = new Size(333, 25);
            comboBoxEquipType.TabIndex = 0;
            // 
            // textBoxId
            // 
            textBoxId.Location = new Point(113, 65);
            textBoxId.Name = "textBoxId";
            textBoxId.Size = new Size(333, 23);
            textBoxId.TabIndex = 1;
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(113, 104);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(333, 23);
            textBoxName.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 26);
            label1.Name = "label1";
            label1.Size = new Size(69, 17);
            label1.TabIndex = 3;
            label1.Text = "EquipType";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(75, 65);
            label2.Name = "label2";
            label2.Size = new Size(20, 17);
            label2.TabIndex = 4;
            label2.Text = "Id";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(52, 104);
            label3.Name = "label3";
            label3.Size = new Size(43, 17);
            label3.TabIndex = 5;
            label3.Text = "Name";
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(113, 145);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(75, 23);
            buttonOk.TabIndex = 6;
            buttonOk.Text = "Ok";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(209, 145);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 7;
            buttonCancel.Text = "Cacnel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // SearchEquipForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(486, 192);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOk);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBoxName);
            Controls.Add(textBoxId);
            Controls.Add(comboBoxEquipType);
            Name = "SearchEquipForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "SearchEquipForm";
            Load += SearchEquipForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBoxEquipType;
        private TextBox textBoxId;
        private TextBox textBoxName;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button buttonOk;
        private Button buttonCancel;
    }
}