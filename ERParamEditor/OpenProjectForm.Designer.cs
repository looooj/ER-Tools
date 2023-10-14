namespace ERParamEditor
{
    partial class OpenProjectForm
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
            listBoxProject = new ListBox();
            buttonOk = new Button();
            buttonCancel = new Button();
            SuspendLayout();
            // 
            // listBoxProject
            // 
            listBoxProject.FormattingEnabled = true;
            listBoxProject.ItemHeight = 17;
            listBoxProject.Location = new Point(22, 12);
            listBoxProject.Name = "listBoxProject";
            listBoxProject.Size = new Size(366, 327);
            listBoxProject.TabIndex = 0;
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(102, 358);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(75, 23);
            buttonOk.TabIndex = 1;
            buttonOk.Text = "Ok";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(211, 358);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 2;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // OpenProjectForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(413, 397);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOk);
            Controls.Add(listBoxProject);
            Name = "OpenProjectForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "OpenProjectForm";
            Load += OpenProjectForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private ListBox listBoxProject;
        private Button buttonOk;
        private Button buttonCancel;
    }
}