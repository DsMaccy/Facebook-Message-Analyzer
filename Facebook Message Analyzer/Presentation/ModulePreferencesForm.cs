using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Facebook_Message_Analyzer.Business;
using ModuleInterface;
using System.Threading;

namespace Facebook_Message_Analyzer.Presentation
{
    public partial class ModulePreferencesForm : Form
    {

        #region Constructor + Instance Variables

        private PreferenceControl m_openPreference;
        private Dictionary<string, Dictionary<string, object>> m_cachedPreferenceData;
        private Dictionary<string, Dictionary<string, object>> m_currentPreferenceData;
        private bool m_dirty;
        private SortedSet<string> m_dirtyModules;
        private Thread m_daemonThread;
        private string m_currentModule;
        private bool m_validClosing;

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
                    m_dirtyModules = new SortedSet<string>();
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

            Dictionary<string, Type> moduleTypes = StateMaster.getModules();
            m_dirtyModules = new SortedSet<string>();
            m_cachedPreferenceData = new Dictionary<string, Dictionary<string, object>>();
            m_currentPreferenceData = new Dictionary<string, Dictionary<string, object>>();
            m_validClosing = false;

            modules.Items.Add("General");
            m_cachedPreferenceData["General"] = StateMaster.getPreferenceData("General");
            m_currentPreferenceData["General"] = m_cachedPreferenceData["General"];
            int index = 1;
            foreach (KeyValuePair<string, Type> kv in moduleTypes)
            {
                IModule moduleInstance = Activator.CreateInstance(kv.Value) as IModule;
                if (moduleInstance.preferencesAvailable())
                {
                    modules.Items.Add(kv.Key);
                    m_cachedPreferenceData[kv.Key] = StateMaster.getPreferenceData(kv.Key);
                    m_currentPreferenceData[kv.Key] = m_cachedPreferenceData[kv.Key];
                    index++;
                }
            }
            Dirty = false;
            modules.SetSelected(0, true);

            m_currentModule = "General";
            m_daemonThread = new Thread(new ThreadStart(updateDaemon));
            m_daemonThread.Start();
        }

        #endregion

        #region UI Adjustment Methods

        private void ModulePreferencesForm_Resize(object sender, EventArgs e)
        {
            alignWidgets();
        }

        private void alignWidgets()
        {
            
            modules.Height = this.ClientRectangle.Height - 18;

            apply.Top = this.ClientRectangle.Height - 18 - apply.Height;
            ok.Top = this.ClientRectangle.Height - 18 - ok.Height;
            cancel.Top = this.ClientRectangle.Height - 18 - cancel.Height;
            apply.Left = this.ClientRectangle.Width - 9 - apply.Width;
            cancel.Left = apply.Left - 9 - cancel.Width;
            ok.Left = cancel.Left - 9 - ok.Width;

            m_openPreference.Top = modules.Top;
            m_openPreference.Height = Math.Max(apply.Top, Math.Max(ok.Top, cancel.Top)) - m_openPreference.Top - 18;
            m_openPreference.Left = modules.Left + modules.Width + 9;
            m_openPreference.Width = apply.Right - m_openPreference.Left;

            MinimumSize = new System.Drawing.Size(modules.Width + ok.Width + cancel.Width + apply.Width + 9 * 7, 18 * 4 + Math.Max(apply.Height, Math.Max(ok.Height, cancel.Height)));
        }

        private void setPreferenceControl(int index)
        {
            if (m_openPreference != null)
            {
                m_openPreference.Hide();
                this.Controls.Remove(m_openPreference);
            }

            string tag = modules.Items[index] as string;
            m_currentModule = tag;
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

            m_openPreference.LoadValues(m_currentPreferenceData[tag]);
            this.Controls.Add(m_openPreference);
            m_openPreference.Show();
            alignWidgets();            
        }

        void updateDaemon()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(10);
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(() =>
                    {
                        m_currentPreferenceData[m_currentModule] = m_openPreference.GetValues();
                    }));
                }
                else { m_currentPreferenceData[m_currentModule] = m_openPreference.GetValues(); }
                
                
                string currentModule = m_currentModule;
                Dictionary<string, object> cachedValues = m_cachedPreferenceData[currentModule];
                Dictionary<string, object> currentValues = m_currentPreferenceData[currentModule];
                bool thisIsDirty = false;
                foreach(KeyValuePair<string, object> kvPair in currentValues)
                {
                    if (!currentValues[kvPair.Key].Equals(cachedValues[kvPair.Key]))
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                Dirty = true;
                            }));
                        }
                        else { Dirty = true; }
                        m_dirtyModules.Add(m_currentModule);
                        thisIsDirty = true;
                        break;
                    }
                }
                if (thisIsDirty)
                {
                    m_dirtyModules.Add(m_currentModule);
                }
                else
                {
                    m_dirtyModules.Remove(m_currentModule);
                    if (m_dirtyModules.Count == 0)
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                Dirty = false;
                            }));
                        }
                        else { Dirty = false; }
                    }
                }
            }
        }

        #endregion

        #region Events

        private void modules_SelectedIndexChanged(object sender, EventArgs e)
        {
            setPreferenceControl(((ListBox)sender).SelectedIndex);
        }

        private void apply_Click(object sender, EventArgs e)
        {
            foreach (string moduleTag in m_dirtyModules)
            {
                StateMaster.savePreferenceData(moduleTag, m_currentPreferenceData[moduleTag]);
                m_cachedPreferenceData[moduleTag] = m_currentPreferenceData[moduleTag];
            }
            
            Dirty = false;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            m_validClosing = true;
            this.Close();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (Dirty)
            {
                apply_Click(sender, e);
            }
            m_validClosing = true;
            this.Close();
        }

        private void ModulePreferencesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_daemonThread.Abort();
            if (!m_validClosing && Dirty)
            {
                DialogResult dr = MessageBox.Show("There is unsaved data.  Do you still want to exit?", "", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        #endregion
    }
}