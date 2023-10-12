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
    public partial class ExecWaitForm : Form
    {
        public ExecWaitForm()
        {
            InitializeComponent();
        }

        int waitCount = 1000;
        private void timer1_Tick(object sender, EventArgs e)
        {
            waitCount-=100;
            if (waitCount <= 0)
            {
                timer1.Stop();
                Close();
            }
        }

        private void ExecWaitForm_Shown(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
