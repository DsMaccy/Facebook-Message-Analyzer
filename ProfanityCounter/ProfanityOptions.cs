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
            profanity.Add("shit");
            profanity.Add("cunt");
            profanity.Add("bastard");
            profanity.Add("bitch");
            profanity.Add("ass");
            profanity.Add("nigger");
            profanity.Add("nigga");
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
            values.Add("saveLocation", saveLocationText.Text);
            values.Add("saveChecked", createFiles.Checked);


            return values;
        }
        public override void LoadValues(Dictionary<string, object> initialValues)
        {
            censorCheckBox.Checked = (bool)initialValues["innocent"];
            profanity = new List<string>(((string)initialValues["wordFlags"]).Split(';'));
            showWordsCheckbox.Checked = (bool)initialValues["showBreakdown"];
            saveLocationText.Text = (string)initialValues["saveLocation"];
            createFiles.Checked = (bool)initialValues["saveChecked"];
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

        private void createFiles_CheckedChanged(object sender, EventArgs e)
        {
            if (createFiles.Checked)
            {
                saveLocationBrowseButton.Enabled = true;
                saveLocationText.Enabled = true;
            }
            else
            {
                saveLocationBrowseButton.Enabled = false;
                saveLocationText.Enabled = false;
            }
        }

        private void saveLocationBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = saveLocationText.Text;
            fbd.ShowDialog();
            saveLocationText.Text = fbd.SelectedPath;
        }
    }
}
