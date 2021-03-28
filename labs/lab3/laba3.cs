using System;
using System.IO;
using static System.Console;

namespace lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger logger;
            if(args[0] == "file")
            {
                if(args[1] == null || args[2] == null)
                {
                    WriteLine("> file path is required.");
                    return;
                }
                logger = new PlainFileLogger(args[1], args[2]);
            }
            else
            {
                logger = new ConsoleLogger ();
            }
            UI.ProcessSets(logger);
        }
    }
}
