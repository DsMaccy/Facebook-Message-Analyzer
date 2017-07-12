using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace CustomInstaller
{
    class WriteFileBinaries
    {
        public static void WriteMainApp(string writeLocation, string programName)
        {
            writeBinaryFile(writeLocation + programName, Properties.Resources.Facebook_Message_Analyzer);
        }

        public static void CreateMainShortcut(string linkedFileLocation, string linkedFileName)
        {
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = 
                (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(
                    Environment.SpecialFolder.Desktop) + "\\" + linkedFileName);
            
            shortcut.Description = "Shortcut for main application";
            shortcut.TargetPath = linkedFileLocation + linkedFileName;
            shortcut.Save();
        }

        public static void CreateStartMenuShortcut(string linkedFileLocation, string linkedFileName)
        {
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut =
                (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(
                    Environment.SpecialFolder.StartMenu) + "\\" + linkedFileName);

            shortcut.Description = "Shortcut for main application";
            shortcut.TargetPath = linkedFileLocation + linkedFileName;
            shortcut.Save();
        }




        public static void WriteAppDlls(string writeLocation)
        {
            writeBinaryFile(writeLocation + "Facebook.dll", Properties.Resources.Facebook);
            writeBinaryFile(writeLocation + "ModuleInterface.dll", Properties.Resources.ModuleInterface);
        }

        public static void WriteLibraryDlls(string writeLocation, Dictionary<string, byte[]> binaries)
        {
            foreach (KeyValuePair<string, byte[]> file in binaries)
            {
                writeBinaryFile(writeLocation + file.Key, file.Value);
            }
        }

        private static void writeBinaryFile(string writeFile, byte[] binaryFile)
        {
            using (FileStream mainOut = File.Open(writeFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (BinaryWriter bw = new BinaryWriter(mainOut))
                {
                    bw.Write(binaryFile);
                }
            }
        }
    }
}
