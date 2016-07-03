using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Facebook;
using Facebook_Message_Analyzer.Business;

namespace Facebook_Message_Analyzer.Presentation
{
    public partial class AuthenticationForm : Form
    {     
        private const string m_APP_ID = "1465357507096514";
        private const string m_APP_SECRET = "ac5817c4f2dd07bf18137d7297d4015c";
        private const string m_SUCCESS_URI = "https://www.facebook.com/connect/login_success.html";
        private string m_queryURL = "";
        private bool m_connectionSuccess;

        public AuthenticationForm()
        {
            InitializeComponent();
            m_connectionSuccess = requestLogin();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>false if the form cannot be properly displayed and should be closed immediately</returns>
        private bool requestLogin()
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
                return false;
            }
            FBClient.AccessToken = token.access_token;
            FBClient.AppId = m_APP_ID;
            FBClient.AppSecret = m_APP_SECRET;

            Uri loginUrl = FBClient.GetLoginUrl(parameters);
            m_queryURL = loginUrl.AbsoluteUri;
            authBrowser.Navigate(m_queryURL);

            return true;
        }

        private void authBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            var fb = new FacebookClient();
            FacebookOAuthResult oauthResult;
            if (fb.TryParseOAuthCallbackUrl(e.Url, out oauthResult))
            {
                // The url is the result of OAuth 2.0 authentication
                if (oauthResult.IsSuccess)
                {
                    StateMaster.setOAuthToken(oauthResult.AccessToken);
                    Close();
                }
                else // User pressed cancel button
                {
                    var errorDescription = oauthResult.ErrorDescription;
                    var errorReason = oauthResult.ErrorReason;

                    ErrorMessages.LoginCancelled();
                    Close();
                }
            }
            else if (e.Url.AbsolutePath != "/login.php")
            {
                // The url is NOT the result of OAuth 2.0 authentication.
                MessageBox.Show("Error logging In: Please try again");
                m_connectionSuccess = requestLogin();
                if (!m_connectionSuccess)
                {
                    Close();
                }
            }
        }

        /*
        private void AuthenticationScreen_Load(object sender, EventArgs e)
        {
            //authBrowser.Navigate(m_url);
            if (authBrowser.Url != null &&
                authBrowser.Url.ToString() == m_SUCCESS_URI)
            {
                Close();
            }
            else
            {
                ErrorMessages.LoginFailure();
                m_connectionSuccess = requestLogin();
                if (!m_connectionSuccess)
                {
                    Close();
                }
            }
        }
        */

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (!m_connectionSuccess)
            {
                this.Close();
            }
        }
    }
}
