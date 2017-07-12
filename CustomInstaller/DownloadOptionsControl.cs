using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomInstaller
{
    public struct DownloadOptionsParams
    {
        public bool createDesktopShortcut;
        public bool createStartMenuShortcut;
        public string programLocation;
        public string dataLocation;
    }

    public partial class DownloadOptionsControl : UserControl
    {
        public DownloadOptionsControl(DownloadOptionsParams initialValues)
        {
            InitializeComponent();

            DesktopShortcutCheckBox.Checked = initialValues.createDesktopShortcut;
            StartMenuCheckBox.Checked = initialValues.createStartMenuShortcut;
            ProgramLocationTextBox.Text = initialValues.programLocation;
            DataLocationTextBox.Text = initialValues.programLocation;
        }

        private void browseSave_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            ProgramLocationTextBox.Text = fbd.SelectedPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            DataLocationTextBox.Text = fbd.SelectedPath;
        }

        private void ProgLocBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            ProgramLocationTextBox.Text = fbd.SelectedPath;
        }
        
        private void DataLocBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            DataLocationTextBox.Text = fbd.SelectedPath;
        }

        public DownloadOptionsParams getParams()
        {
            DownloadOptionsParams rVal = new DownloadOptionsParams();
            rVal.createDesktopShortcut = DesktopShortcutCheckBox.Checked;
            rVal.createStartMenuShortcut = StartMenuCheckBox.Checked;
            rVal.programLocation = ProgramLocationTextBox.Text;
            rVal.programLocation = DataLocationTextBox.Text;
            return rVal;
        }
    }
}
