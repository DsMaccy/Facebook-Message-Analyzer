using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facebook_Message_Analyzer.Business
{
    static class ErrorMessages
    {
        public static void WebConnectionFailure()
        {
            MessageBox.Show("Unable to access internet --  Please check connection");
        }

        public static void LoginFailure()
        {
            MessageBox.Show("Error logging In: Please try again");
        }

        public static void LoginCancelled()
        {
            MessageBox.Show("You must login to proceed");
        }
    }
}
