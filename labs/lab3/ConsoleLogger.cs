using System;

namespace lab3
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($">> {message}");
        }

        public void LogError(string errorMessage)
        {
            Console.WriteLine($">> error: {errorMessage}");
        }
    }
}