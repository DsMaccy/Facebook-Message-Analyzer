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
        private Dictionary<string, IModule> m_modules;
        private int m_hoverIndex;

        public SelectModulesForm()
        {
            InitializeComponent();
            alignWidgets();
            m_hoverIndex = -1;


            // TODO: Programmatically add options for each of the modules
            Dictionary<string, Type> moduleList = StateMaster.getModules();
            m_modules = new Dictionary<string, IModule>();

            
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
                m_modules.Add(module.Key, moduleObj);
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

        private void checkedList_MouseMove(object sender, MouseEventArgs e)
        {
            int newIndex = checkedList.IndexFromPoint(e.Location);
            if (m_hoverIndex != newIndex)
            {
                m_hoverIndex = newIndex;
                if (m_hoverIndex > -1)
                {
                    hoverText.Show(m_modules[(string)(checkedList.Items[m_hoverIndex])].description(), checkedList);
                }
                else
                {
                    hoverText.Hide(checkedList);
                }
            }
        }
    }
}
