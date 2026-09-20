//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using SalesManagement.Busniess;
//using Microsoft.Win32;


//namespace SalesManagement.GlobalClasses
//{
//    internal static  class clsGlobal
//    {
//        public static clsUser CurrentUser = null!;
//        //public static frmMain frmMdiMain = new frmMain(null!);
//        public static frmMain frmMdiMain = null!;

//        public static bool RememberUsernameAndPasswordToFile(string Username, string Password)
//        {

//            try
//            {
//                //this will get the current project directory folder.
//                string currentDirectory = System.IO.Directory.GetCurrentDirectory();


//                // Define the path to the text file where you want to save the data
//                string filePath = currentDirectory + "\\data.txt";

//                //incase the username is empty, delete the file
//                if (Username == "" && File.Exists(filePath))
//                {
//                    File.Delete(filePath);
//                    return true;
//                }

//                // concatonate username and passwrod withe seperator.
//                string dataToSave = Username + "#//#" + Password;

//                // Create a StreamWriter to write to the file
//                using (StreamWriter writer = new StreamWriter(filePath))
//                {
//                    // Write the data to the file
//                    writer.WriteLine(dataToSave);

//                    return true;
//                }

//            }
//            catch (Exception ex)
//            {
//                clsGlobal.LoggingErrorEvent($"An error occurred: {ex.Message}");
//                return false;
//            }

//        }

//        public static bool RememberUsernameAndPasswordToRegistry(string Username, string Password)
//        {
//            // Specify the Registry key and path
//            // string keyPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\SalesManagement";
//            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\SalesManagement";

//            if (Username == "")
//            {
//                try
//                {
//                    Registry.SetValue(keyPath, "Username", "", RegistryValueKind.String);
//                    Registry.SetValue(keyPath, "Password", "", RegistryValueKind.String);

//                    Console.WriteLine($"Value Username and Password successfully written to the Registry.");
//                }
//                catch (Exception ex)
//                {
//                    clsGlobal.LoggingErrorEvent($"An error occurred: {ex.Message}");
//                    return false;
//                }
//                return false;
//            }

//            try
//            {
//                Registry.SetValue(keyPath, "Username", Username, RegistryValueKind.String);
//                Registry.SetValue(keyPath, "Password", Password, RegistryValueKind.String);

//                Console.WriteLine($"Value Username and Password successfully written to the Registry.");
//            }
//            catch (Exception ex)
//            {
//                clsGlobal.LoggingErrorEvent($"An error occurred: {ex.Message}");
//                return false;
//            }

//            return true;
//        }

//        public static bool GetStoredCredentialFromFile(ref string Username, ref string Password)
//        {
//            //this will get the stored username and password and will return true if found and false if not found.
//            try
//            {
//                //gets the current project's directory
//                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

//                // Path for the file that contains the credential.
//                string filePath = currentDirectory + "\\data.txt";

//                // Check if the file exists before attempting to read it
//                if (File.Exists(filePath))
//                {
//                    // Create a StreamReader to read from the file
//                    using (StreamReader reader = new StreamReader(filePath))
//                    {
//                        // Read data line by line until the end of the file
//                        string line;
//                        while ((line = reader.ReadLine()) != null)
//                        {
//                            Console.WriteLine(line); // Output each line of data to the console
//                            string[] result = line.Split(new string[] { "#//#" }, StringSplitOptions.None);

//                            Username = result[0];
//                            Password = result[1];
//                        }
//                        return true;
//                    }
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                clsGlobal.LoggingErrorEvent($"An error occurred: {ex.Message}");
//                return false;
//            }

//        }

//        public static bool GetStoredCredentialFromRegistry(ref string Username, ref string Password)
//        {

//            // Specify the Registry key and path
//            // string keyPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\SalesManagement";
//            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\SalesManagement";

//            try
//            {


//                // Read the value from the Registry
//                Username = Registry.GetValue(keyPath, "Username", "") as string;
//                Password = Registry.GetValue(keyPath, "Password", "") as string;


//                if (Username == "")
//                    return false;

//            }
//            catch (Exception ex)
//            {
//                clsGlobal.LoggingErrorEvent($"An error occurred: {ex.Message}");
//                return false;
//            }

//            return true;

//        }


//        public static void LoggingInformationEvent(string InformationMessage)
//        {
//            // Specify the source name for the event log
//            string sourceName = "SalesManagement";


//            // Create the event source if it does not exist
//            if (!EventLog.SourceExists(sourceName))
//            {
//                EventLog.CreateEventSource(sourceName, "Application");
//                Console.WriteLine("Event source created.");
//            }


//            // Log an information event
//            EventLog.WriteEntry(sourceName, InformationMessage, EventLogEntryType.Information);

//        }

//        public static void LoggingWarningEvent(string WarningMessage)
//        {
//            // Specify the source name for the event log
//            string sourceName = "SalesManagement";


//            // Create the event source if it does not exist
//            if (!EventLog.SourceExists(sourceName))
//            {
//                EventLog.CreateEventSource(sourceName, "Application");
//                Console.WriteLine("Event source created.");
//            }

//            // Log a Warning event
//            EventLog.WriteEntry(sourceName, WarningMessage, EventLogEntryType.Warning);

//        }

//        public static void LoggingErrorEvent(string ErrorMessage)
//        {
//            // Specify the source name for the event log
//            string sourceName = "SalesManagement";


//            // Create the event source if it does not exist
//            if (!EventLog.SourceExists(sourceName))
//            {
//                EventLog.CreateEventSource(sourceName, "Application");
//                Console.WriteLine("Event source created.");
//            }

//            // Log an Error event
//            EventLog.WriteEntry(sourceName, ErrorMessage, EventLogEntryType.Error);

//        }

//        public static void _OpenForm(System.Windows.Forms.Form form)
//        {
//            foreach (System.Windows.Forms.Form frm in Application.OpenForms)
//            {
//                if (frm.Name == "frmLogin" || frm.Name == "frmMain") continue;
//                if (frm.Name == form.Name)
//                {
//                    frm.Activate(); // Focus
//                    //frm.BringToFront();
//                    frm.WindowState = FormWindowState.Normal;
//                    return;
//                }
//            }

//            form.Icon = frmMdiMain.Icon;
//            form.MdiParent = frmMdiMain;
//            form.Show();
//            //form.ShowDialog();
//        }

//    }
//}




using Microsoft.Win32;
using SalesManagement.Busniess;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SalesManagement.GlobalClasses
{
    internal static class clsGlobal
    {
        public static clsUser CurrentUser = null!;
        public static frmMain frmMdiMain = null!;

        private const string _separator = "#//#";
        private const string _registryPath = @"HKEY_CURRENT_USER\SOFTWARE\SalesManagement";
        private const string _eventSourceName = "SalesManagement";

        // ── Credentials — File ─────────────────────────────────────────────────────

        public static bool RememberUsernameAndPasswordToFile(string username, string password)
        {
            try
            {
                string filePath = _GetCredentialFilePath();

                // If username is empty — delete the saved credentials file
                if (string.IsNullOrEmpty(username))
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    return true;
                }

                using StreamWriter writer = new StreamWriter(filePath);
                writer.WriteLine(username + _separator + password);
                return true;
            }
            catch (Exception ex)
            {
                LoggingErrorEvent($"RememberToFile failed: {ex.Message}");
                return false;
            }
        }

        public static bool GetStoredCredentialFromFile(ref string username, ref string password)
        {
            try
            {
                string filePath = _GetCredentialFilePath();

                if (!File.Exists(filePath))
                    return false;

                using StreamReader reader = new StreamReader(filePath);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(
                        new string[] { _separator },
                        StringSplitOptions.None);

                    if (parts.Length < 2)
                        return false;

                    username = parts[0];
                    password = parts[1];
                }

                return true;
            }
            catch (Exception ex)
            {
                LoggingErrorEvent($"GetCredentialFromFile failed: {ex.Message}");
                return false;
            }
        }

        // Returns the full path of the credentials file next to the executable
        private static string _GetCredentialFilePath()
            => Path.Combine(Directory.GetCurrentDirectory(), "data.txt");

        // ── Credentials — Registry ─────────────────────────────────────────────────

        public static bool RememberUsernameAndPasswordToRegistry(string username, string password)
        {
            try
            {
                // Write empty strings to clear saved credentials when username is empty
                Registry.SetValue(_registryPath, "Username", username, RegistryValueKind.String);
                Registry.SetValue(_registryPath, "Password", string.IsNullOrEmpty(username) ? "" : password, RegistryValueKind.String);

                return !string.IsNullOrEmpty(username);
            }
            catch (Exception ex)
            {
                LoggingErrorEvent($"RememberToRegistry failed: {ex.Message}");
                return false;
            }
        }

        public static bool GetStoredCredentialFromRegistry(ref string username, ref string password)
        {
            try
            {
                username = Registry.GetValue(_registryPath, "Username", "") as string ?? "";
                password = Registry.GetValue(_registryPath, "Password", "") as string ?? "";

                return !string.IsNullOrEmpty(username);
            }
            catch (Exception ex)
            {
                LoggingErrorEvent($"GetCredentialFromRegistry failed: {ex.Message}");
                return false;
            }
        }

        // ── Event Logging ──────────────────────────────────────────────────────────

        public static void LoggingInformationEvent(string message)
            => _WriteEventLog(message, EventLogEntryType.Information);

        public static void LoggingWarningEvent(string message)
            => _WriteEventLog(message, EventLogEntryType.Warning);

        public static void LoggingErrorEvent(string message)
            => _WriteEventLog(message, EventLogEntryType.Error);

        // Single method handles all event log types — avoids repeating the same
        // SourceExists check and WriteEntry call in every logging method
        private static void _WriteEventLog(string message, EventLogEntryType entryType)
        {
            try
            {
                if (!EventLog.SourceExists(_eventSourceName))
                    EventLog.CreateEventSource(_eventSourceName, "Application");

                EventLog.WriteEntry(_eventSourceName, message, entryType);
            }
            catch
            {
                // Swallow logging failures silently — logging should never crash the app
            }
        }

        // ── Form Management ────────────────────────────────────────────────────────

        public static void _OpenForm(Form form)
        {
            // If the form is already open — focus it instead of opening a new instance
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm.Name == "frmLogin" || openForm.Name == "frmMain")
                    continue;

                if (openForm.Name == form.Name)
                {
                    openForm.WindowState = FormWindowState.Normal;
                    openForm.Activate();
                    return;
                }
            }

            form.Icon = frmMdiMain.Icon;
            form.MdiParent = frmMdiMain;
            form.Show();
        }
    }
}
