using System;
using System.Text.Json;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.Build.Logging.StructuredLogger;

class MyBinLogReader
{
    static void Main(string[] args)
    {
        string binLogFilePath = args[0];

        bool first = true;
        var binLogReader = new BinLogReader();
        Console.WriteLine("[");
        foreach (var record in binLogReader.ReadRecords(binLogFilePath))
        {
            var buildEventArgs = record.Args;

            // print command lines of all tool tasks such as Csc
            if (buildEventArgs is TaskCommandLineEventArgs taskCommandLine)
            {
                if(first){
                    first = false;
                }else{
                    Console.WriteLine(",");
                }

                Console.WriteLine(JsonSerializer.Serialize(buildEventArgs));
            }
        }
        Console.WriteLine("]");
    }
}
