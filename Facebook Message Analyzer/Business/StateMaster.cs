/* TODO
 * Fix up profanity counter module (look at profanityCounter.cs for more details)
 * Properties:
 *      Consider making dirty module preferences red
 * Test to make sure data sets can be loaded between users
 * Test ConversationIterator:
 *      Cases to Consider:
 *          1) GOOD: Messages uncached
 *          2) GOOD: Messages fully cached
 *          3) Messages partially cached, no new messages
 *          4) Message fully cached, but new messages present
 *          5) Messages partially cached AND new messages present
 * Create installer that will set registry values and download appropriate dll files
 * Create Additional Modules: Points Module ...?
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
                    repeat = false;
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
            lock (m_activeForm)
            {
                m_activeForm.Invoke(new MethodInvoker(() =>
                {
                    form.Show();
                }));
            }
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


            Dictionary<string, object> defaultValues = gp.GetValues(); //getControlValues(gp.Controls);
            Dictionary<string, Type> columns = new Dictionary<string, Type>();
            foreach (KeyValuePair<string, object> kv in defaultValues)
            {
                columns.Add(kv.Key, kv.Value.GetType());
            }
            DataSetManager.Manager.addTable(DataSets.Config, DataSetManager.GENERIC_TABLE_NAME, columns);
            DataSetManager.Manager.addValuesToEnd(DataSets.Config, DataSetManager.GENERIC_TABLE_NAME, defaultValues);

            columns = new Dictionary<string, Type>();
            columns.Add(DataSetManager.SELECTED_MODULES_NAME_COLUMN_TAG, typeof(string));

            DataSetManager.Manager.addTable(DataSets.Config, DataSetManager.SELECTED_MODULES_TABLE_NAME, columns, DataSetManager.SELECTED_MODULES_NAME_COLUMN_TAG);
            defaultValues = new Dictionary<string, object>();
            defaultValues.Add(DataSetManager.SELECTED_MODULES_NAME_COLUMN_TAG, "GeneralInfo");
            DataSetManager.Manager.addValuesToEnd(DataSets.Config, DataSetManager.SELECTED_MODULES_TABLE_NAME, defaultValues);
            DataSetManager.Manager.saveDataSet(DataSets.Config);
        }

        public static Dictionary<string, object> getPreferenceData(string tag)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            
            if (tag == "General")
            {
                IEnumerator iterator = DataSetManager.Manager.getData(DataSets.Config, DataSetManager.GENERIC_TABLE_NAME);
                GeneralPreferences gp = new GeneralPreferences();
                Dictionary<string, object> dataValues = gp.GetValues();
                if (iterator.MoveNext())
                {
                    DataRow dr = ((DataRow)iterator.Current);
                    foreach (KeyValuePair<string, object> kv in dataValues)
                    {
                        values[kv.Key] = dr[kv.Key];
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, object> kv in dataValues)
                    {
                        values[kv.Key] = kv.Value;
                    }
                }
            }
            else
            {
                Dictionary<string, Type> modules = getModules();

                Type[] types = new Type[0]; object[] parameters = new object[0];
                IModule moduleObject = (modules[tag]).GetConstructor(types).Invoke(parameters) as IModule;

                if (moduleObject.preferencesAvailable()) // Previous Data Never Stored
                {
                    IEnumerator iterator = DataSetManager.Manager.getData(DataSets.Config, tag);
                    if (iterator == null || !iterator.MoveNext()) // No Table or Empty Table
                    {
                        
                        PreferenceControl preferenceControl = moduleObject.getPreferenceControl();

                        Dictionary<string, object> mappings = preferenceControl.GetValues();
                        Dictionary<string, Type> columns= new Dictionary<string, Type>();
                        foreach (KeyValuePair<string, object> mapping in mappings)
                        {
                            values.Add(mapping.Key, mapping.Value);
                            columns.Add(mapping.Key, mapping.Value.GetType());
                        }
                        if (iterator == null) { DataSetManager.Manager.addTable(DataSets.Config, tag, columns); }
                    }
                    else// Should only have 1 data element
                    {
                        DataRow dr = iterator.Current as DataRow;
                        foreach (DataColumn column in dr.Table.Columns)
                        {
                            values.Add(column.ColumnName, dr[column]);
                        }
                    }
                }
            }

            return values;
        }

        public static void savePreferenceData(string preferenceTag, Dictionary<string, object> values)
        {
            if (preferenceTag == "General")
            {
                DataSetManager.Manager.clearTable(DataSets.Config, DataSetManager.GENERIC_TABLE_NAME);
                DataSetManager.Manager.addValuesToEnd(DataSets.Config, DataSetManager.GENERIC_TABLE_NAME, values);
            }
            else
            {
                DataSetManager.Manager.clearTable(DataSets.Config, preferenceTag);
                DataSetManager.Manager.addValuesToEnd(DataSets.Config, preferenceTag, values);
            }
            DataSetManager.Manager.saveDataSet(DataSets.Config);
        }

        public static void loadActiveModules()
        {
            IEnumerator iterator = DataSetManager.Manager.getData(DataSets.Config, DataSetManager.SELECTED_MODULES_TABLE_NAME);
            Dictionary<string, Type> modules = getModules();
            //if (iterator != null)
            //{
                while (iterator.MoveNext())
                {
                    string moduleName = ((DataRow)iterator.Current)[DataSetManager.SELECTED_MODULES_NAME_COLUMN_TAG] as string;
                    m_activeModules.Add(modules[moduleName]);
                }
            //}
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
            // This set the m_activeModules instance variable to contain the appropriate types
            m_activeModules = new List<Type>(activeModules);

            // Modify the dataset values appropriately
            DataSetManager.Manager.clearTable(DataSets.Config, DataSetManager.SELECTED_MODULES_TABLE_NAME);
            Dictionary<string, Type> modules = getModules();
            // Currently need to traverse dictionary which is not efficient but shouldn't matter
            foreach (KeyValuePair<string, Type> kv in modules)
            {
                if (m_activeModules.Contains(kv.Value))
                {
                    Dictionary<string, object> activeModule = new Dictionary<string, object>();
                    activeModule.Add(DataSetManager.SELECTED_MODULES_NAME_COLUMN_TAG, kv.Key);
                    DataSetManager.Manager.addValuesToEnd(DataSets.Config, DataSetManager.SELECTED_MODULES_TABLE_NAME, activeModule);
                }
            }
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

        public static bool getCacheMessages()
        {
            IEnumerator iterator = DataSetManager.Manager.getData(DataSets.Config, DataSetManager.GENERIC_TABLE_NAME);
            iterator.MoveNext();

            // Highly coupled w/ the GeneralPreference control
            return (bool)((DataRow)iterator.Current)["cache"];
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

        /*
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
        }*/

        #endregion
    }
}
