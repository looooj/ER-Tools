using ERParamUtils;
using ERParamUtils.UpateParam;
using NLog;

namespace ERParamEditor
{
    public partial class MainForm : Form
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        public MainForm()
        {
            InitializeComponent();
        }

        private void panelBottom_Paint(object sender, PaintEventArgs e)
        {

        }


        private void MainForm_Load(object sender, EventArgs e)
        {



        }
        private bool FirstShow = true;

        private static async Task<int> InitLoadTask()
        {



            RowNamesManager.Load();
            ParamFieldMetaManager.Load();
            ParamProjectManager.OpenLastProject();

            return 1;
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {

            if (FirstShow)
            {
                InitControls();
                InitConfig();

                await Task.Run(InitLoadTask);

                logger.Info("===InitRefreshProject 1");
                RefreshProject();
                logger.Info("===InitRefreshProject 2");
            }
            FirstShow = false;
        }

        private ContextMenuStrip? menuProject;

        void InitControls()
        {
#if DEBUG
            buttonTest.Visible = true;
#endif

            menuProject = new();

            WindowState = FormWindowState.Maximized;
            panelClient.Dock = DockStyle.Fill;
            panelClient.BringToFront();

            panelParamList.Dock = DockStyle.Fill;

            listViewProject.Columns.Add("", 240);
            listViewProject.Columns.Add("", -2);
            listViewProject.ContextMenuStrip = menuProject;

            menuProject.Items.Add(
                  new ToolStripMenuItem("OpenInExpoler", null, OpenInExplorerClick)
              );



            listViewParam.Dock = DockStyle.Fill;
            listViewParam.Columns.Add("ParamName", 240);
            listViewParam.Columns.Add("", -2);


        }

        void InitConfig()
        {

            GlobalConfig.Load();

            SpecEquipConfig.LoadConfig();
        }

        void EnabledPanels(bool enabled)
        {
            panelBottom.Enabled = enabled;
            panelParamList.Enabled = enabled;
        }

        void RefreshProject()
        {

            /*
            comboBoxProjectList.Items.Clear();

            List<string> projectList = ParamProjectManager.GetProjectList();
            foreach (string name in projectList)
            {
                comboBoxProjectList.Items.Add(name);

            }
            if (comboBoxProjectList.Items.Count > 0)
                comboBoxProjectList.SelectedIndex = 0;
            */

            listViewProject.Items.Clear();
            listViewParam.Items.Clear();

            ListViewUtils.AddItem(listViewProject, "GlobalConfig");
            ListViewUtils.AddItem(listViewProject, "BaseDir", GlobalConfig.BaseDir);
            ListViewUtils.AddItem(listViewProject, "AssetsDir", GlobalConfig.AssetsDir);
            ListViewUtils.AddItem(listViewProject, "");

            ParamProject? project = GlobalConfig.GetCurrentProject();
            if (project == null)
            {
                ListViewUtils.AddItem(listViewProject, "CurrentProject", "?");
                EnabledPanels(false);
                return;
            }
            EnabledPanels(true);

            ListViewUtils.AddItem(listViewProject, "ProjectDir", project.GetDir());
            ListViewUtils.AddItem(listViewProject, "ProjectName", project.GetName());


            ListViewUtils.AddItem(listViewProject, "RegulationPath", project.GetRegulationPath());
            ListViewUtils.AddItem(listViewProject, "ModRegulationPath", project.GetModRegulationPath());


            foreach (var p in project.GetParamList())
            {

                ListViewUtils.AddItem(listViewParam, p);
            }


        }

        private void OpenInExplorerClick(object? sender, EventArgs e)
        {
            ProcessUtils.OpenInExplorer(GlobalConfig.GetProjectsDir());
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            CreateProjectForm form = new CreateProjectForm();
            var ret = form.ShowDialog();
            if (ret == DialogResult.OK)
            {

                var project = ParamProjectManager.CreateProject(form.ProjectName, form.RegulationPath);
                if (project != null)
                {

                    RefreshProject();
                }
            }


        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {

            OpenProjectForm form = new();
            var ret = form.ShowDialog();
            if (ret == DialogResult.OK)
            {

                var project = ParamProjectManager.OpenProject(form.ProjectName);
                if (project != null)
                {

                    RefreshProject();
                }
            }
            /*
            if (comboBoxProjectList.SelectedIndex >= 0)
            {

                string? projectName = comboBoxProjectList.Items[comboBoxProjectList.SelectedIndex].ToString();
                if (projectName != null)
                {
                    ParamProjectManager.OpenProject(projectName);

                    RefreshProject();
                }
            }
            */

        }

        private bool resizeFirst = true;
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (resizeFirst)
                listViewProject.Width = (Width / 2) - 20;
            resizeFirst = false;
        }

        private void buttonExec_Click_1(object sender, EventArgs e)
        {
            ParamProject? paramProject = GlobalConfig.GetCurrentProject();
            if (paramProject == null)
                return;

            ParamUpdateForm form = new();

            form.Exec(paramProject);
            /*
            DialogResult r = MessageBox.Show("Are you sure update Regulation.bin", "", MessageBoxButtons.YesNo);
            if (r != DialogResult.Yes)
            {
                return;
            }

            UpdateParamExector.Exec(paramProject, new UpdateParamOptions());
            */

        }

        private void listViewParam_DoubleClick(object sender, EventArgs e)
        {

        }

        private void buttonTest_Click_1(object sender, EventArgs e)
        {
            Tests.Run();

     

        }

        private void panelClient_Paint(object sender, PaintEventArgs e)
        {

        }

        void OpenParamRowForm()
        {

            ParamProject? paramProject = GlobalConfig.GetCurrentProject();
            if (paramProject == null)
                return;

            if (listViewParam.SelectedItems.Count < 1)
                return;

            var selection = listViewParam.SelectedItems[0];
            var param = paramProject.FindParam(selection.Text);
            if (param == null)
                return;

            ParamRowForm paramRowForm = new();

            RowFilter[] rowFilers = { new RowBlankNameFiler() };
            paramRowForm.CurrentRowWrappers = ParamRowUtils.ConvertToRowWrapper(param, rowFilers);
            //paramRowForm.CurrentParam = param;
            paramRowForm.ShowDialog();
        }

        private void buttonOpenParam_Click(object sender, EventArgs e)
        {
            OpenParamRowForm();
        }

        private void buttonRestore_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Are you sure", "", MessageBoxButtons.YesNo);
            if (r != DialogResult.Yes)
            {
                return;
            }

            GlobalConfig.GetCurrentProject().Restore();

        }
    }
}