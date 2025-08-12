using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamEditor
{
    class CheckBoxUtils
    {
    }

    public class CustomCheckBoxList { 
    

        //Control? parent;
        private TableLayoutPanel? mainPanel;
        private List<CheckBox> checkBoxItems = new();
        private string checkNamePrefix = "_check_list";

        public CustomCheckBoxList() { 
        }

        //public void SetParent(Control parent) { 
        //    this.parent = parent;
        //}

        public void Init(TableLayoutPanel panel) {
            this.mainPanel = panel;
        }

        public void AddCheckItem(string name, string description, bool initValue) {
             if (mainPanel == null) {
                return;
             }
             CheckBox item  = new CheckBox();
             item.Name = checkNamePrefix + "_" + name;
             
             item.Parent = mainPanel;
             item.Text = description;
             item.Checked = initValue;
             mainPanel.SetColumnSpan(item, mainPanel.ColumnCount);
        }

        public void AddSpace(string text) {
            if (mainPanel == null)
            {
                return;
            }
            Label label = new Label();
            label.Text = text;
            label.AutoSize = true;
            label.Parent = mainPanel;
            mainPanel.SetColumnSpan(label, mainPanel.ColumnCount);

        }
    }
}
