using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.Build.Logging.StructuredLogger;

class MyBinLogReader
{
    static void Main(string[] args)
    {
        string binLogFilePath = args[0];

        var binLogReader = new BinLogReader();
        foreach (var record in binLogReader.ReadRecords(binLogFilePath))
        {
            var buildEventArgs = record.Args;

            // print command lines of all tool tasks such as Csc
            if (buildEventArgs is TaskCommandLineEventArgs taskCommandLine)
            {
                Console.WriteLine(taskCommandLine.CommandLine);
            }
        }
    }
}
