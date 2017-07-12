using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomInstaller
{
    public struct ModuleDownloadParams
    {
        public List<Type> selectedModules;
    }

    public partial class ModuleDownloadControl : UserControl
    {
        public ModuleDownloadControl(ModuleDownloadParams initialValues)
        {
            InitializeComponent();
        }
        public ModuleDownloadParams getParams()
        {
            // TODO: Fix
            return new ModuleDownloadParams();
        }
    }
}
