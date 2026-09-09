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
    public partial class ProgressTextForm : Form
    {
        public ProgressTextForm()
        {
            InitializeComponent();
        }


        public void Start(Form? form) {
            this.StartPosition = FormStartPosition.CenterScreen;
            Show(form);
        }

        public void UpdateText(Form? form) { 

            if ( !Visible)
               Start(form);

            label1.Text=ProgressInfo.GetGlobal().GetText();

        }

        public void UpdateTask(Form? form,Task task)
        {

            while (true) {

                if (task.IsCompleted)
                    break;
                Application.DoEvents();
                UpdateText(form);

            }
        }

        public static void ExecUpdateTask(Form? pform, Task task) {
            ProgressTextForm form = new();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.UpdateTask(pform, task);
            form.Close();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
