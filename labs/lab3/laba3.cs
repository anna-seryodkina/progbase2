using System;
using static System.Console;

namespace lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger logger;
            if(args.Length == 0 || args[0] == "console")
            {
                logger = new ConsoleLogger ();
            }
            else if(args[0] == "file")
            {
                if(args.Length < 3)
                {
                    WriteLine(">> file path is required.");
                    return;
                }
                logger = new PlainFileLogger(args[1], args[2]);
            }
            else
            {
                WriteLine(">> incorect args.");
                return;
            }
            //
            UI.ProcessSets(logger);
        }
    }
}
