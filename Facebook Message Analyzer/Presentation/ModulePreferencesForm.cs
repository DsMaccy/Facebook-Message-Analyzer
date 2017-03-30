using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Facebook_Message_Analyzer.Business;
using ModuleInterface;

namespace Facebook_Message_Analyzer.Presentation
{
    public partial class ModulePreferencesForm : Form
    {
        private UserControl m_openPreference;
        private Dictionary<string, Dictionary<string, string>> m_cachedPreferenceData;
        private Dictionary<string, Dictionary<string, string>> m_currentPreferenceData;
        private bool m_dirty;
        private bool Dirty
        {
            get
            {
                return m_dirty;
            }
            set
            {
                m_dirty = value;
                if (!m_dirty)
                {
                    apply.Enabled = false;
                }
                else
                {
                    apply.Enabled = true;
                }
            }

        }

        public ModulePreferencesForm()
        {
            InitializeComponent();     
            modules.Items.Add("General");
            int index = 1;
            Dictionary<string, Type> moduleTypes = StateMaster.getModules();
            foreach (KeyValuePair<string, Type> kv in moduleTypes)
            {
                IModule moduleInstance = Activator.CreateInstance(kv.Value) as IModule;
                if (moduleInstance.preferencesAvailable())
                {
                    modules.Items.Add(kv.Key);
                    index++;
                }
            }
            Dirty = false;

            
            m_cachedPreferenceData = new Dictionary<string, Dictionary<string, string>>();
            m_currentPreferenceData = new Dictionary<string, Dictionary<string, string>>();

            modules.SetSelected(0, true);
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

        private void setPreferenceControl(int index)
        {
            // TODO: Add a warning about unsaved changes
            if (m_openPreference != null)
            {
                m_openPreference.Hide();
                this.Controls.Remove(m_openPreference);
            }

            string tag = modules.Items[index] as string;
            Dictionary<string, string> loadedData;
            if (tag == "General")
            {
                m_openPreference = new GeneralPreferences();
            }
            else
            {
                Type moduleType = (StateMaster.getModules()[tag]);
                IModule module = Activator.CreateInstance(moduleType) as IModule;

                m_openPreference = module.getPreferenceControl();
            }


            if (!m_cachedPreferenceData.ContainsKey(tag))
            {
                m_cachedPreferenceData[tag] = StateMaster.getPreferenceData(tag);
            }

            loadedData = m_cachedPreferenceData[tag];
            if (m_currentPreferenceData.ContainsKey(tag))
            {
                loadedData = m_currentPreferenceData[tag];
            }

            this.Controls.Add(m_openPreference);
            m_openPreference.Show();
            alignWidgets();

            foreach (KeyValuePair<string, string> control in loadedData)
            {
                Control widget = m_openPreference.Controls[control.Key];
                if (widget is CheckBox)
                {
                    ((CheckBox)widget).Checked = control.Value.ToLower() == "true";
                    ((CheckBox)widget).CheckedChanged += new EventHandler(subControlModified);
                }
                else if (widget != null)
                {
                    widget.Text = control.Value;
                    widget.TextChanged += new EventHandler(subControlModified);
                }
            }
        }

        #region Events

        private void modules_SelectedIndexChanged(object sender, EventArgs e)
        {
            setPreferenceControl(((ListBox)sender).SelectedIndex);
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
            Dirty = false;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (Dirty)
            {
                apply_Click(sender, e);
            }
            this.Close();
        }

        private void subControlModified(object sender, EventArgs e)
        {
            Dirty = true;
        }

        #endregion
    }
}