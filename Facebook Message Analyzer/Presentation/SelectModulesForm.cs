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
    public partial class SelectModulesForm : Form
    {
        public SelectModulesForm()
        {
            InitializeComponent();
            alignWidgets();
            
            // TODO: Programmatically add options for each of the modules
        }

        private void SelectModulesForm_Resize(object sender, EventArgs e)
        {
            alignWidgets();
        }

        private void alignWidgets()
        {
            title.Left = this.ClientRectangle.Width / 2 - title.Width / 2;
            checkedList.Width = this.ClientRectangle.Width - 26;
            checkedList.Height = this.ClientRectangle.Height - title.Height - 27;
        }
    }
}
