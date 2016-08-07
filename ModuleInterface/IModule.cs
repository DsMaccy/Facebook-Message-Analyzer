using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuleInterface
{
    public struct User
    {
        string name;
        string id;
    }
    public struct Message
    {
        string message;
        User sender;
        DateTime timeSent;
    }

    public interface IModule
    {
        void analyze(Message message);
        bool canParallelize();
        void parallelAnalyze(Message message);

        bool preferencesAvailable();
        UserControl getPreferenceForm();
        void savePreferences(Dictionary<string,dynamic> newValues);

        bool formAvailable();
        // TODO: add form

        // TODO: Add sql data for preferences???
    }
}