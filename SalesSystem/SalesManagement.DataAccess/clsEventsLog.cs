using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SalesManagement.DataAccess
{
    internal static class clsEventsLog
    {
        public static void LoggingInformationEvent(string InformationMessage)
        {
            // Specify the source name for the event log
            string sourceName = "SalesManagement";


            // Create the event source if it does not exist
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "Application");
                Console.WriteLine("Event source created.");
            }


            // Log an information event
            EventLog.WriteEntry(sourceName, InformationMessage, EventLogEntryType.Information);

        }

        public static void LoggingWarningEvent(string WarningMessage)
        {
            // Specify the source name for the event log
            string sourceName = "SalesManagement";


            // Create the event source if it does not exist
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "Application");
                Console.WriteLine("Event source created.");
            }

            // Log a Warning event
            EventLog.WriteEntry(sourceName, WarningMessage, EventLogEntryType.Warning);

        }

        public static void LoggingErrorEvent(string ErrorMessage)
        {
            // Specify the source name for the event log
            string sourceName = "SalesManagement";


            // Create the event source if it does not exist
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "Application");
                Console.WriteLine("Event source created.");
            }

            // Log an Error event
            EventLog.WriteEntry(sourceName, ErrorMessage, EventLogEntryType.Error);

        }

    }
}
