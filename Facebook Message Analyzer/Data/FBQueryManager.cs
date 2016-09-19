using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using Facebook_Message_Analyzer.Business;
using ModuleInterface;

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
        public const string LOGOUT_URL = "https://www.facebook.com/?stype=lo&jlou=AfdXxs9jrmtwW_Wd7wAlGx219TRXOaF_FjDGvDM94bZnYWPQl-fh_HQTglLiJ7JtKvLGk8Ndck5IUCogRJzvY74K&smuh=5490&lh=Ac8t2PonlypJDoHD";

        private const string m_APP_ID = "1465357507096514";
        private const string m_APP_SECRET = "ac5817c4f2dd07bf18137d7297d4015c";
        private const string m_SUCCESS_URI = "https://www.facebook.com/connect/login_success.html";

        private Facebook.FacebookClient m_fbClient;
        private string m_token;
        private dynamic m_userInfo;
        private dynamic m_conversations;
        private dynamic m_messages;
        private string m_next;

        private FBQueryManager()
        {
            m_token = "";
            m_fbClient = null;
            m_userInfo = null;
            m_conversations = null;
            m_messages = null;
            m_next = "";
        }

        public void Cleanup()
        {
            m_next = null;
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

        public Uri getLogoutURL()
        {
            // TODO: There is a bug with the logout
            dynamic parameters = new System.Dynamic.ExpandoObject();

            parameters.access_token = m_token;
            parameters.next = FBQueryManager.LOGOUT_URL;

            Facebook.FacebookClient FBClient = new Facebook.FacebookClient();
            FBClient.AccessToken = m_token;
            FBClient.AppId = m_APP_ID;
            FBClient.AppSecret = m_APP_SECRET;

            // string redirectUrl = "";
            // string accessToken = "";
            // new Uri("https://www.facebook.com/logout.php?next=" + redirectUrl + "&access_token=" + accessToken)

            return FBClient.GetLogoutUrl(parameters);
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
            try
            {
                if (m_conversations == null)
                {
                    m_conversations = m_fbClient.Get("me/inbox", new
                    {
                        limit = 200,
                        offset = 50
                    });
                }
                /*
                else
                {
                    m_conversations = m_fbClient.Get("me/inbox");
                }*/

                return (m_conversations.data);
            }
            catch (Facebook.FacebookOAuthException)
            {
                Console.Error.WriteLine("Api calls exceeded.  You must wait");
                return null;
            }
        }

        public void nextConversations()
        {
            try
            {
                dynamic conversation = m_fbClient.Get(m_conversations.paging.next);
                if (conversation.data.Count > 0)
                {
                    m_conversations = conversation;
                }
            }
            catch (Exception ex) when (ex is Facebook.FacebookOAuthException || ex is Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                // Error caused
            }
        }

        public void prevConversations()
        {
            try
            {
                dynamic conversation = m_fbClient.Get(m_conversations.paging.previous);
                if (conversation.data.Count > 0)
                {
                    m_conversations = conversation;
                }
            }
            catch (Exception ex) when (ex is Facebook.FacebookOAuthException || ex is Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
            }
        }
        
        public List<FacebookMessage> getComments(string conversationID)
        {
            List<FacebookMessage> messageList = new List<FacebookMessage>();

            // TODO: Check values
            if (m_next == "" || m_next == null)
            {
                for (int i = 0; i < m_conversations.data.Count; i++)
                {
                    if (m_conversations.data[i].id == conversationID)
                    {
                        m_messages = m_conversations.data[i].comments;
                        if (m_conversations.data[i].comments != null)
                        {
                            m_next = m_conversations.data[i].comments.paging.next;
                        }
                        break;
                    }
                }

                // May not be neccessary -- check else Post call
                // TODO: Parse m_next parameter to change limit size

            }
            else
            {
                m_next = m_next.Replace("limit=25", "limit=10000");
                m_messages = m_fbClient.Get(m_next);
                // m_messages = m_fbClient.Post(m_next, new { method = "GET", limit = 10000 });
                if (m_messages.paging != null)
                {
                    m_next = m_messages.paging.next;
                }
                else
                {
                    Cleanup();
                }
            }
            if (m_messages != null)
            {
                for (int i = 0; i < m_messages.data.Count; i++)
                {
                    FacebookMessage fm = new FacebookMessage();

                    User sender = new User();
                    sender.id = m_messages.data[i].from.id;
                    sender.name = m_messages.data[i].from.name;
                    fm.sender = sender;
                    string dtStr = (string)m_messages.data[i].created_time;
                    int year = Int32.Parse(dtStr.Substring(0, 4));
                    int month = Int32.Parse(dtStr.Substring(5, 2));
                    int day = Int32.Parse(dtStr.Substring(8, 2));
                    int hour = Int32.Parse(dtStr.Substring(11, 2));
                    int minute = Int32.Parse(dtStr.Substring(14, 2));
                    int second = Int32.Parse(dtStr.Substring(17, 2));

                    fm.timeSent = new DateTime(year, month, day, hour, minute, second);
                    fm.message = m_messages.data[i].message;
                    messageList.Add(fm);
                }
            }
            return messageList;
        }
    }
}