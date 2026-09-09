using DotNext.Collections.Generic;
using ERParamUtils.UpdateParam;
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
    public partial class NotifyForm : Form
    {
        public NotifyForm()
        {
            InitializeComponent();
        }

        BindingSource bs;
        BindingList<string> boundList = new BindingList<string>();

        private void NotifyForm_Load(object sender, EventArgs e)
        {
            listBox1.Dock = DockStyle.Fill;
            panel1.Visible = false;

            bs = new BindingSource();

        }

        private void NotifyForm_Shown(object sender, EventArgs e)
        {
            //
        }

        private void NotifyForm_VisibleChanged(object sender, EventArgs e)
        {
             Text = "NotifyForm_VisibleChanged  "+this.Visible;
            if (this.Visible)
            {
                Text = "timer1_Start";
                timer1.Enabled = true;
                timer1.Start();
            }
            else {
                Text = "timer1_Stop";
                timer1.Enabled = false;
                timer1.Stop();
            }
        }

        public void Start() { 
        
            //timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Text = "timer1_Tick";
            listBox1.Items.Clear();
            foreach (var line in UpdateLogger.GetNotifyLines())
            {
                listBox1.Items.Add(line);
            }
            //listBox1.DataSource = boundList;

            //boundList.Clear();
            //boundList.AddAll(UpdateLogger.GetNotifyLines());


        }
    }
}
