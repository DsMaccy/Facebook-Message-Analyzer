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
        
        private string m_queryURL = "";
        private bool m_connectionSuccess;

        public AuthenticationForm(Uri url)
        {
            InitializeComponent();

            m_connectionSuccess = (url != null);
            if (m_connectionSuccess)
            {
                m_queryURL = url.AbsoluteUri;
                authBrowser.Navigate(m_queryURL);
            }
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
            else if (e.Url.AbsoluteUri == StateMaster.getLogoutRedirectUrl())
            {
                this.Close();
            }
            /*
            else if (e.Url.AbsoluteUri != "https://www.facebook.com/home.php")
            {
                Business.StateMaster.logout();
            }*/
            // Unknown location
            /*
            else if (e.Url.AbsoluteUri != m_queryURL)
            {
                Business.StateMaster.logout();
            }*/
        }

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