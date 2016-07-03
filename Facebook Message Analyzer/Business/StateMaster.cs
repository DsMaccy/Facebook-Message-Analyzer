using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Facebook_Message_Analyzer.Business;
using Facebook_Message_Analyzer.Presentation;
using Facebook_Message_Analyzer.Data;

namespace Facebook_Message_Analyzer.Business
{
    internal static class StateMaster
    {
        private static Form m_activeForm;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            m_activeForm = new WelcomeForm();
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(m_activeForm);
        }

        public static void Login()
        {
            AuthenticationForm loginScreen = new AuthenticationForm();
            loginScreen.ShowDialog();
        }
        public static void setOAuthToken(string token)
        {
            if (token != null && token != "")
            {
                FBQueryManager.Manager.setToken(token);
                // TODO: Uncomment code -- Close Current Form and Continue to the ConversationSelectionForm
                // m_activeForm.Close();
                // m_activeForm = new ConversationSelectionForm()

                // Application.Run(m_activeForm)
            }
        }

        public static void Exit()
        {
            m_activeForm.Close();
            m_activeForm = null;
        }
    }
}
