using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Message_Analyzer.Data
{
    class FBQueryManager
    {
        public static FBQueryManager Manager = new FBQueryManager();

        private string m_token;

        private FBQueryManager()
        {
            m_token = "";
        }
        
        public void setToken(string token)
        {
            m_token = token;
        }
    }
}
