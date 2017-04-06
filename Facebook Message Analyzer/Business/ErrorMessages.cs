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

        public static DateTime timeOfOverflow;
        public static void APIOverflow()
        {
            int timeDifference = (DateTime.Now - timeOfOverflow).Minutes;
            if (timeOfOverflow == null || timeDifference > 20)
            {
                timeOfOverflow = DateTime.Now;
                timeDifference = (DateTime.Now - timeOfOverflow).Minutes;
            }
            string message = String.Concat("Too much information has been requested from Facebook for too long.  You must wait about ", 20 - timeDifference, " minutes");
            MessageBox.Show(message);
        }

        public static void AnalysisAborted()
        {
            MessageBox.Show("Message Analysis Aborted");
        }

        internal static void NoAnalysisModuleSelected()
        {
            MessageBox.Show("No Analysis Module Selected.  You must select an analysis module to proceed");
        }
        public static void NoMessages()
        {
            MessageBox.Show("There were no messages in this conversation.");
        }
    }
}
