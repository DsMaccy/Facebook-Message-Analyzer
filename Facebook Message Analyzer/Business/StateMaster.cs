/* TODO
 * Create Additional Modules:
 *      General Module Preferences Form
 *      Test Module for analysis counting
 * Properties:
 *      Handle Switching between various Property types
 *      Fix Dirty Metric (when Apply is enabled)
 *          Consider making dirty module preferences red
 * Test to make sure data sets can be loaded between users
 * ConversationIterator:
 *      Cases to Consider:
 *          1) GOOD: Messages uncached
 *          2) GOOD: Messages fully cached
 *          3) Messages partially cached, no new messages
 *          4) Message fully cached, but new messages present
 *          5) Messages partially cached AND new messages present
 * Create installer that will set registry values and download appropriate dll files
 * 
 * Create Additional Modules
 *      Profanity Module
 *      Points Module ...?
 */

using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using Facebook_Message_Analyzer.Presentation;
using Facebook_Message_Analyzer.Data;
using ModuleInterface;
using System.IO;
using System.Reflection;
using GeneralInfoModule;
using System.Threading;
using System.Data;

namespace Facebook_Message_Analyzer.Business
{
    internal static class StateMaster
    {
        private static Form m_activeForm = null;
        private static bool m_loggedIn = false;
        private static bool m_restart = false;
        private static List<Type> m_activeModules = new List<Type>();
        private static Analyzer m_analyzer = null;
        private static Form m_analysisForm = null;
        private static string m_dllLocation = "";
        private static string m_path = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            getPath();
            m_dllLocation = Microsoft.Win32.Registry.GetValue(
                Microsoft.Win32.Registry.CurrentUser.Name + "\\SOFTWARE\\Facebook Message Analyzer", 
                "library path", 
                (m_path + "libraries" + "\\")) as string;

            do
            {
                m_restart = false;

                // Set the Active Form to the welcome screen
                m_activeForm = new WelcomeForm();
                Application.Run(m_activeForm);

                if (!m_loggedIn)
                {
                    return;
                }
                initializeConfig();
                loadActiveModules();
                m_activeForm = new ConversationSelectionForm();
                Application.Run(m_activeForm);
            } while (m_restart);
        }

        #region Form Activation Methods

        public static void login()
        {
            AuthenticationForm loginScreen = new AuthenticationForm(FBQueryManager.Manager.getLoginURL());
            loginScreen.ShowDialog();
            if (m_loggedIn)
            {
                DataSetManager.Manager.loadFiles();
                m_activeForm.Close();
            }
        }

        public static void logout()
        {
            AuthenticationForm loginScreen = new AuthenticationForm(FBQueryManager.Manager.getLogoutURL());
            loginScreen.ShowDialog();
            

            // Reset states and start over from the Welcome Screen
            m_loggedIn = false;
            m_activeForm.Close();
            m_restart = true;
            FBQueryManager.Manager.setToken(null);
        }

        public static void selectAnalysisModules()
        {
            SelectModulesForm smf = new SelectModulesForm();
            smf.ShowDialog();
        }

        public static void runAnalysisModules(string conversationIndex)
        {
            m_analyzer = new Analyzer(conversationIndex);
            m_analyzer.setModules(m_activeModules);
            m_analyzer.runAnalysisAsync();

            m_analysisForm = new AnalyzingForm();
            m_analysisForm.ShowDialog();
        }

        public static void closeAnalysisForm()
        {
            bool repeat = false;
            do
            {
                try
                {
                    m_analysisForm.Invoke(new MethodInvoker(
                        () =>
                        {
                            m_analysisForm.Close();
                        }
                    ));
                }
                catch (Exception e)
                {
                    if (e is InvalidOperationException || e is NullReferenceException)
                    {
                        repeat = true;
                        Thread.Sleep(100);
                    }
                }
            } while (repeat);
        }

        public static void displayAnalysisResult(Form form)
        {
            m_activeForm.Invoke(new MethodInvoker(() => { form.Show(); }));
        }

        public static void abortAnalysis()
        {
            m_analyzer.abort();
            ErrorMessages.AnalysisAborted();
        }

        public static void showPreferences()
        {
            ModulePreferencesForm mpf = new ModulePreferencesForm();
            mpf.ShowDialog();
        }

        public static void Exit()
        {
            m_activeForm.Close();
            m_activeForm = null;
        }

        #endregion

        #region Transfer Methods

        public static void setOAuthToken(string token)
        {
            if (token != null && token != "")
            {
                FBQueryManager.Manager.setToken(token);
                //m_activeForm.Close();
                m_loggedIn = true;
            }
            else
            {
                m_loggedIn = false;
            }
        }

        public static string getLogoutRedirectUrl()
        {
            return Data.FBQueryManager.LOGOUT_URL;
        }

        private static void initializeConfig()
        {
            if (DataSetManager.Manager.getData(DataSets.Config, DataSetManager.GENERIC_TABLE_NAME) != null)
            {
                return;
            }
            Dictionary<string, string> values = new Dictionary<string, string>();

            GeneralPreferences gp = new GeneralPreferences();

            Dictionary<string, object> defaultValues = getControlValues(gp.Controls);
            Dictionary<string, Type> columns = new Dictionary<string, Type>();
            foreach (KeyValuePair<string, object> kv in defaultValues)
            {
                columns.Add(kv.Key, kv.Value.GetType());
            }
            columns.Add(DataSetManager.SELECTED_MODULES, typeof(string));
            defaultValues[DataSetManager.SELECTED_MODULES] = "GeneralInfo";

            DataSetManager.Manager.addTable(DataSets.Config, DataSetManager.GENERIC_TABLE_NAME, columns);
            DataSetManager.Manager.addValuesToEnd(DataSets.Config, DataSetManager.GENERIC_TABLE_NAME, defaultValues);

            DataSetManager.Manager.saveDataSet(DataSets.Config);
        }

        public static Dictionary<string, string> getPreferenceData(string tag)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            if (tag != "General")
            {
                Dictionary<string, Type> modules = getModules();

                Type[] types = new Type[0]; object[] parameters = new object[0];
                IModule moduleObject = (modules[tag]).GetConstructor(types).Invoke(parameters) as IModule;

                if (moduleObject.preferencesAvailable())
                {
                    IEnumerator iterator = DataSetManager.Manager.getData(DataSets.Config, tag);
                    if (iterator == null)
                    {
                        Control preferenceControl = moduleObject.getPreferenceControl();

                        Dictionary<string, object> mappings = getControlValues(preferenceControl.Controls);
                        foreach (KeyValuePair<string, object> mapping in mappings)
                        {
                            values.Add(mapping.Key, mapping.Value.ToString());
                        }
                    }
                    else
                    {
                        while (iterator.MoveNext())
                        {
                            DataRow dr = iterator.Current as DataRow;
                            foreach (DataColumn column in dr.Table.Columns)
                            {
                                values.Add(column.ColumnName, dr[column].ToString());
                            }
                        }
                    }
                }
                return values;
            }
            else
            {
                IEnumerator iterator = DataSetManager.Manager.getData(DataSets.Config, DataSetManager.GENERIC_TABLE_NAME);
                iterator.MoveNext();

                GeneralPreferences gp = new GeneralPreferences();

                Dictionary<string, object> dataValues = getControlValues(gp.Controls);
                foreach (KeyValuePair<string, object> kv in dataValues)
                {
                    values[kv.Key] = kv.Value.ToString();
                }

                return values;
            }
        }

        public static void loadActiveModules()
        {
            IEnumerator iterator = DataSetManager.Manager.getData(DataSets.Config, DataSetManager.GENERIC_TABLE_NAME);
            iterator.MoveNext();
            string selectedModules = ((DataRow)iterator.Current)[DataSetManager.SELECTED_MODULES] as string;
            string[] selectedModuleTags = selectedModules.Split(';');

            Dictionary<string, Type> modules = getModules();
            for (int index = 0; index < selectedModuleTags.Length; index++)
            {
                string moduleTag = selectedModuleTags[index].Trim();
                if (moduleTag != "")
                {
                    m_activeModules.Add(modules[moduleTag]);
                }
            }
        }

        public static Dictionary<string, Type> getModules()
        {
            Dictionary<string, Type> modules = new Dictionary<string, Type>();
            //modules.Add("General Info Module", typeof(GeneralInfoModule.GeneralInfo));

            foreach(string dllFile in Directory.EnumerateFiles(m_dllLocation, "*.dll"))
            {
                Assembly dllReference = Assembly.LoadFile(dllFile);
                Type[] types = dllReference.GetExportedTypes();
                for (int i = 0; i < types.Length; i++)
                {
                    Type type = types[i];
                    object result = types[i].GetInterface("IModule");
                    if (result != null)
                    {
                        modules.Add(types[i].Name, types[i]);
                    }
                }
            }
            return modules;
        }

        public static bool isModuleActive(Type module)
        {
            for (int i = 0; i < m_activeModules.Count; i++)
            {
                if (module.Equals(m_activeModules[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public static void setActiveModules(params Type [] activeModules)
        {
            m_activeModules.Clear();
            for (int i = 0; i < activeModules.Length; i++)
            {
                m_activeModules.Add(activeModules[i]);
            }

            Dictionary<string, object> values = getPreviousConfigValues();

            List<string> newlyActiveModules = new List<string>();
            Dictionary<string, Type> modules = getModules();

            foreach (KeyValuePair<string, Type> kv in modules)
            {
                if (m_activeModules.Contains(kv.Value))
                {
                    newlyActiveModules.Add(kv.Key);
                }
            }

            values[DataSetManager.SELECTED_MODULES] = string.Join(";", newlyActiveModules);

            DataSetManager.Manager.clearTable(DataSets.Config, DataSetManager.GENERIC_TABLE_NAME);
            DataSetManager.Manager.addValuesToEnd(DataSets.Config, DataSetManager.GENERIC_TABLE_NAME, values);
            DataSetManager.Manager.saveDataSet(DataSets.Config);
        }

        public static string getPath()
        {
            if (m_path == null)
            {
                System.Reflection.Assembly assemblyObj = System.Reflection.Assembly.GetExecutingAssembly();

                System.Reflection.AssemblyCompanyAttribute companyAttr = System.Reflection.AssemblyCompanyAttribute.GetCustomAttribute(assemblyObj, typeof(System.Reflection.AssemblyCompanyAttribute)) as System.Reflection.AssemblyCompanyAttribute;
                string companyName = companyAttr.Company;

                System.Reflection.AssemblyTitleAttribute titleAttr = System.Reflection.AssemblyTitleAttribute.GetCustomAttribute(assemblyObj, typeof(System.Reflection.AssemblyTitleAttribute)) as System.Reflection.AssemblyTitleAttribute;
                string programTitle = titleAttr.Title;

                // ... AppData/Local/Solace Inc./Facebook Message Analyzer/
                m_path = Microsoft.Win32.Registry.GetValue(
                    Microsoft.Win32.Registry.CurrentUser.Name + "\\SOFTWARE\\Facebook Message Analyzer", 
                    "data path", 
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + companyName + "\\" + programTitle + "\\"
                ) as string;
            }

            return m_path;
        }

        public static void setCacheData(bool shouldCache)
        {
            Dictionary<string, object> values = getPreviousConfigValues();
            values[DataSetManager.CACHE_DATA_TAG] = shouldCache;
            
            DataSetManager.Manager.clearTable(DataSets.Config, DataSetManager.GENERIC_TABLE_NAME);
            DataSetManager.Manager.addValuesToEnd(DataSets.Config, DataSetManager.GENERIC_TABLE_NAME, values);
            DataSetManager.Manager.saveDataSet(DataSets.Config);
        }

        #endregion

        #region Helper Methods

        private static Dictionary<string, object> getPreviousConfigValues()
        {
            Dictionary<string, object> values = new Dictionary<string, object>();

            IEnumerator iterator = DataSetManager.Manager.getData(DataSets.Config, DataSetManager.GENERIC_TABLE_NAME);
            iterator.MoveNext();
            DataRow dr = iterator.Current as DataRow;
            foreach (DataColumn column in dr.Table.Columns)
            {
                values[column.ColumnName] = dr[column];
            }
            return values;
        }

        private static Dictionary<string, object> getControlValues(Control.ControlCollection controls)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            foreach (Control control in controls)
            {
                if (control is CheckBox)
                {
                    values.Add(control.Name, ((CheckBox)control).Checked);
                }
                else
                {
                    values.Add(control.Name, control.Text);
                }
            }
            return values;
        }

        #endregion
    }
}
