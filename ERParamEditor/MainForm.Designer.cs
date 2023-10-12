﻿namespace ERParamEditor
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panel1 = new Panel();
            comboBoxProjectList = new ComboBox();
            buttonOpen = new Button();
            buttonCreate = new Button();
            panelClient = new Panel();
            panelParamList = new Panel();
            panel3 = new Panel();
            buttonOpenParam = new Button();
            listViewParam = new ListView();
            splitter1 = new Splitter();
            listViewProject = new ListView();
            statusInfo = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            panelBottom = new Panel();
            buttonTest = new Button();
            buttonExec = new Button();
            comboBox1 = new ComboBox();
            errorProvider1 = new ErrorProvider(components);
            panel1.SuspendLayout();
            panelClient.SuspendLayout();
            panelParamList.SuspendLayout();
            panel3.SuspendLayout();
            statusInfo.SuspendLayout();
            panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(comboBoxProjectList);
            panel1.Controls.Add(buttonOpen);
            panel1.Controls.Add(buttonCreate);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1008, 47);
            panel1.TabIndex = 3;
            // 
            // comboBoxProjectList
            // 
            comboBoxProjectList.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxProjectList.FormattingEnabled = true;
            comboBoxProjectList.Location = new Point(93, 11);
            comboBoxProjectList.Name = "comboBoxProjectList";
            comboBoxProjectList.Size = new Size(334, 25);
            comboBoxProjectList.TabIndex = 2;
            // 
            // buttonOpen
            // 
            buttonOpen.Location = new Point(433, 12);
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new Size(75, 23);
            buttonOpen.TabIndex = 1;
            buttonOpen.Text = "Open";
            buttonOpen.UseVisualStyleBackColor = true;
            buttonOpen.Click += buttonOpen_Click;
            // 
            // buttonCreate
            // 
            buttonCreate.Location = new Point(12, 12);
            buttonCreate.Name = "buttonCreate";
            buttonCreate.Size = new Size(75, 23);
            buttonCreate.TabIndex = 0;
            buttonCreate.Text = "Create";
            buttonCreate.UseVisualStyleBackColor = true;
            buttonCreate.Click += buttonCreate_Click;
            // 
            // panelClient
            // 
            panelClient.Controls.Add(panelParamList);
            panelClient.Controls.Add(splitter1);
            panelClient.Controls.Add(listViewProject);
            panelClient.Location = new Point(0, 62);
            panelClient.Name = "panelClient";
            panelClient.Size = new Size(927, 402);
            panelClient.TabIndex = 4;
            panelClient.Paint += panelClient_Paint;
            // 
            // panelParamList
            // 
            panelParamList.Controls.Add(panel3);
            panelParamList.Controls.Add(listViewParam);
            panelParamList.Location = new Point(525, 59);
            panelParamList.Name = "panelParamList";
            panelParamList.Size = new Size(287, 237);
            panelParamList.TabIndex = 4;
            // 
            // panel3
            // 
            panel3.Controls.Add(buttonOpenParam);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 190);
            panel3.Name = "panel3";
            panel3.Size = new Size(287, 47);
            panel3.TabIndex = 5;
            // 
            // buttonOpenParam
            // 
            buttonOpenParam.Location = new Point(24, 12);
            buttonOpenParam.Name = "buttonOpenParam";
            buttonOpenParam.Size = new Size(75, 23);
            buttonOpenParam.TabIndex = 0;
            buttonOpenParam.Text = "Open";
            buttonOpenParam.UseVisualStyleBackColor = true;
            buttonOpenParam.Click += buttonOpenParam_Click;
            // 
            // listViewParam
            // 
            listViewParam.Dock = DockStyle.Fill;
            listViewParam.Location = new Point(0, 0);
            listViewParam.MultiSelect = false;
            listViewParam.Name = "listViewParam";
            listViewParam.Size = new Size(287, 237);
            listViewParam.TabIndex = 4;
            listViewParam.UseCompatibleStateImageBehavior = false;
            listViewParam.View = View.Details;
            // 
            // splitter1
            // 
            splitter1.Location = new Point(502, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(3, 402);
            splitter1.TabIndex = 2;
            splitter1.TabStop = false;
            // 
            // listViewProject
            // 
            listViewProject.Dock = DockStyle.Left;
            listViewProject.FullRowSelect = true;
            listViewProject.Location = new Point(0, 0);
            listViewProject.MultiSelect = false;
            listViewProject.Name = "listViewProject";
            listViewProject.Size = new Size(502, 402);
            listViewProject.TabIndex = 1;
            listViewProject.UseCompatibleStateImageBehavior = false;
            listViewProject.View = View.Details;
            // 
            // statusInfo
            // 
            statusInfo.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusInfo.Location = new Point(0, 570);
            statusInfo.Name = "statusInfo";
            statusInfo.Size = new Size(1008, 22);
            statusInfo.TabIndex = 10;
            statusInfo.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(0, 17);
            // 
            // panelBottom
            // 
            panelBottom.Controls.Add(buttonTest);
            panelBottom.Controls.Add(buttonExec);
            panelBottom.Controls.Add(comboBox1);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 525);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(1008, 45);
            panelBottom.TabIndex = 12;
            // 
            // buttonTest
            // 
            buttonTest.Location = new Point(326, 12);
            buttonTest.Name = "buttonTest";
            buttonTest.Size = new Size(75, 23);
            buttonTest.TabIndex = 2;
            buttonTest.Text = "Test";
            buttonTest.UseVisualStyleBackColor = true;
            buttonTest.Click += buttonTest_Click_1;
            // 
            // buttonExec
            // 
            buttonExec.Location = new Point(227, 12);
            buttonExec.Name = "buttonExec";
            buttonExec.Size = new Size(75, 23);
            buttonExec.TabIndex = 1;
            buttonExec.Text = "Exec";
            buttonExec.UseVisualStyleBackColor = true;
            buttonExec.Click += buttonExec_Click_1;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(12, 12);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(195, 25);
            comboBox1.TabIndex = 0;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 592);
            Controls.Add(panelBottom);
            Controls.Add(statusInfo);
            Controls.Add(panelClient);
            Controls.Add(panel1);
            Name = "MainForm";
            Text = "Form1";
            Load += MainForm_Load;
            Shown += MainForm_Shown;
            SizeChanged += MainForm_SizeChanged;
            panel1.ResumeLayout(false);
            panelClient.ResumeLayout(false);
            panelParamList.ResumeLayout(false);
            panel3.ResumeLayout(false);
            statusInfo.ResumeLayout(false);
            statusInfo.PerformLayout();
            panelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panel1;
        private Button buttonOpen;
        private Button buttonCreate;
        private Panel panelClient;
        private ListView listViewProject;
        private Splitter splitter1;
        private StatusStrip statusInfo;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Panel panelBottom;
        private Button buttonTest;
        private Button buttonExec;
        private ComboBox comboBox1;
        private ComboBox comboBoxProjectList;
        private Panel panelParamList;
        private ListView listViewParam;
        private Panel panel3;
        private Button buttonOpenParam;
        private ErrorProvider errorProvider1;
    }
}