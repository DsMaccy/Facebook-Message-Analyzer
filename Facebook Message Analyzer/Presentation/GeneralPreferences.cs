using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Facebook_Message_Analyzer.Business;

namespace Facebook_Message_Analyzer.Presentation
{
    public partial class GeneralPreferences : UserControl
    {
        public GeneralPreferences()
        {
            InitializeComponent();
        }

        public void saveData()
        {
            StateMaster.setCacheData(cacheMessages.Checked);
        }
    }
}
