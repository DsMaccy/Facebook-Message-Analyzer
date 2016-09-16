using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModuleInterface;

namespace Facebook_Message_Analyzer.Business
{
    class ConversationIterator
    {
        private string m_conversationID;
        public ConversationIterator(string conversationID)
        {
            m_conversationID = conversationID;
        }
        public bool hasNext()
        {
            throw new NotImplementedException();
        }
        public FacebookMessage next()
        {
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
