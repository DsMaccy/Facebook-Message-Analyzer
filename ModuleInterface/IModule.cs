using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        // TODO: Add preference

        bool formAvailable();
        // TODO: add form

        // TODO: Add sql data for preferences???
        
    }
}