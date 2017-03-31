using System.Collections.Generic;
using Facebook_Message_Analyzer.Business;
using System.Windows.Forms;
using ModuleInterface;
using System;

namespace Facebook_Message_Analyzer.Presentation
{
    public partial class GeneralPreferences : PreferenceControl
    {
        public GeneralPreferences()
        {
            InitializeComponent();
        }

        public override Dictionary<string, object> GetValues()
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("cache", cacheMessages.Checked);

            return values;

        }

        public override void LoadValues(Dictionary<string, object> values)
        {
            cacheMessages.Checked = (bool) values["cache"];
        }
    }
}
