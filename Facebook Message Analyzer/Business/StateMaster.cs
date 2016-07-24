/* TODO
 * Facebook Model Times
 */

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
        private static Form m_activeForm = null;
        private static bool m_loggedIn = false;


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

            if (!m_loggedIn)
            {
                return;
            }

            m_activeForm = new ConversationSelectionForm();
            Application.Run(m_activeForm);
        }

        public static void Login()
        {
            AuthenticationForm loginScreen = new AuthenticationForm();
            loginScreen.ShowDialog();
            if (m_loggedIn)
            {
                m_activeForm.Close();
            }
        }

        public static Uri getLoginURL()
        {
            return FBQueryManager.Manager.getLoginURL();
        }

        public static void setOAuthToken(string token)
        {
            if (token != null && token != "")
            {
                FBQueryManager.Manager.setToken(token);
                //m_activeForm.Close();
                m_loggedIn = true;
            }
        }

        public static dynamic getConversations()
        {
            return FBQueryManager.Manager.getConversations();
        }

        public static void setConversation()
        {
            FBQueryManager.Manager.setConversation();
        }

        public static void selectAnalysisModules()
        {
            SelectModulesForm smf = new SelectModulesForm();
            smf.ShowDialog();
        }

        public static void runAnalysisModules(int conversationIndex)
        {
            m_activeForm.Close();
            m_activeForm = new AnalyzingForm();
            Application.Run(m_activeForm);
            // TODO
            // Close window and open new one for analysis
            // Have new one open analysis window for each of the analysis options available
        }

        public static void showPreferences()
        {
            ModulePreferencesForm mpf = new ModulePreferencesForm();
            mpf.ShowDialog();
            // TODO
            // Open modal for displaying module preferences
        }

        public static void Exit()
        {
            m_activeForm.Close();
            m_activeForm = null;
        }
    }
}
