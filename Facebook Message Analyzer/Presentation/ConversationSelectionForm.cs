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
    public partial class ConversationSelectionForm : Form
    {
        dynamic m_conversations;

        public ConversationSelectionForm()
        {
            InitializeComponent();
            m_conversations = Facebook_Message_Analyzer.Data.FBQueryManager.Manager.getConversations();
            conversations.Columns.Add("id", "Conversation ID");
            conversations.Columns.Add("users", "User List");
            conversations.Columns.Add("last message", "Date and Time of Last Message");
            if (m_conversations != null)
            {
                setColumns();
            }
        }

        private void setColumns()
        {
            conversations.Rows.Clear();

            for (int i = 0; i < m_conversations.Count; i++)
            {
                string userList = "";
                for (int j = 0; j < m_conversations[i].to.data.Count; j++)
                {
                    userList += m_conversations[i].to.data[j].name + ", ";
                }

                DateTime dtime = new DateTime();
                if (m_conversations[i].comments != null)
                {
                    dtime = DateTime.Parse(m_conversations[i].comments.data[0].created_time);
                    conversations.Rows.Add(m_conversations[i].id, userList, dtime);
                }
                else
                {
                    conversations.Rows.Add(m_conversations[i].id, userList, "");
                }
            }
        }

        private void next_Click(object sender, EventArgs e)
        {
            Facebook_Message_Analyzer.Data.FBQueryManager.Manager.nextConversations();
            dynamic conversations = Facebook_Message_Analyzer.Data.FBQueryManager.Manager.getConversations();

            if (conversations == null)
            {
                Business.ErrorMessages.APIOverflow();
            }
            else
            {
                m_conversations = conversations;
                setColumns();
            }
        }

        private void previous_Click(object sender, EventArgs e)
        {
            Facebook_Message_Analyzer.Data.FBQueryManager.Manager.prevConversations();
            dynamic conversations = Facebook_Message_Analyzer.Data.FBQueryManager.Manager.getConversations();
            
            if (conversations == null)
            {
                Business.ErrorMessages.APIOverflow();
            }
            else
            {
                m_conversations = conversations;
                setColumns();
            }
        }

        private void alignWidgets()
        {
            // Horizontal Sizing and Positioning
            next.Left = this.ClientSize.Width - next.Width - 13;
            conversations.Width = this.ClientSize.Width - 26;
            refresh.Left = this.ClientSize.Width / 2 - refresh.Width / 2;

            // Vertical Sizing and Positioning
            next.Top = this.ClientSize.Height - next.Height - 13;
            previous.Top = this.ClientSize.Height - previous.Height - 13;
            refresh.Top = this.ClientSize.Height - refresh.Height - 13;
            conversations.Top = menu.Height;
            conversations.Height = previous.Top - 26 - menu.Height;
        }

        protected override void OnResize(EventArgs e)
        {
            alignWidgets();
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            dynamic conversations = Facebook_Message_Analyzer.Data.FBQueryManager.Manager.getConversations();
            if (conversations == null)
            {
                Business.ErrorMessages.APIOverflow();
            }
            else
            {
                m_conversations = conversations;
            }
        }

        private void analyze_click(object sender, EventArgs e)
        {
            string value = (string)conversations.SelectedRows[0].Cells[0].Value;
            Business.StateMaster.runAnalysisModules(value);
        }

        private void moduleSelect_click(object sender, EventArgs e)
        {
            Business.StateMaster.selectAnalysisModules();
        }

        private void moduleOptions_Click(object sender, EventArgs e)
        {
            Business.StateMaster.showPreferences();
        }

        private void signOut_Click(object sender, EventArgs e)
        {
            Business.StateMaster.logout();
        }
    }
}
