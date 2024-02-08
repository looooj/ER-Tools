using ERParamUtils;
using ERParamUtils.UpdateParam;
using MultiLangLib;
using NLog;
using System.Globalization;

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

            try
            {

                RowNamesManager.Load();
                ParamFieldMetaManager.Load();
                ParamNames.LoadDesc(MultiLang.GetLangId());
                ParamProjectManager.OpenLastProject();
            }
            catch (Exception e)
            {
                logger.Info("===InitLoadTask {}",e);
                return 0;
            }

            return 1;
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {

            if (FirstShow)
            {
                Cursor = Cursors.WaitCursor;

                InitConfig();
                InitControls();

                int r = await Task.Run(InitLoadTask);

                logger.Info("===InitRefreshProject 1");
                RefreshProject();
                logger.Info("===InitRefreshProject 2");
                Cursor = Cursors.Default;
            }
            FirstShow = false;
        }

        private ContextMenuStrip? menuProject;

        void InitControls()
        {

            if (GlobalConfig.Debug)
            {
                buttonTest.Visible = true;
            }

            menuProject = new();

            WindowState = FormWindowState.Maximized;
            panelClient.Dock = DockStyle.Fill;
            panelClient.BringToFront();

            panelParamList.Dock = DockStyle.Fill;

            listViewProject.Columns.Add("", 240);
            listViewProject.Columns.Add("", -2);
            listViewProject.ContextMenuStrip = menuProject;


            menuProject.Items.Add(
                  new ToolStripMenuItem("OpenInExplorer", null, OpenInExplorerClick)
              );



            listViewParam.Dock = DockStyle.Fill;
            listViewParam.Columns.Add("ParamName", 440);
            listViewParam.Columns.Add("", -2);


        }

        void InitConfig()
        {

            GlobalConfig.Load();

            SpecEquipConfig.LoadConfig();
            ParamNameFilter.LoadParamNames();
            MultiLang.InitDefault("ERParamEditor");
            MultiLang.ApplyForm(this, "MainForm");
        }

        void EnabledPanels(bool enabled)
        {
            panelBottom.Enabled = enabled;
            panelParamList.Enabled = enabled;
        }

        void RefreshProject()
        {


            listViewProject.Items.Clear();
            listViewParam.Items.Clear();

            //ListViewUtils.AddItem(listViewProject, "CurrentCulture", CultureInfo.CurrentCulture.ThreeLetterISOLanguageName);


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

            bool nameFilter = GlobalConfig.UseParamNameFilter;
            var paramList = project.GetParamNameList(nameFilter);
            foreach (var p in paramList)
            {
                string? desc = ParamNames.GetDesc(p);
                if ( desc != null )
                    ListViewUtils.AddItem(listViewParam, p+ " | "+desc);
                else
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
                    bool changeCursor = false;
                    if (Cursor != Cursors.WaitCursor)
                    {
                        Cursor = Cursors.WaitCursor;
                        changeCursor = true;
                    }

                    RefreshProject();

                    if (changeCursor)
                        Cursor = Cursors.Default;
                }
            }
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

        }

        private void listViewParam_DoubleClick(object sender, EventArgs e)
        {

        }

        private void buttonTest_Click_1(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                Tests.Run();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
            Cursor = Cursors.Default;
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
            var selectionText = selection.Text;
            var items = selectionText.Split(" | ");
            var param = paramProject.FindParam(items[0]);
            if (param == null)
                return;

            ParamRowForm paramRowForm = new();

            List<RowFilter> rowFilers = new();

            List<RowBuilder> rowBuilders = new();

            rowBuilders.Add(new SpEffectSetParamRowBuilder());
            rowFilers.Add(new CharaInitFilter());
            var rows = ParamRowUtils.ConvertToRowWrapper(paramProject, param, rowFilers, rowBuilders);
            RowListManager.Add(0, rows);
            //var rowListItem = RowListManager.GetCurrent();
            //if (rowListItem == null)
            //    return;

            paramRowForm.ParamName = param.Name;
            paramRowForm.ShowDialog();
        }

        private void buttonOpenParam_Click(object sender, EventArgs e)
        {
            OpenParamRowForm();
        }

        private void buttonRestore_Click(object sender, EventArgs e)
        {
            string msg = MultiLang.GetText("Are you sure exec restore?");
            DialogResult r = MessageBox.Show(msg, "", MessageBoxButtons.YesNo);
            if (r != DialogResult.Yes)
            {
                return;
            }

            GlobalConfig.GetCurrentProject().Restore();

        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            SearchEquipForm form = new();
            form.ShowDialog();
        }

        private void buttonCompare_Click(object sender, EventArgs e)
        {
            CompareProjectForm form = new();
            form.ShowDialog();
        }
    }
}