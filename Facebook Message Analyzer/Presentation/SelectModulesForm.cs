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
using ModuleInterface;

namespace Facebook_Message_Analyzer.Presentation
{
    public partial class SelectModulesForm : Form
    {

        Dictionary<string, string> m_descriptions;

        public SelectModulesForm()
        {
            InitializeComponent();
            alignWidgets();


            // TODO: Programmatically add options for each of the modules
            Dictionary<string, Type> moduleList = StateMaster.getModules();
            m_descriptions = new Dictionary<string, string>();

            foreach (KeyValuePair<string, Type> module in moduleList)
            {
                checkedList.Items.Add(module.Key);
                m_descriptions.Add(module.Key, ((IModule)module.Value.GetConstructor(null)).description());
            }
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

        private void checkedList_MouseHover(object sender, EventArgs e)
        {
            Console.WriteLine(e);
        }
    }
}
