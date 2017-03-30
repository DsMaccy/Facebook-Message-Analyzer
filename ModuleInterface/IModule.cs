using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ModuleInterface
{
    public interface IModule
    {
        /// <summary>
        /// Gives an explanation of what this analysis module does
        /// </summary>
        /// <returns>A string description</returns>
        string description();

        /// <summary>
        /// Analysis algorithm that takes a single message at a time in order
        /// </summary>
        /// <param name="message">A message to be analyzed</param>
        void analyze(FacebookMessage message);

        /// <summary>
        /// Determines whether this module can analyze the messages using multithreading
        /// </summary>
        /// <returns>True if this module can run it's analysis in a thread-safe manner</returns>
        bool canParallelize();

        /// <summary>
        /// Analyzes a single message at a time.  This method gets called in a multithreaded manner.
        /// Only runs if canParallelize returns true
        /// </summary>
        /// <param name="message">A message to be analyzed</param>
        void parallelAnalyze(FacebookMessage message);

        /// <summary>
        /// Determines whether this module has user options available for modification
        /// </summary>
        /// <returns>Returns true if this module should display a preferences control for the user</returns>
        bool preferencesAvailable();

        /// <summary>
        /// This is called by the application whenever the User opens up the preference menu and is used universally across all instances of the IModule class
        /// Only called if preferencesAvailable returns true
        /// </summary>
        /// <returns>The User Control that is used as the preferences screen</returns>
        PreferenceControl getPreferenceControl();

        /// <summary>
        /// Called when the user preferences get modified
        /// </summary>
        /// <param name="newValues">A dictionary of the UI elements that were modified as well as their modified values</param>
        void savePreferences(Dictionary<string, object> newValues);

        /// <summary>
        /// Determines whether this module will create a popup display
        /// </summary>
        /// <returns>Returns true if the module should be allowed to create a popup window after the analysis is complete</returns>
        bool formAvailable();

        /// <summary>
        /// Provides the form that should be displayed when the analysis has been finished
        /// </summary>
        /// <returns>Returns the form meant to be displayed or null if no form is meant to be displayed</returns>
        Form getResultForm();
    }
}