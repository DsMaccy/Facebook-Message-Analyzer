using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleInterface
{
    interface IPreference
    {
        Dictionary<string, object> GetValues();
        void LoadValues(Dictionary<string, object> initialValues);
    }
}
