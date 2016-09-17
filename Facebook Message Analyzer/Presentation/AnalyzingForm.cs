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
    public partial class AnalyzingForm : Form
    {
        public AnalyzingForm()
        {
            InitializeComponent();
            alignWidgets();
        }

        private void AnalyzingForm_Resize(object sender, EventArgs e)
        {
            alignWidgets();
        }

        private void alignWidgets()
        {
            message.Left = this.ClientRectangle.Width / 2 - message.Width / 2;
            abort.Left = this.ClientRectangle.Width / 2 - abort.Width / 2;
        }

        private void abort_Click(object sender, EventArgs e)
        {
            StateMaster.abortAnalysis();
            this.Close();
        }
    }
}