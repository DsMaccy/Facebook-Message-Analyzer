using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModuleInterface;

namespace TestModule
{
    public partial class TestControl : PreferenceControl
    {
        public TestControl() : base()
        {
            InitializeComponent();
        }

        public override Dictionary<string, object> GetValues()
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("CheckBox", checkBox1.Checked);
            values.Add("CheckListBox", String.Join<int>(";", checkedListBox1.CheckedIndices.Cast<int>()));
            values.Add("ComboBox", comboBox1.Text);
            values.Add("DateTime", dateTimePicker1.Value);
            values.Add("Radio1", radioButton1.Checked);
            values.Add("Radio2", radioButton2.Checked);
            values.Add("Radio3", radioButton3.Checked);
            values.Add("Number", numericUpDown1.Value);
            values.Add("TextBox", textBox1.Text);

            return values;
        }
        public override void LoadValues(Dictionary<string, object> values)
        {
            foreach (KeyValuePair<string, object> kv in values)
            {
                object value = kv.Value;
                switch(kv.Key)
                {
                    case "CheckBox":
                        checkBox1.Checked = (bool)value;
                        break;
                    case "CheckListBox":
                        string[] selectedFields = ((string)value).Split(';');
                        for (int index = 0; index < selectedFields.Length; index++)
                        {
                            if (selectedFields[index] != "")
                            {
                                checkedListBox1.SetItemChecked(int.Parse(selectedFields[index]), true);
                            }
                        }
                        break;
                    case "ComboBox":
                        comboBox1.Text = (string)value;
                        break;
                    case "DateTime":
                        dateTimePicker1.Value = (DateTime)value;
                        break;
                    case "Radio1":
                        radioButton1.Checked = (bool)value;
                        break;
                    case "Radio2":
                        radioButton2.Checked = (bool)value;
                        break;
                    case "Radio3":
                        radioButton3.Checked = (bool)value;
                        break;
                    case "Number":
                        numericUpDown1.Value = (decimal)value;
                        break;
                    case "TextBox":
                        textBox1.Text = (string)value;
                        break;
                }
            }
        }
    }
}
