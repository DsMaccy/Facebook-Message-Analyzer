/* TODO
 * Create a DB for each different necessary version and clean up DB files
 *  - Consider creating contract file and enum to access different databases
 * Find way to Parse DLL files in path (preferences) and add modules to application
 * Set up Preferences DB
 * Set up caching for 
 * s
 * Create Module for Generic Analysis
 * Facebook Model Times
 * Create Constants for Width and Heigth offsets
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Facebook_Message_Analyzer.Business;
using Facebook_Message_Analyzer.Presentation;
using Facebook_Message_Analyzer.Data;
using ModuleInterface;
using System.IO;
using System.Reflection;
using GeneralInfoModule;


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
            AnalyzingForm af = new AnalyzingForm();
            af.ShowDialog();
            // TODO -- Have new one open analysis window for each of the analysis options available
        }

        public static void showPreferences()
        {
            ModulePreferencesForm mpf = new ModulePreferencesForm();
            mpf.ShowDialog();
        }

        public static Dictionary<string, Type> getModules()
        {
            Dictionary<string, Type> modules = new Dictionary<string, Type>();
            modules.Add("General Info Module", typeof(GeneralInfoModule.GeneralInfo));

            string dllPath = ConfigManager.Manager.getValue(ConfigManager.GENERIC_TABLE_NAME, ConfigManager.DLL_PATH_TAG);
            if (dllPath != null)
            {
                foreach (string file in Directory.EnumerateFiles(dllPath, "*.dll"))
                {
                    Assembly assembly = Assembly.LoadFrom("dllPath");
                    if (assembly.GetType() is IModule)
                    {
                        AppDomain.CurrentDomain.Load(assembly.GetName());
                        modules.Add(assembly.GetName() + "Module", assembly.GetType());
                    }
                }
            }

            return modules;
        }

        public static void Exit()
        {
            m_activeForm.Close();
            m_activeForm = null;
        }
    }
}
