using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace Facebook_Message_Analyzer.Data
{ 
    /// <summary>
    /// THIS CLASS IS OBSOLETE
    /// </summary>
    static class DatabaseConstants
    {
        public static string PATH
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }
        }
        public const string CONFIG_DB = "config.mdf";


        
    }
}
