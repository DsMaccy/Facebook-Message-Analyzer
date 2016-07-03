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

namespace Facebook_Message_Analyzer
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();

            title.Text = "Facebook Message Analyzer";
            title.TextAlign = ContentAlignment.MiddleCenter;

            version.Text = Application.ProductVersion;
            version.TextAlign = ContentAlignment.MiddleCenter;
            
            login.ImageAlign = ContentAlignment.MiddleCenter;
            login.TextAlign = ContentAlignment.MiddleCenter;

            alignWidgets();
            
        }

        private void alignWidgets()
        {
            title.Left = this.ClientSize.Width / 2 - title.Width / 2;
            version.Left = this.ClientSize.Width / 2 - version.Width / 2;
            login.Left = this.ClientSize.Width / 2 - login.Width / 2;
            Console.WriteLine(login.Width + " " + this.ClientSize.Height);
            login.Top = this.ClientSize.Height - login.Height - 9;
        }

        protected override void OnResize(EventArgs e)
        {
            alignWidgets();
        }

        private void login_Click(object sender, EventArgs e)
        {
            StateMaster.Login();
        }
    }
}
