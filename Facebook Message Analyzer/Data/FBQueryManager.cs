using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using Facebook_Message_Analyzer.Business;

namespace Facebook_Message_Analyzer.Data
{
    /* Notes:
     * Possible Addresses:
     * me
     * me/inbox -- must request permission
     * me/friends -- must request permission
     * me/feed
     */
    class FBQueryManager
    {
        public static FBQueryManager Manager = new FBQueryManager();

        private const string m_APP_ID = "1465357507096514";
        private const string m_APP_SECRET = "ac5817c4f2dd07bf18137d7297d4015c";
        private const string m_SUCCESS_URI = "https://www.facebook.com/connect/login_success.html";

        private Facebook.FacebookClient m_fbClient;
        private string m_token;
        private dynamic m_userInfo;
        private dynamic m_conversations;

        private FBQueryManager()
        {
            m_token = "";
            m_fbClient = null;
            m_userInfo = null;
            m_conversations = null;
        }

        public Uri getLoginURL()
        {
            dynamic parameters = new System.Dynamic.ExpandoObject();

            parameters.client_id = m_APP_ID;
            parameters.redirect_uri = m_SUCCESS_URI;

            // The requested response: an access token (token), an authorization code (code), or both (code token).
            parameters.response_type = "token";

            // list of additional display modes can be found at http://developers.facebook.com/docs/reference/dialogs/#display
            parameters.display = "popup";

            // add the 'scope' parameter only if we have extendedPermissions.
            string extendedPermissions = "read_mailbox";
            parameters.scope = extendedPermissions;

            // generate the login url

            dynamic token = null;
            Facebook.FacebookClient FBClient = new Facebook.FacebookClient();
            try
            {
                token = FBClient.Get("oauth/access_token", new
                {
                    client_id = m_APP_ID, // "1465357507096514"
                    client_secret = m_APP_SECRET, // "ac5817c4f2dd07bf18137d7297d4015c"
                    grant_type = "client_credentials"
                });
            }
            catch (WebExceptionWrapper)
            {
                ErrorMessages.WebConnectionFailure();
                return null;
            }
            FBClient.AccessToken = token.access_token;
            FBClient.AppId = m_APP_ID;
            FBClient.AppSecret = m_APP_SECRET;

            return FBClient.GetLoginUrl(parameters);
        }
        
        public void setToken(string token)
        {
            m_token = token;
            m_fbClient = new Facebook.FacebookClient(m_token);

            m_userInfo = m_fbClient.Get("me");
            getConversations();
        }

        public dynamic getConversations()
        {
            if (m_conversations == null)
            {
                m_conversations = m_fbClient.Get("me/inbox", new
                {
                    limit = 200,
                    offset = 50
                });
            }
            else
            {
                m_conversations = m_fbClient.Get("me/inbox");
            }

            return (m_conversations.data);
        }

        public void nextConversations()
        {
            m_conversations = m_fbClient.Get(m_conversations.paging.next);
        }

        public void prevConversations()
        {
            m_conversations = m_fbClient.Get(m_conversations.paging.previous);
        }

        public void setConversation()
        {

        }
    }
}
