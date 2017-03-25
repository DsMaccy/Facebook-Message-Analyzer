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
        private FacebookMessage m_latestSavedMessage;

        public ConversationIterator(string conversationID)
        {
            m_conversationID = conversationID;
            m_currentIndex = -1;

            m_queryOnline = true;
            m_messageList = new List<FacebookMessage>();
            m_messageList = CachedMessageManager.Manager.getMessages(conversationID);
            CachedMessageManager.Manager.addConversation(conversationID);

            m_latestSavedMessage = CachedMessageManager.Manager.getLastestEntry(conversationID);
        }
        ~ConversationIterator()
        {
            FBQueryManager.Manager.Cleanup();
        }

        // Special Considerations: Passing around a singleton object to multiple threads: inefficient architecture usage
        public bool hasNext()
        {
            if (m_currentIndex + 1 < m_messageList.Count)
            {
                return true;
            }
            else
            {
                if (m_queryOnline)
                {
                    m_messageList = FBQueryManager.Manager.getComments(m_conversationID);
                    string nextURL = FBQueryManager.Manager.getNextURL(m_conversationID);
                    // Works because m_messageList is in time-sorted order
                    if (m_messageList[0].id == m_latestSavedMessage.id)
                    {
                        m_queryOnline = false;
                    }
                    else
                    {

                        System.Threading.ParameterizedThreadStart paramThreadDelegate =
                            new System.Threading.ParameterizedThreadStart((object manager) =>
                            {
                                ((CachedMessageManager)manager).saveMessages(m_conversationID, m_messageList, nextURL);
                            }
                        );
                        System.Threading.Thread task = new System.Threading.Thread(paramThreadDelegate);
                        task.Start(CachedMessageManager.Manager);
                    }
                }
                else
                {
                    m_messageList = CachedMessageManager.Manager.getMessages(m_conversationID);
                    string nextURL = CachedMessageManager.Manager.getNextURL(m_conversationID);
                    FBQueryManager.Manager.setNextURL(nextURL);
                }
                m_currentIndex = -1;
                if (m_messageList.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public FacebookMessage next()
        {
            return m_messageList[++m_currentIndex];

            // If conversation in database
            //      Use database until all the data in DB is used up.
            //      If database has reached its end, query from FBQueryManager
            //  Else
            //      Use FBQueryManager class to query facebook servers directly
            // 
        }

    }
}
