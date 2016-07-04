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
        }

        private void next_Click(object sender, EventArgs e)
        {
            Facebook_Message_Analyzer.Data.FBQueryManager.Manager.nextConversations();
            m_conversations = Facebook_Message_Analyzer.Data.FBQueryManager.Manager.getConversations();
        }

        private void previous_Click(object sender, EventArgs e)
        {
            Facebook_Message_Analyzer.Data.FBQueryManager.Manager.prevConversations();
            m_conversations = Facebook_Message_Analyzer.Data.FBQueryManager.Manager.getConversations();
        }
    }
}
