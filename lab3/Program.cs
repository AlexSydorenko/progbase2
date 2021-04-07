using System;
using System.IO;

namespace lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            // ConsoleLogger: dotnet run console
            // ChunkPlainFileLogger: dotnet run file {directory} {maxNumOfLines}
            
            ILogger logger = null;
            if (args.Length == 0)
            {
                logger = new ConsoleLogger();
            }
            else if (args.Length == 1)
            {
                if (args[0] != "console")
                {
                    throw new Exception($"ERROR! Command line argument should be `console`, but not `{args[0]}`");
                }
                logger = new ConsoleLogger();
            }
            else if (args.Length == 3)
            {
                if (args[0] != "file")
                {
                    throw new Exception($"ERROR! 2nd argument should be `file`, but have `{args[0]}`");
                }
                if (!Directory.Exists(args[1]))
                {
                    throw new Exception($"ERROR! It is non-existing directory: `{args[1]}`!");
                }
                int maxNumOfLines = 0;
                if (!int.TryParse(args[2], out maxNumOfLines))
                {
                    throw new Exception("ERROR! Max number of lines in file is positive integer!");
                }
                if (maxNumOfLines <= 0)
                {
                    throw new Exception("ERROR! Max number of lines in file is positive integer!");
                }
                logger = new ChunkPlainFileLogger(args[1], maxNumOfLines);
            }
            else
            {
                throw new Exception($"ERROR! Incorrect number of command line arguments! Should be `1` or `3`, but have `{args.Length}`");
            }
            // ILogger logger = new ChunkPlainFileLogger("/home/alex/projects/progbase2/lab3/fileLogger/", 10);
            CommandUserInterface cui = new CommandUserInterface();
            cui.ProcessSets(logger);
        }
    }
}
