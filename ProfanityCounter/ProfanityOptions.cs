using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModuleInterface;

namespace ProfanityCounter
{
    public partial class ProfanityOptions : PreferenceControl
    {
        private List<string> profanity;

        public ProfanityOptions()
        {
            InitializeComponent();
            profanity = new List<string>();
            profanity.Add("fuck");
            profanity.Add("fucks");
            profanity.Add("fucked");
            profanity.Add("fucking");
            profanity.Add("fucky");
            profanity.Add("shit");
            profanity.Add("shits");
            profanity.Add("shitting");
            profanity.Add("shitty");
            profanity.Add("cunt");
            profanity.Add("bastard");
            profanity.Add("bastardly");
            profanity.Add("bitch");
            profanity.Add("bitchy");
            profanity.Add("bitching");
            profanity.Add("bitchin");
            profanity.Add("bitchin'");
            profanity.Add("asshole");
            profanity.Add("assholes");
            profanity.Add("ass");
            profanity.Add("asses");
            profanity.Add("nigger");
            profanity.Add("niggers");
            profanity.Add("nigga");
            profanity.Add("niggas");
            propogateItemList();
        }

        #region Helper Methods
        private void propogateItemList()
        {
            profanityListBox.Items.Clear();

            foreach (string item in profanity)
            {
                if (censorCheckBox.Checked)
                {
                    string newItem = item;
                    newItem = newItem.Replace('a', '*');
                    newItem = newItem.Replace('e', '*');
                    newItem = newItem.Replace('i', '*');
                    newItem = newItem.Replace('o', '*');
                    newItem = newItem.Replace('u', '*');
                    profanityListBox.Items.Add(newItem);
                }
                else
                {
                    profanityListBox.Items.Add(item);
                }
            }
        }
        #endregion

        #region PreferenceControl Methods
        public override Dictionary<string, object> GetValues()
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("innocent", censorCheckBox.Checked);
            values.Add("wordFlags", String.Join(";", profanity));
            values.Add("showBreakdown", showWordsCheckbox.Checked);
            return values;
        }
        public override void LoadValues(Dictionary<string, object> initialValues)
        {
            censorCheckBox.Checked = (bool)initialValues["innocent"];
            profanity = new List<string>(((string)initialValues["wordFlags"]).Split(';'));
            showWordsCheckbox.Checked = (bool)initialValues["showBreakdown"];
        }
        #endregion 

        #region Events
        private void censorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            propogateItemList();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string[] values = profanityAdderTextBox.Text.ToLower().Split(';', ' ', '\n', '\t');
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != "" && !profanity.Contains(values[i]))
                {
                    profanity.Add(values[i]);
                }
            }

            propogateItemList();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (profanityListBox.SelectedIndex >= 0)
            {
                profanity.RemoveAt(profanityListBox.SelectedIndex);
                profanityListBox.SelectedIndex = -1;
                propogateItemList();
            }
        }
        #endregion
    }
}
