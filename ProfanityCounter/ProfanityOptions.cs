using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModuleInterface;

namespace ProfanityCounter
{
    public partial class ProfanityOptions : PreferenceControl
    {
        public ProfanityOptions()
        {
            InitializeComponent();
        }

        public override Dictionary<string, object> GetValues()
        {
            throw new NotImplementedException();
        }
        public override void LoadValues(Dictionary<string, object> initialValues)
        {
            throw new NotImplementedException();
        }
    }
}
