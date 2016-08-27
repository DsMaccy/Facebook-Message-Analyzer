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
                if (StateMaster.isModuleActive(module.Value))
                {
                    checkedList.SetItemChecked(checkedList.Items.Count - 1, true);
                }
                Type moduleType = module.Value;

                IModule moduleObj = Activator.CreateInstance(moduleType) as IModule;
                string value = moduleObj.description();
                m_descriptions.Add(module.Key, value);
            }
        }

        private void SelectModulesForm_Resize(object sender, EventArgs e)
        {
            alignWidgets();
        }

        private void alignWidgets()
        {
            title.Left = this.ClientRectangle.Width / 2 - title.Width / 2;
            checkedList.Width = this.ClientRectangle.Width - 13*2;
            okButton.Left = this.ClientRectangle.Width - okButton.Width - cancelButton.Width - 2*13;
            cancelButton.Left = this.ClientRectangle.Width - cancelButton.Width - 13;

            okButton.Top = this.ClientRectangle.Height - okButton.Height - 9;
            cancelButton.Top = this.ClientRectangle.Height - cancelButton.Height - 9;
            checkedList.Height = this.ClientRectangle.Height - checkedList.Top - 9*2 - Math.Max(cancelButton.Height, okButton.Height);
        }

        private void checkedList_MouseHover(object sender, EventArgs e)
        {
            Console.WriteLine(e);
            this.moduleDescription.Tag = "Testing testing 1 2";
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, Type> moduleList = StateMaster.getModules();
            List<Type> selectedModules = new List<Type>();
            for (int i = 0; i < checkedList.CheckedItems.Count; i++)
            {
                selectedModules.Add(moduleList[checkedList.CheckedItems[i].ToString()]);
            }
            StateMaster.setActiveModules(selectedModules.ToArray());

            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
