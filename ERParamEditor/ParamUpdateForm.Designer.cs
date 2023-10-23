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
            checkBoxListTask = new CheckedListBox();
            buttonUpdate = new Button();
            buttonUpdatePublish = new Button();
            checkBoxSelectAll = new CheckBox();
            checkBoxRestore = new CheckBox();
            SuspendLayout();
            // 
            // checkBoxListTask
            // 
            checkBoxListTask.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            checkBoxListTask.FormattingEnabled = true;
            checkBoxListTask.Location = new Point(12, 12);
            checkBoxListTask.Name = "checkBoxListTask";
            checkBoxListTask.Size = new Size(830, 310);
            checkBoxListTask.TabIndex = 0;
            // 
            // buttonUpdate
            // 
            buttonUpdate.Location = new Point(527, 332);
            buttonUpdate.Name = "buttonUpdate";
            buttonUpdate.Size = new Size(145, 23);
            buttonUpdate.TabIndex = 1;
            buttonUpdate.Text = "Upate";
            buttonUpdate.UseVisualStyleBackColor = true;
            buttonUpdate.Click += buttonUpdate_Click;
            // 
            // buttonUpdatePublish
            // 
            buttonUpdatePublish.Location = new Point(678, 332);
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
            checkBoxSelectAll.Location = new Point(32, 333);
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
            checkBoxRestore.Location = new Point(156, 333);
            checkBoxRestore.Name = "checkBoxRestore";
            checkBoxRestore.Size = new Size(72, 21);
            checkBoxRestore.TabIndex = 4;
            checkBoxRestore.Text = "Restore";
            checkBoxRestore.UseVisualStyleBackColor = true;
            // 
            // ParamUpdateForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(853, 369);
            Controls.Add(checkBoxRestore);
            Controls.Add(checkBoxSelectAll);
            Controls.Add(buttonUpdatePublish);
            Controls.Add(buttonUpdate);
            Controls.Add(checkBoxListTask);
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

        private CheckedListBox checkBoxListTask;
        private Button buttonUpdate;
        private Button buttonUpdatePublish;
        private CheckBox checkBoxSelectAll;
        private CheckBox checkBoxRestore;
    }
}