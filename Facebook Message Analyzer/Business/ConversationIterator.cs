using System;
using System.Collections.Generic;
using System.Collections;
using ModuleInterface;
using Facebook_Message_Analyzer.Data;

namespace Facebook_Message_Analyzer.Business
{
    class ConversationIterator : IEnumerable<FacebookMessage>
    {
        private class ConversationIteratorHelper : IEnumerator<FacebookMessage>
        {
            
            private enum IteratorState
            {
                OnlineQuery,
                CacheQuery,
                RemnantQuery,
            }
            private string m_conversationID;
            private IteratorState m_state;
            private int m_currentIndex;
            private int m_saveCount;
            private int m_cacheIndex;

            private List<FacebookMessage> m_messageList;
            private List<FacebookMessage> m_cachedMessageList;
            private Dictionary<int, string> m_queryLinks;
            private bool m_saveMessages;

            public ConversationIteratorHelper(string conversationID, bool saveMessages)
            {
                constructorHelper(conversationID, saveMessages);
            }
            private void constructorHelper(string conversationID, bool saveMessages)
            {
                m_conversationID = conversationID;
                m_saveCount = 0;
                m_saveMessages = saveMessages;

                CachedMessageManager.Manager.addConversation(conversationID);

                m_cachedMessageList = CachedMessageManager.Manager.getMessages(conversationID);
                getNextOnlineMessages();
                m_queryLinks = CachedMessageManager.Manager.getNextURLs(conversationID);
                m_currentIndex = m_messageList.Count;
                m_cacheIndex = m_cachedMessageList.Count - 1;

                m_state = IteratorState.OnlineQuery;
            }

            #region Private Helper Methods

            private void saveMessages(List<FacebookMessage> messagesToSave)
            {
                if (!m_saveMessages || messagesToSave.Count == 0)
                {
                    return;
                }
                
                System.Threading.ParameterizedThreadStart paramThreadDelegate =
                    new System.Threading.ParameterizedThreadStart((object customObj) =>
                    {
                        Dictionary<string, object> paramObj = customObj as Dictionary<string, object>;

                        CachedMessageManager manager = paramObj["manager"] as CachedMessageManager;
                        List<FacebookMessage> messages = paramObj["messages"] as List<FacebookMessage>;
                        int index = (int)(paramObj["saveIndex"]);
                        string url = paramObj["url"] as string;
                        try
                        {
                            manager.saveMessages(m_conversationID, messages, url, index);
                        }
                        catch (System.Data.ConstraintException)
                        {
                            // TODO: Consider reacquiring cached messages
                        }
                    }
                );
                Dictionary<string, object> delegateParams = new Dictionary<string, object>();
                delegateParams["manager"] = CachedMessageManager.Manager;
                delegateParams["messages"] = messagesToSave;
                delegateParams["saveIndex"] = m_saveCount;
                delegateParams["url"] = FBQueryManager.Manager.getNextURL(m_conversationID);

                System.Threading.Thread task = new System.Threading.Thread(paramThreadDelegate);
                task.Start(delegateParams);
            }
            private void getNextOnlineMessages()
            {
                m_messageList = FBQueryManager.Manager.getComments(m_conversationID);
            }

            // TODO: Ensure that the state methods work
            private bool moveOnOnlineState()
            {
                m_currentIndex -= 1;
                if (m_currentIndex < 0)
                {
                    saveMessages(new List<FacebookMessage>(m_messageList));
                    m_saveCount += m_messageList.Count;
                    getNextOnlineMessages();
                    m_currentIndex = m_messageList.Count - 1;
                }
                else if (m_cachedMessageList.Count > 0 && m_messageList[m_currentIndex] == m_cachedMessageList[m_cacheIndex])
                {
                    if (m_currentIndex != m_messageList.Count - 1)
                    {
                        m_messageList.RemoveRange(0, m_currentIndex + 1);
                        saveMessages(new List<FacebookMessage>(m_messageList));
                    }
                    m_state = IteratorState.CacheQuery;
                    m_messageList = m_cachedMessageList;
                    m_currentIndex = m_cacheIndex;
                }

                if (m_messageList == null || m_messageList.Count == 0)
                {
                    return false;
                }
                return true;
            }

            private bool moveOnCacheState()
            {
                m_currentIndex--;
                m_cacheIndex--;
                if (m_currentIndex < 0)
                {
                    // This should only occur if the cached messages contain all messages up to the initial messsage
                    return false;
                }
                if (m_queryLinks.ContainsKey(m_messageList[m_currentIndex].id))
                {
                    lock (this)
                    {
                        m_queryLinks = CachedMessageManager.Manager.getNextURLs(m_conversationID);
                        if (m_queryLinks.ContainsKey(m_messageList[m_currentIndex].id))
                        {
                            m_state = IteratorState.RemnantQuery;
                            FBQueryManager.Manager.setNextURL(m_conversationID, m_queryLinks[m_messageList[m_currentIndex].id]);
                            // TODO: Check transition correctness from using cached messages vs. online facebook servers
                            m_cacheIndex = m_currentIndex;
                            m_messageList = new List<FacebookMessage>();
                            m_messageList.Add(m_cachedMessageList[m_currentIndex]);
                            m_currentIndex = 0;
                        }
                        else
                        {
                            FacebookMessage message = m_cachedMessageList[m_cacheIndex];
                            m_messageList = m_cachedMessageList = CachedMessageManager.Manager.getMessages(m_conversationID);
                            m_currentIndex =  m_cacheIndex = m_cachedMessageList.BinarySearch(message);
                            
                        }
                    }
                }
                return true;
            }

            // TODO: Consider removing this state...
            private bool moveOnRemnantState()
            {
                m_currentIndex -= 1;
                if (m_currentIndex < 0)
                {
                    getNextOnlineMessages();
                    m_currentIndex = m_messageList.Count - 1;
                    m_state = IteratorState.OnlineQuery;
                    return m_messageList.Count > 0;
                }
                return true;
            }

            #endregion

            #region IEnumerator Interface Methods

            // Special Considerations: 
            //      Passing around singleton obj to multiple threads inefficient usage of arch...?
            bool IEnumerator.MoveNext()
            {
                switch (m_state)
                {
                    case IteratorState.OnlineQuery:
                        return moveOnOnlineState();
                    case IteratorState.CacheQuery:
                        return moveOnCacheState();
                    case IteratorState.RemnantQuery:
                        return moveOnRemnantState();
                    default:
                        return false;
                }
            }
            FacebookMessage IEnumerator<FacebookMessage>.Current
            {
                get
                {
                    return m_messageList[m_currentIndex];
                }
            }
            object IEnumerator.Current
            {
                get
                {
                    return m_messageList[m_currentIndex];
                }
            }
            void IEnumerator.Reset()
            {
                ((IDisposable)this).Dispose();
                constructorHelper(m_conversationID, m_saveMessages);
            }

            #endregion

            #region IDispose Interface Methods
            void IDisposable.Dispose()
            {
                FBQueryManager.Manager.Cleanup();
            }
            #endregion
        }

        #region Constructor, Destructor and Instance Variables

        private ConversationIteratorHelper m_iterator;

        public ConversationIterator(string conversationID, bool saveMessages)
        {
            m_iterator = new ConversationIteratorHelper(conversationID, saveMessages);
        }

        ~ConversationIterator()
        {
            ((IDisposable)m_iterator).Dispose();
        }

        #endregion

        #region IEnumerable Interface Implementation

        IEnumerator<FacebookMessage> IEnumerable<FacebookMessage>.GetEnumerator()
        {
            return m_iterator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_iterator as IEnumerator;
        }
        #endregion

        
    }
}