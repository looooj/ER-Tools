namespace ERParamEditor
{
    partial class NotifyForm
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
            listBox1 = new ListBox();
            panel1 = new Panel();
            buttonOk = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 17;
            listBox1.Location = new Point(12, 12);
            listBox1.Name = "listBox1";
            listBox1.ScrollAlwaysVisible = true;
            listBox1.Size = new Size(416, 412);
            listBox1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(buttonOk);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 430);
            panel1.Name = "panel1";
            panel1.Size = new Size(440, 43);
            panel1.TabIndex = 1;
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(163, 8);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(75, 23);
            buttonOk.TabIndex = 0;
            buttonOk.Text = "Ok";
            buttonOk.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // NotifyForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(440, 473);
            Controls.Add(panel1);
            Controls.Add(listBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "NotifyForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NotifyForm";
            Load += NotifyForm_Load;
            Shown += NotifyForm_Shown;
            VisibleChanged += NotifyForm_VisibleChanged;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ListBox listBox1;
        private Panel panel1;
        private System.Windows.Forms.Timer timer1;
        private Button buttonOk;
    }
}