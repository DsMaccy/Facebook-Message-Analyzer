using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Facebook_Message_Analyzer.Business;

namespace Facebook_Message_Analyzer.Presentation
{
    public partial class ModulePreferencesForm : Form
    {
        private UserControl m_openPreference;
        private Dictionary<string, string> m_cachedPreferenceData;
        public ModulePreferencesForm()
        {
            InitializeComponent();

            modules.Items.Add("General");
            
            // TODO: Add a label in modules for evey available module that has a preference
            m_openPreference = new GeneralPreferences();
            m_openPreference.Show();
            modules.SetSelected(0, true);
            this.Controls.Add(m_openPreference);
            m_cachedPreferenceData = StateMaster.getPreferenceData("General");
            foreach (KeyValuePair<string, string> control in m_cachedPreferenceData)
            {
                ((GeneralPreferences)m_openPreference).Controls["modulePath"].Text = m_cachedPreferenceData["modulePath"];
            }

            alignWidgets();
        }

        private void ModulePreferencesForm_Resize(object sender, EventArgs e)
        {
            alignWidgets();
        }

        private void alignWidgets()
        {
            modules.Height = this.ClientRectangle.Height - 18;
            m_openPreference.Height = this.ClientRectangle.Height - 18;
            m_openPreference.Top = modules.Top;
            m_openPreference.Left = modules.Left + modules.Width + 9;
            apply.Top = this.ClientRectangle.Height - 18 - apply.Height;
            ok.Top = this.ClientRectangle.Height - 18 - ok.Height;
            cancel.Top = this.ClientRectangle.Height - 18 - cancel.Height;
            apply.Left = this.ClientRectangle.Width - 9 - apply.Width;
            cancel.Left = apply.Left - 9 - cancel.Width;
            ok.Left = cancel.Left - 9 - ok.Width;
        }

        private void modules_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Controls.Remove(m_openPreference);

            // TODO: Set m_openPreference to the correct preference given the new selected index

            this.Controls.Add(m_openPreference);
        }

        private void apply_Click(object sender, EventArgs e)
        {
            if (modules.SelectedIndex == 0)
            {
                ((GeneralPreferences)(m_openPreference)).saveData();
            }
            else
            {

                // TODO -- save to DB and send call to module to save their stuff if they are not the general object
                // TODO: Fill -- this is one of the added IModule preferences
            }

        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            apply_Click(sender, e);
            this.Close();
        }
    }
}
