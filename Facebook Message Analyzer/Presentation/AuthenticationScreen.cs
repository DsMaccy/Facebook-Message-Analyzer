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

namespace Facebook_Message_Analyzer.Presentation
{
    public partial class AuthenticationScreen : Form
    {
        public AuthenticationScreen()
        {
            InitializeComponent();
        }
        private string m_url;
        private dynamic m_token;

        public AuthenticationScreen(string authenticationUrl)
        {
            InitializeComponent();

            dynamic parameters = new System.Dynamic.ExpandoObject();

            parameters.client_id = appId;
            parameters.redirect_uri = "https://www.facebook.com/connect/login_success.html";

            // The requested response: an access token (token), an authorization code (code), or both (code token).
            parameters.response_type = "token";

            // list of additional display modes can be found at http://developers.facebook.com/docs/reference/dialogs/#display
            parameters.display = "popup";

            // add the 'scope' parameter only if we have extendedPermissions.
            string extendedPermissions = "read_mailbox";
            parameters.scope = extendedPermissions;

            // generate the login url

            Facebook.FacebookClient FBClient = new Facebook.FacebookClient();
            dynamic token = FBClient.Get("oauth/access_token", new
            {
                client_id = appId, // "1465357507096514"
                client_secret = appSecret, // "ac5817c4f2dd07bf18137d7297d4015c"
                grant_type = "client_credentials"
            });
            FBClient.AccessToken = token.access_token;
            FBClient.AppId = appId;
            FBClient.AppSecret = appSecret;

            Uri loginUrl = FBClient.GetLoginUrl(parameters);
            
            m_token = token;
            authBrowser.Navigate(loginUrl.AbsoluteUri);
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
                    m_token = oauthResult.AccessToken;
                    Close();
                }
                else
                {
                    var errorDescription = oauthResult.ErrorDescription;
                    var errorReason = oauthResult.ErrorReason;
                }
            }
            else
            {
                // The url is NOT the result of OAuth 2.0 authentication.
            }
        }

        private void AuthenticationScreen_Load(object sender, EventArgs e)
        {
            //authBrowser.Navigate(m_url);
            if (authBrowser.Url != null &&
                authBrowser.Url.ToString() == "https://www.facebook.com/connect/login_success.html")
            {
                Close();
            }
        }
    }
}
