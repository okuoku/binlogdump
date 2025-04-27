using System;
using System.Collections;
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

        var jsonopt = new JsonSerializerOptions{ WriteIndented = true };

        // Start JSONL
        Console.WriteLine("[");

        // Dump Project Ids
        var slog = BinaryLog.ReadBuild(binLogFilePath);
        foreach(var p in slog.FindChildrenRecursive<Project>()){
            var outproj = new { proj = p.SourceFilePath, id = p.Id };
            if(first){
                first = false;
            }else{
                Console.WriteLine(",");
            }
            Console.WriteLine(JsonSerializer.Serialize(outproj));
        }

        // Dump command lines
        foreach (var record in binLogReader.ReadRecords(binLogFilePath))
        {
            var buildEventArgs = record.Args;

            // Dump task events
            if (buildEventArgs is ProjectEvaluationFinishedEventArgs e) {
                var dict = new Dictionary<string, object>();
                foreach (DictionaryEntry de in e.Items){
                    dict[de.Key.ToString()] = de.Value;
                }
                var outtask = new { proj = e.ProjectFile, q = "evalfinished", o = dict };
                if(first){
                    first = false;
                }else{
                    Console.WriteLine(",");
                }

                Console.WriteLine(JsonSerializer.Serialize(outtask, jsonopt));
            }

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

        // End JSONL
        Console.WriteLine("]");
    }
}
