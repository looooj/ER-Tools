using ERParamUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERParamEditor
{
    public class ControlUtils
    {
        public static void AddInput(Control parent, string text, string value) {

            Label label1 = new();
            label1.Parent = parent;
            label1.Text = text;
            label1.TextAlign = ContentAlignment.MiddleRight;

            TextBox textBox1 = new TextBox();
            textBox1.Parent = parent;
            textBox1.Text = value;
        }

        public static void AddSelection2(Control parent, string namePrefix, string text, string selections, string value)
        {

            Label label1 = new();
            label1.Name = namePrefix + "_label";
            label1.Parent = parent;
            label1.Text = text;
            label1.TextAlign = ContentAlignment.MiddleRight;
            label1.Width = 300;
            ComboBox comboBox1 = new ComboBox();
            comboBox1.Name = namePrefix + "_selections";
            comboBox1.Parent = parent;
            var items = selections.Split(",");

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.AddRange(items);
            for (int i = 0; i < items.Length; i++) {
                if (value == items[i]) { 
                    comboBox1.SelectedIndex = i;
                }
            }
        }

        public static object GetComboBoxValue(ComboBox comboBox ) {

            return comboBox.SelectedValue;
        }

        public static Control FindControl(Control parent, string name) { 
            Control control = null;
            var items = parent.Controls;
            for (int i = 0; i < items.Count; i++) {
                if (control != null)
                    return control;
                var item = items[i];
                if (item.Name == name) {
                    control = item;
                    break;
                }
                control = FindControl(item, name);
            }
            return control;
        }

        class SelectionItem {
            public string ItemName { get; set; }
            public string ItemValue { get; set; }
        }

        public static ComboBox AddSelectionX(Control parent, string namePrefix, string labelText, string selections, string value) {

            var items = selections.Split(",");
            var values = "";
            for (int i = 0; i < items.Length; i++) { 
                if (i> 0) 
                    values += ",";
                values += (i+"");     
            }
            return AddSelectionNameValue(parent, namePrefix, labelText, selections, values, value);
        }

        public static ComboBox AddSelectionNameValue(Control parent, string namePrefix, string labelText,
            List<object> list, string displayName, string valueName, string value) {


            Label label1 = new();
            label1.Name = namePrefix + "_label";
            label1.Parent = parent;
            label1.Text = labelText;
            label1.TextAlign = ContentAlignment.MiddleRight;
            label1.Width = 200;

            ComboBox comboBox1 = new ComboBox();
            comboBox1.Width = 300;
            comboBox1.Name = namePrefix;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Parent = parent;

            BindingSource source = new BindingSource();
            source.DataSource = list;
            comboBox1.DataSource = source.DataSource;
            comboBox1.DisplayMember = displayName;
            comboBox1.ValueMember = valueName;
            for (int i = 0; i < list.Count; i++)
            {
                if (value == ObjectUtils.GetPropertyValue(list[i],valueName) )
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
            }
            return comboBox1;
        }


        public static ComboBox AddSelectionNameValue(Control parent, string namePrefix, string labelText, string selections, string values, string value)
        {

            List<object> list = new List<object>();
            var items = selections.Split(",");
            var itemValues = values.Split(",");
            for (int i = 0; i < items.Length; i++) {
                
                var dict = new SelectionItem
                {
                    ItemName = items[i],
                    ItemValue = itemValues[i]
                };
                list.Add(dict);            
            }
            return AddSelectionNameValue(parent,namePrefix, labelText, list,"ItemName","ItemValue",value);

        }

    }
}
