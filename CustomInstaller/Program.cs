using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace CustomInstaller
{
    // TODO: Create prompt for install directory and location of data
    static class Program
    {
        private static bool m_continue;

        private static string m_install_directory;
        private static string m_data_directory;
        private static bool m_create_desktop_shortcut;
        private static bool m_create_start_menu_shortcut;
        
        private static Dictionary<string, byte[]> m_libraryFiles;

        private const string PROGRAM_EXECUTABLE = "Facebook Message Analyzer.exe";

        public static void setState(DownloadOptionsParams values)
        {
            m_install_directory = values.programLocation;
            m_data_directory = values.dataLocation;
            m_create_desktop_shortcut= values.createDesktopShortcut;
            m_create_start_menu_shortcut = values.createStartMenuShortcut;
        }

        public static void setProceed()
        {
            m_continue = true;
        }

        public static void setState(ModuleDownloadParams values)
        {
            // TODO: Use types to extrapolate file names and binary arrays from resources
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            m_install_directory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Facebook Message Analyzer\\";
            m_data_directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Solace Inc.\\Facebook Message Analyzer\\";
            m_create_desktop_shortcut = true;
            m_create_start_menu_shortcut = true;
            m_libraryFiles = new Dictionary<string, byte[]>();

            // TODO: Add UI elements for each of these elements

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new InstallPrompt());

            if (!m_continue)
            { Abort(); }

            RegistryKey rk = Microsoft.Win32.Registry.CurrentUser;
            using (rk = rk.CreateSubKey("Software\\" + Application.ProductName + "_2"))
            {
                if (rk == null)
                {
                    Abort();
                }
                // TODO: Remove the _2 in these values when the UI has been properly modified and the rest of the installation is correct
                rk.SetValue("library path 2", m_install_directory + "Libraries");
                rk.SetValue("data path 2", m_data_directory);
            }

            WriteFileBinaries.WriteMainApp(m_install_directory, PROGRAM_EXECUTABLE);
            WriteFileBinaries.WriteAppDlls(m_install_directory);
            if (m_create_desktop_shortcut)
            {
                WriteFileBinaries.CreateMainShortcut(m_install_directory, PROGRAM_EXECUTABLE);
            }

            if (m_create_start_menu_shortcut)
            {
                WriteFileBinaries.CreateStartMenuShortcut(m_install_directory, PROGRAM_EXECUTABLE);
            }

            string libraryDirectory = m_install_directory + "\\Libraries";
            System.IO.Directory.CreateDirectory(libraryDirectory);
            WriteFileBinaries.WriteLibraryDlls(libraryDirectory, m_libraryFiles);
        }
        static void Abort()
        {
            MessageBox.Show("There was an issue installing the program");
            // TODO: Do any necessary cleanup
            Environment.Exit(0);
        }
    }
}
