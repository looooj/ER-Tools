using ERParamUtils;
using ERParamUtils.UpateParam;
using ERParamUtils.UpdateParam;
using MultiLangLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace ERParamEditor
{
    public partial class ParamUpdateForm3 : Form
    {
        public ParamUpdateForm3()
        {
            InitializeComponent();
            showFirstFlag = true;
        }

        private void ParamUpdateForm3_Load(object sender, EventArgs e)
        {
            InitControls();
        }

        List<UpdateParamTask> updateParamTasks = new();
        List<UpdateParamOptionItem> updateParamOptions = new();
        List<CustomTablePanel> customTablePanels = new();
        DictConfig currentConfig = new DictConfig();

        public ParamProject paramProject;

        private void InitControls()
        {
            //currentConfig = ParamUpdateFormUtils.loadOptions();
            UpdateParamExector.CreateTaskList(updateParamTasks);
            MultiLang.ApplyMessage(updateParamTasks);


            UpdateParamExector.CreateOptionList(updateParamOptions);
            MultiLang.ApplyMessage(updateParamOptions);
            tabControl1.Dock = DockStyle.Fill;

            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel4.Dock = DockStyle.Fill;

            InitPages();
        }

        private void InitPages()
        {
            currentConfig = ParamUpdateFormUtils.loadOptions();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel2.RowStyles.Clear();
            tableLayoutPanel3.RowStyles.Clear();
            tableLayoutPanel4.RowStyles.Clear();
            //
            InitPageA(tableLayoutPanel1);
            InitPageB(tableLayoutPanel1);
            //
            InitPageC(tableLayoutPanel2);
            //
            InitPageD(tableLayoutPanel3);
            //
            InitPageScript(tableLayoutPanel4);


            for (int i = 0; i < customTablePanels.Count; i++)
            {
                customTablePanels[i].SetValues(currentConfig);
            }

            MultiLang.ApplyForm(this, "ParamUpdateForm");

        }


        private void InitPageA(TableLayoutPanel control)
        {

            CustomTablePanel panel = new CustomTablePanel();
            customTablePanels.Add(panel);
            panel.Init(control);
            //panel.AddSpace("---");

            for (int i = 0; i < updateParamOptions.Count; i++)
            {
                var option = updateParamOptions[i];
                panel.AddCheckBoxList(option.Name, option.Description);
            }


            //panel.SetValues(currentConfig);
        }



        private void InitPageB(TableLayoutPanel control)
        {
            CustomTablePanel panel = new CustomTablePanel();
            customTablePanels.Add(panel);
            panel.Init(control);
            panel.AddSpace("------");
            ///
            string runeNames = MultiLang.GetText("UpdateParam", UpdateParamOptionNames.ReplaceGoldenRune,
                ReplaceGoldenRune.GetNameList());
            panel.AddSelectionNameValue(
                UpdateParamOptionNames.ReplaceGoldenRune,
                UpdateParamOptionNames.ReplaceGoldenRune,
                runeNames, ReplaceGoldenRune.GetValueList(), "0");
                
            ///
            string rateList = "1,2,3,4,5,10,20,50";
            panel.AddSelectionNameValue(UpdateParamOptionNames.GetRuneRate,
                UpdateParamOptionNames.GetRuneRate, rateList, rateList, "1");

            ///
            string unlockGraceNames = MultiLang.GetText("UpdateParam", UpdateParamOptionNames.UnlockGrace,
                 UnlockGraceConfig.GetNameList());
            panel.AddSelectionNameValue(
               UpdateParamOptionNames.UnlockGrace,
               UpdateParamOptionNames.UnlockGrace,
               unlockGraceNames, UnlockGraceConfig.GetValueList(), "0");

            panel.AddCheckBox(UpdateParamOptionNames.UnlockRoundtableHold, "");


        }

        private void InitPageC(TableLayoutPanel control)
        {
            CustomTablePanel panel = new CustomTablePanel();
            customTablePanels.Add(panel);
            panel.Init(control);

            var updateTalismanOptions = UpdateTalisman.GetUpdateParams();
            MultiLang.ApplyMessage(updateTalismanOptions);

            for (int i = 0; i < updateTalismanOptions.Count; i++)
            {
                var item = updateTalismanOptions[i];
                panel.AddCheckBox(item.Name, item.Description);
            }
        }

        private void InitPageD(TableLayoutPanel control) {

            CustomTablePanel panel = new CustomTablePanel();
            customTablePanels.Add(panel);
            panel.Init(control);

            //List<UpdateParamOptionItem> updateParamOptionItems = new List<UpdateParamOptionItem>();

            panel.AddCheckBox(UpdateParamOptionNames.AddInitCrimsonAmberMedallion, "");
            panel.AddCheckBox(UpdateParamOptionNames.AddInit99Rune, "");
            panel.AddCheckBox(UpdateParamOptionNames.AddInitMimicTear, "");
            panel.AddCheckBox(UpdateParamOptionNames.AddInitPurebloodKnightMeda, "");

            var bowNames = MultiLang.GetText("UpdateParam", UpdateParamOptionNames.ReplaceInitBow,
                WeaponConfig.GetReplaceBowNames());
            var bowIds = WeaponConfig.GetReplaceBowIds();
            panel.AddSelectionNameValue(UpdateParamOptionNames.ReplaceInitBow,
                UpdateParamOptionNames.ReplaceInitBow, bowNames, bowIds,"0");

            var wepNames =
                MultiLang.GetText("UpdateParam", UpdateParamOptionNames.ReplaceInitWeaponRight3,
                WeaponConfig.GetReplaceWeaponRightNames());
            var wepIds = WeaponConfig.GetReplaceWeaponRightIds();

            panel.AddSelectionNameValue(UpdateParamOptionNames.ReplaceInitWeaponRight2,
              UpdateParamOptionNames.ReplaceInitWeaponRight2, wepNames, wepIds, "0");

            panel.AddSelectionNameValue(UpdateParamOptionNames.ReplaceInitWeaponRight3,
              UpdateParamOptionNames.ReplaceInitWeaponRight3, wepNames, wepIds, "0");


            var iWepIds = WeaponConfig.GetReplaceStaffIds();
            var iWepNames = WeaponConfig.GetReplaceStaffNames();
            panel.AddSelectionNameValue(UpdateParamOptionNames.ReplaceInitStaff,
                UpdateParamOptionNames.ReplaceInitStaff, iWepNames, iWepIds, "0");

            var dWepIds = WeaponConfig.GetReplaceDexterityWeaponIds();
            var dWepNames = WeaponConfig.GetReplaceDexterityWeaponNames();
            panel.AddSelectionNameValue(UpdateParamOptionNames.ReplaceInitDexterityWeapon,
                UpdateParamOptionNames.ReplaceInitDexterityWeapon, dWepNames, dWepIds, "0");


            var leftIds = WeaponConfig.GetAddWeaponLeftIds();
            var leftNames = WeaponConfig.GetAddWeaponLeftNames();
            panel.AddSelectionNameValue(UpdateParamOptionNames.AddInitWeaponLeft3,
                UpdateParamOptionNames.AddInitWeaponLeft3, leftNames, leftIds, "0");


            var shieldIds = WeaponConfig.GetShieldIds();
            var shieldNames = WeaponConfig.GetShieldNames();
            panel.AddSelectionNameValue(UpdateParamOptionNames.ReplaceInitShield,
                UpdateParamOptionNames.ReplaceInitShield, shieldNames, shieldIds, "0");



            panel.AddCheckBox(UpdateParamOptionNames.RemoveInitWeaponWeightRequire,"");

        }

        private void InitPageScript(TableLayoutPanel control)
        {
            CustomTablePanel panel = new CustomTablePanel();
            customTablePanels.Add(panel);
            panel.Init(control);

            for (int i = 0; i < updateParamTasks.Count; i++)
            {
                var option = updateParamTasks[i];
                panel.AddCheckBox(option.UpdateName, option.Description);
            }
        }


        public void Exec(ParamProject paramProject)
        {

            ParamUpdateFormUtils.Init(paramProject);
            this.paramProject = paramProject;
            ShowDialog();
        }

        bool showFirstFlag = true;
        private void ParamUpdateForm3_Shown(object sender, EventArgs e)
        {

            //if (showFirstFlag)
            //    InitPages();
            showFirstFlag = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }


        void execUpdatePublish(string? msg, bool publishFlag) {

            DictConfig config = new();
            for (int i = 0; i < customTablePanels.Count; i++)
            {
                customTablePanels[i].GetValues(config);
            }

            ParamUpdateFormUtils.saveOptions(config);
            ParamUpdateFormUtils.ExecUpdatePublish(this, msg, publishFlag, config);
            Close();
        }

        private void buttonPublish_Click(object sender, EventArgs e)
        {
            string msg = MultiLang.GetDefaultText("publish", "Are you sure exec publish?");
            execUpdatePublish(msg, true);
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            execUpdatePublish(null, false);
        }
    }
}
