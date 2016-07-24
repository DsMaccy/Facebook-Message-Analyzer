using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Message_Analyzer.Presentation
{
    public partial class ModulePreferencesForm : Form
    {
        public ModulePreferencesForm()
        {
            InitializeComponent();
            alignWidgets();
        }

        private void ModulePreferencesForm_Resize(object sender, EventArgs e)
        {
            alignWidgets();
        }

        private void alignWidgets()
        {
            modules.Height = this.ClientRectangle.Height - 18;
        }
    }
}
