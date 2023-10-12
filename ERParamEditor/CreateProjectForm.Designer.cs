namespace ERParamEditor
{
    partial class CreateProjectForm
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
            label1 = new Label();
            textFilename = new TextBox();
            buttonOk = new Button();
            buttonCancel = new Button();
            buttonSelect = new Button();
            fileDialogSelect = new OpenFileDialog();
            textProjectName = new TextBox();
            label2 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(37, 50);
            label1.Name = "label1";
            label1.Size = new Size(73, 17);
            label1.TabIndex = 0;
            label1.Text = "Regulation:";
            // 
            // textFilename
            // 
            textFilename.Location = new Point(116, 47);
            textFilename.Name = "textFilename";
            textFilename.Size = new Size(728, 23);
            textFilename.TabIndex = 1;
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(116, 100);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(75, 23);
            buttonOk.TabIndex = 2;
            buttonOk.Text = "Ok";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(222, 100);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonSelect
            // 
            buttonSelect.Location = new Point(862, 47);
            buttonSelect.Name = "buttonSelect";
            buttonSelect.Size = new Size(75, 23);
            buttonSelect.TabIndex = 4;
            buttonSelect.Text = "SelectFile";
            buttonSelect.UseVisualStyleBackColor = true;
            buttonSelect.Click += buttonSelect_Click;
            // 
            // fileDialogSelect
            // 
            fileDialogSelect.FileName = "regulation.bin";
            // 
            // textProjectName
            // 
            textProjectName.Location = new Point(116, 15);
            textProjectName.Name = "textProjectName";
            textProjectName.Size = new Size(270, 23);
            textProjectName.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 21);
            label2.Name = "label2";
            label2.Size = new Size(86, 17);
            label2.TabIndex = 5;
            label2.Text = "ProjectName:";
            // 
            // CreateProjectForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(950, 150);
            Controls.Add(textProjectName);
            Controls.Add(label2);
            Controls.Add(buttonSelect);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOk);
            Controls.Add(textFilename);
            Controls.Add(label1);
            Name = "CreateProjectForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CreateProject";
            Load += CreateProjectForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textFilename;
        private Button buttonOk;
        private Button buttonCancel;
        private Button buttonSelect;
        private OpenFileDialog fileDialogSelect;
        private TextBox textProjectName;
        private Label label2;
    }
}