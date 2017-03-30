using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModuleInterface;

namespace TestModule
{
    public partial class TestControl : PreferenceControl
    {
        public TestControl() : base()
        {
            InitializeComponent();
        }

        public override Dictionary<string, object> GetValues()
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("comboBox1", this.Controls["comboBox1"].Text);

            // TODO: Fill

            return values;
        }
    }
}
