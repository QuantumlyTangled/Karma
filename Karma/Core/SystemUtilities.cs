using System;
using System.IO;

namespace Karma.Core
{
    public class SystemUtilities
    {
        
        public static string GetSeparator { get; } = "-";
        
        public static void WriteToLogFile(string directory, string message)
        {
            using (var logfile = File.AppendText($"{directory}/{DateTime.Today.ToString("yyyy-MM-dd")}.log"))
            {
                logfile.WriteLine(message);
            }
        }
    }
}