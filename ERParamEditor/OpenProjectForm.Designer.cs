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
            buttonDelete = new Button();
            SuspendLayout();
            // 
            // listBoxProject
            // 
            listBoxProject.FormattingEnabled = true;
            listBoxProject.ItemHeight = 17;
            listBoxProject.Location = new Point(12, 12);
            listBoxProject.Name = "listBoxProject";
            listBoxProject.Size = new Size(666, 327);
            listBoxProject.TabIndex = 0;
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(213, 356);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(75, 23);
            buttonOk.TabIndex = 1;
            buttonOk.Text = "Ok";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(309, 356);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 2;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Location = new Point(404, 356);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(75, 23);
            buttonDelete.TabIndex = 3;
            buttonDelete.Text = "Delete";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // OpenProjectForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(689, 391);
            Controls.Add(buttonDelete);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOk);
            Controls.Add(listBoxProject);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
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
        private Button buttonDelete;
    }
}