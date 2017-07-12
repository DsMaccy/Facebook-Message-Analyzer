using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomInstaller
{
    public partial class InstallPrompt : Form
    {
        private enum InstallerScreenState
        {
            DownloadOptions = 1,
            ModuleOptions = 2
        }
        private UserControl m_preferenceControl;
        private DownloadOptionsParams m_doParams;
        private ModuleDownloadParams m_mdParams;
        private InstallerScreenState state;
        private InstallerScreenState m_state
        {
            get
            {
                return state;
            }
            set
            {
                savePreviousState();
                state = value;

                changeState();
            }
        }
        public InstallPrompt()
        {
            InitializeDefaultParams();

            InitializeComponent();
            m_state = InstallerScreenState.DownloadOptions;
            m_preferenceControl.Location = new Point(12, 12);
            m_preferenceControl.Visible = true;
        }
        private void InitializeDefaultParams()
        {
            m_doParams = new DownloadOptionsParams();
            m_doParams.createDesktopShortcut = true;
            m_doParams.createStartMenuShortcut = true;
            m_doParams.programLocation = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Facebook Message Analyzer\\";
            m_doParams.dataLocation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Solace Inc.\\Facebook Message Analyzer\\";

            m_mdParams = new ModuleDownloadParams();
            // TODO: May need to change the md param instance variable
            m_mdParams.selectedModules = new List<Type>();
        }

        private void changeState()
        {
            switch (m_state)
            {
                case InstallerScreenState.DownloadOptions:
                    nextButton.Text = "Next";
                    backButton.Visible = false;
                    m_preferenceControl = new DownloadOptionsControl(m_doParams);
                    break;
                case InstallerScreenState.ModuleOptions:
                    nextButton.Text = "Finish";
                    backButton.Visible = true;
                    m_preferenceControl = new ModuleDownloadControl(m_mdParams);
                    break;
            }
        }
        private void savePreviousState()
        {
            switch (m_state)
            {
                case InstallerScreenState.DownloadOptions:
                    m_doParams = ((DownloadOptionsControl)m_preferenceControl).getParams();
                    break;
                case InstallerScreenState.ModuleOptions:
                    m_mdParams = ((ModuleDownloadControl)m_preferenceControl).getParams();
                    break;
            }
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            switch(m_state)
            {
                case InstallerScreenState.DownloadOptions:
                    m_state = InstallerScreenState.ModuleOptions;
                    break;
                case InstallerScreenState.ModuleOptions:
                    Program.setState(m_doParams);
                    Program.setState(m_mdParams);
                    this.Close();
                    break;
            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            m_state = InstallerScreenState.DownloadOptions;
        }

    }
}
