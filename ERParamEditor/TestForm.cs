using ERParamUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERParamEditor
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {

            initControls();
        }

        private void initControls()
        {

            init02();
        }
        DictConfig dictConfig = new DictConfig();
        CustomTablePanel customPanel = new();
        private void init02()
        {

            dictConfig.SetString("bbb", "1");
            dictConfig.SetString("rune", "3");

            customPanel.Init(tableLayoutPanel1);

            customPanel.AddCheckBox("aaa", "aaa");
            customPanel.AddCheckBox("bbb", "bbb");
            customPanel.AddCheckBox("ccc", "ccc");

            customPanel.AddSelectionNameValue("rune", "卢恩获取倍数", "a,b,c", "1,2,3", "2");

            customPanel.SetValues(dictConfig);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var config = new DictConfig();

            customPanel.GetValues(config);

            textBox1.Text = config.GetKeyValueString();
        }

        /*
        CustomCheckBoxList boxList = new CustomCheckBoxList();

        private void init01() {

            //tableLayoutPanel1.

            boxList.Init(tableLayoutPanel1);
            boxList.AddCheckItem("a1", "aa1", false);
            boxList.AddCheckItem("a2", "aa2", false);
            boxList.AddCheckItem("a3", "aa3", false);

            boxList.AddSpace("-----");

            boxList.AddCheckItem("b1", "bb1", false);
            boxList.AddCheckItem("b2", "bb2", false);
            boxList.AddCheckItem("b3", "bb3", false);

        }*/
    }
}
