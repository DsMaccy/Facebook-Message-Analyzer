using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ModuleInterface;

namespace GeneralInfoModule
{
    public partial class ResultForm : Form
    {
        public ResultForm(DataTable values)
        {
            InitializeComponent();
            alignWidgets();
            dataChart.DataSource = values;
        }

        private void alignWidgets()
        {
            dataChart.Width = this.ClientRectangle.Width - 26;
            dataChart.Height = this.ClientRectangle.Height - 13 - menuStrip1.Height - 9;
        }

        private void ResultForm_Resize(object sender, EventArgs e)
        {
            alignWidgets();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool successful = HelperMethods.saveFile(dataChart);
        }
    }
}
