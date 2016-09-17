using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModuleInterface;
using Facebook_Message_Analyzer.Data;

namespace Facebook_Message_Analyzer.Business
{
    class ConversationIterator
    {
        private string m_conversationID;
        private int m_currentIndex;
        private bool m_queryOnline;
        private List<FacebookMessage> m_messageList;

        public ConversationIterator(string conversationID)
        {
            m_conversationID = conversationID;
            m_currentIndex = -1;
            m_queryOnline = false;
            m_messageList = new List<FacebookMessage>();
        }
        public bool hasNext()
        {
            if (m_currentIndex + 1 < m_messageList.Count)
            {
                return true;
            }
            else
            {
                m_messageList = FBQueryManager.Manager.getComments(m_conversationID);
                m_currentIndex = -1;

                /*
                if (m_queryOnline)
                {
                    // use Facebook to get next set of messages
                }
                else
                {
                    // Use CachedMessagesManager to get new messages
                }
                */
            }
            return false;
        }
        public FacebookMessage next()
        {
            return m_messageList[++m_currentIndex];
            throw new NotImplementedException();

            // If conversation in database
            //      Use database until all the data in DB is used up.
            //      If database has reached its end, query from FBQueryManager
            //  Else
            //      Use FBQueryManager class to query facebook servers directly
            // 
        }
    }
}
