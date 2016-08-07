using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Message_Analyzer.Presentation
{
    public partial class GeneralPreferences : UserControl
    {
        public GeneralPreferences()
        {
            InitializeComponent();
        }

        private void browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.ShowDialog();
            Console.WriteLine(FBD.SelectedPath);
            if (FBD.SelectedPath != null && FBD.SelectedPath != "")
            {
                modulePath.Text = FBD.SelectedPath;
            }
        }
    }
}
