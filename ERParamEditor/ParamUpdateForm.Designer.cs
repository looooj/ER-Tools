namespace ERParamEditor
{
    partial class ParamUpdateForm
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
            checkListBoxTask = new CheckedListBox();
            buttonUpdate = new Button();
            buttonUpdatePublish = new Button();
            checkBoxSelectAll = new CheckBox();
            checkBoxRestore = new CheckBox();
            checkListBoxUpdateCommandOption = new CheckedListBox();
            SuspendLayout();
            // 
            // checkListBoxTask
            // 
            checkListBoxTask.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            checkListBoxTask.FormattingEnabled = true;
            checkListBoxTask.Location = new Point(12, 12);
            checkListBoxTask.Name = "checkListBoxTask";
            checkListBoxTask.Size = new Size(830, 292);
            checkListBoxTask.TabIndex = 0;
            // 
            // buttonUpdate
            // 
            buttonUpdate.Location = new Point(546, 593);
            buttonUpdate.Name = "buttonUpdate";
            buttonUpdate.Size = new Size(145, 23);
            buttonUpdate.TabIndex = 1;
            buttonUpdate.Text = "Upate";
            buttonUpdate.UseVisualStyleBackColor = true;
            buttonUpdate.Click += buttonUpdate_Click;
            // 
            // buttonUpdatePublish
            // 
            buttonUpdatePublish.Location = new Point(697, 593);
            buttonUpdatePublish.Name = "buttonUpdatePublish";
            buttonUpdatePublish.Size = new Size(145, 23);
            buttonUpdatePublish.TabIndex = 2;
            buttonUpdatePublish.Text = "UpdateAndPublish";
            buttonUpdatePublish.UseVisualStyleBackColor = true;
            buttonUpdatePublish.Click += buttonUpdatePublish_Click;
            // 
            // checkBoxSelectAll
            // 
            checkBoxSelectAll.AutoSize = true;
            checkBoxSelectAll.Location = new Point(12, 311);
            checkBoxSelectAll.Name = "checkBoxSelectAll";
            checkBoxSelectAll.Size = new Size(75, 21);
            checkBoxSelectAll.TabIndex = 3;
            checkBoxSelectAll.Text = "SelectAll";
            checkBoxSelectAll.UseVisualStyleBackColor = true;
            checkBoxSelectAll.CheckedChanged += checkBoxSelectAll_CheckedChanged;
            // 
            // checkBoxRestore
            // 
            checkBoxRestore.AutoSize = true;
            checkBoxRestore.Location = new Point(425, 595);
            checkBoxRestore.Name = "checkBoxRestore";
            checkBoxRestore.Size = new Size(72, 21);
            checkBoxRestore.TabIndex = 4;
            checkBoxRestore.Text = "Restore";
            checkBoxRestore.UseVisualStyleBackColor = true;
            checkBoxRestore.Visible = false;
            // 
            // checkListBoxUpdateCommandOption
            // 
            checkListBoxUpdateCommandOption.FormattingEnabled = true;
            checkListBoxUpdateCommandOption.Location = new Point(12, 350);
            checkListBoxUpdateCommandOption.Name = "checkListBoxUpdateCommandOption";
            checkListBoxUpdateCommandOption.Size = new Size(830, 220);
            checkListBoxUpdateCommandOption.TabIndex = 5;
            // 
            // ParamUpdateForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(852, 629);
            Controls.Add(checkListBoxUpdateCommandOption);
            Controls.Add(checkBoxRestore);
            Controls.Add(checkBoxSelectAll);
            Controls.Add(buttonUpdatePublish);
            Controls.Add(buttonUpdate);
            Controls.Add(checkListBoxTask);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ParamUpdateForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ParamUpdateForm";
            Load += ParamUpdateForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckedListBox checkListBoxTask;
        private Button buttonUpdate;
        private Button buttonUpdatePublish;
        private CheckBox checkBoxSelectAll;
        private CheckBox checkBoxRestore;
        private CheckedListBox checkListBoxUpdateCommandOption;
    }
}