namespace ERBackup
{
    partial class BackupForm
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
            panel1 = new Panel();
            autoBakCheckBox = new CheckBox();
            showAutoBak = new CheckBox();
            restoreBackupButton = new Button();
            createBackupButton = new Button();
            zipListBox = new ListBox();
            listBoxConfig = new ListBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(autoBakCheckBox);
            panel1.Controls.Add(showAutoBak);
            panel1.Controls.Add(restoreBackupButton);
            panel1.Controls.Add(createBackupButton);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 497);
            panel1.Name = "panel1";
            panel1.Size = new Size(706, 54);
            panel1.TabIndex = 1;
            // 
            // autoBakCheckBox
            // 
            autoBakCheckBox.AutoSize = true;
            autoBakCheckBox.Location = new Point(345, 18);
            autoBakCheckBox.Name = "autoBakCheckBox";
            autoBakCheckBox.Size = new Size(140, 21);
            autoBakCheckBox.TabIndex = 3;
            autoBakCheckBox.Text = "AutoBackupCurrent";
            autoBakCheckBox.UseVisualStyleBackColor = true;
            // 
            // showAutoBak
            // 
            showAutoBak.AutoSize = true;
            showAutoBak.Location = new Point(28, 18);
            showAutoBak.Name = "showAutoBak";
            showAutoBak.Size = new Size(128, 21);
            showAutoBak.TabIndex = 2;
            showAutoBak.Text = "ShowAutoBackup";
            showAutoBak.UseVisualStyleBackColor = true;
            showAutoBak.CheckedChanged += showAutoBak_CheckedChanged;
            // 
            // restoreBackupButton
            // 
            restoreBackupButton.Location = new Point(509, 17);
            restoreBackupButton.Name = "restoreBackupButton";
            restoreBackupButton.Size = new Size(148, 23);
            restoreBackupButton.TabIndex = 1;
            restoreBackupButton.Text = "RestoreBackup";
            restoreBackupButton.UseVisualStyleBackColor = true;
            restoreBackupButton.Click += restoreBackupButton_Click;
            // 
            // createBackupButton
            // 
            createBackupButton.Location = new Point(162, 17);
            createBackupButton.Name = "createBackupButton";
            createBackupButton.Size = new Size(148, 23);
            createBackupButton.TabIndex = 0;
            createBackupButton.Text = "CreateBackup";
            createBackupButton.UseVisualStyleBackColor = true;
            createBackupButton.Click += createBackupButton_Click;
            // 
            // zipListBox
            // 
            zipListBox.FormattingEnabled = true;
            zipListBox.ItemHeight = 17;
            zipListBox.Location = new Point(12, 12);
            zipListBox.Name = "zipListBox";
            zipListBox.Size = new Size(675, 378);
            zipListBox.TabIndex = 2;
            // 
            // listBoxConfig
            // 
            listBoxConfig.FormattingEnabled = true;
            listBoxConfig.ItemHeight = 17;
            listBoxConfig.Location = new Point(12, 405);
            listBoxConfig.Name = "listBoxConfig";
            listBoxConfig.Size = new Size(675, 72);
            listBoxConfig.TabIndex = 3;
            // 
            // BackupForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(706, 551);
            Controls.Add(listBoxConfig);
            Controls.Add(zipListBox);
            Controls.Add(panel1);
            Name = "BackupForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "BackupForm";
            Load += BackupForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private Button restoreBackupButton;
        private Button createBackupButton;
        private ListBox zipListBox;
        private CheckBox showAutoBak;
        private CheckBox autoBakCheckBox;
        private ListBox listBoxConfig;
    }
}