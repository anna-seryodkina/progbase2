using System.IO;

namespace lab3
{
    public class PlainFileLogger : ILogger
    {
        public string filePathMessage;
        public string filePathError;

        public PlainFileLogger(string filePath1, string filePath2)
        {
            this.filePathMessage = filePath1;
            this.filePathError = filePath2;
        }

        public void Log(string message)
        {
            File.AppendAllLines(this.filePathMessage, new string[] {message});
        }

        public void LogError(string errorMessage)
        {
            File.AppendAllLines(this.filePathError, new string[] {errorMessage});
        }
    }
}