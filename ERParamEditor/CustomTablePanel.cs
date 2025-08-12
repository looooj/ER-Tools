using ERParamUtils;
using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ERParamEditor.ControlUtils;

namespace ERParamEditor
{
    public delegate void ConvertTableItemValue(CustomTableItem item);

    public class CustomTableItem {
        public Control?  Control;
        public string Name="";
        public string Description="";
        public int Index = 0;
        public ConvertTableItemValue? convertTableItemValue = null;
    }

    public class CustomTablePanel
    {

        private TableLayoutPanel? mainPanel;
        private List<CustomTableItem> items = new();
        private CheckedListBox? currentCheckListBox=null;
        private int rowCount = 0;
        private int col2Width = 500;
        private int col1Width = 200;
        public CustomTablePanel() { 
        }

        public void Init(TableLayoutPanel panel) 
        { 
            mainPanel = panel;
            //mainPanel.RowStyles.Clear();
        }

        public TableLayoutPanel? GetPanel() {
            return mainPanel;
        }

 
        void IncRow() {
            if (mainPanel == null)
            {
                return;
            }
            var a = new RowStyle(SizeType.AutoSize);
            mainPanel.RowStyles.Add(a);
        }

        public void AddCheckBoxList(string name, string description) {

            if (mainPanel == null)
            {
                return;
            }
            if (currentCheckListBox == null) {
                IncRow();

                Label label1 = new();
                label1.Parent = mainPanel;
                label1.Width = col1Width;

                currentCheckListBox = new CheckedListBox();
                currentCheckListBox.Parent = mainPanel;
                currentCheckListBox.Width = col2Width;
                
            }

            currentCheckListBox.Items.Add(description);

            CustomTableItem item = new CustomTableItem();
            item.Control = currentCheckListBox;
            item.Name = name;
            item.Index = currentCheckListBox.Items.Count-1;
            items.Add(item);
            currentCheckListBox.Height = currentCheckListBox.Items.Count * 20;
        }
        public void AddCheckBox(string name, string description)
        {
            if (mainPanel == null)
            {
                return;
            }
            IncRow();
            if (description.Length < 1)
                description = name;
            Label label1 = new();
            label1.Parent = mainPanel;
            label1.Width = col1Width;

            CheckBox checkBox1 = new CheckBox();
            checkBox1.Name = name;
            checkBox1.Parent = mainPanel;

            checkBox1.Width = col2Width;
            checkBox1.Text = description;
            
            CustomTableItem item = new CustomTableItem();
            item.Control = checkBox1;
            item.Name = name;

            items.Add(item);
            rowCount++;
        }

        public void AddSpace(string text)
        {
            if (mainPanel == null)
            {
                return;
            }
            IncRow();

            Label label1 = new();
            label1.Parent = mainPanel;
            label1.Width = col1Width;

            Label label = new Label();
            label.Text = text;
            label.AutoSize = true;
            label.Parent = mainPanel;
            //mainPanel.SetColumnSpan(label, mainPanel.ColumnCount);
            rowCount++;
        }

        public void AddSelectionNameValue(string namePrefix, string labelText,
            List<object> list, string displayName, string valueName, string value)
        {

            if (mainPanel == null) {
                return;
            }
            IncRow();
            Control parent = mainPanel;

            Label label1 = new();
            label1.Name = namePrefix + "_label";
            label1.Width= col1Width;
            label1.Parent = parent;
            label1.Text = labelText;
            label1.TextAlign = ContentAlignment.MiddleRight;
            //label1.AutoSize = true;

            ComboBox comboBox1 = new ComboBox();
            comboBox1.Width = col2Width;
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
                if (value == ObjectUtils.GetPropertyValue(list[i], valueName))
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
            }

            CustomTableItem item = new CustomTableItem();
            item.Control = comboBox1;
            item.Name = namePrefix;

            items.Add(item);
            rowCount++;
        }

        public void AddSelectionNameValue(string namePrefix, string labelText, string selections, string values, string value)
        {

            List<object> list = new List<object>();
            var items = selections.Split(",");
            var itemValues = values.Split(",");
            for (int i = 0; i < items.Length; i++)
            {

                var dict = new SelectionItem
                {
                    ItemName = items[i],
                    ItemValue = itemValues[i]
                };
                list.Add(dict);
            }
            AddSelectionNameValue(namePrefix, labelText, list, "ItemName", "ItemValue", value);

        }

        //
        //
        //
        public void GetValues(DictConfig config) {

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item.Control == null)
                {
                    continue;
                }

                if (item.Control.GetType().Name == "ComboBox")
                {
                    ComboBox comboBox = item.Control as ComboBox;
                    config.SetString(item.Name, comboBox.SelectedValue.ToString());
                }
                if (item.Control.GetType().Name == "CheckBox")
                {
                    CheckBox checkBox = item.Control as CheckBox;
                    if (checkBox.Checked)
                        config.SetString(item.Name, "1");
                }
                if (item.Control.GetType().Name == "CheckedListBox")
                {
                    var listBox = item.Control as CheckedListBox;
                    if (listBox.GetItemChecked(item.Index))
                        config.SetString(item.Name, "1");
        
                }
            }
        }

        public void SetValues(DictConfig config) {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item.Control == null)
                {
                    continue;
                }
                if (!config.Contains(item.Name)) {
                    continue;
                }

                if (item.Control.GetType().Name == "ComboBox")
                {
                    ComboBox comboBox = item.Control as ComboBox;
                    comboBox.SelectedValue = config.GetString(item.Name,"");
                }
                
                if (item.Control.GetType().Name == "CheckBox")
                {
                    CheckBox checkBox = item.Control as CheckBox;
                    checkBox.Checked = true;
                }

                if (item.Control.GetType().Name == "CheckedListBox") {

                    CheckedListBox listBox = item.Control as CheckedListBox;
                    listBox.SetItemChecked(item.Index, true);
                }

            }
        }
    }
}
