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
            StreamWriter sw = new StreamWriter(this.filePathMessage);
            sw.WriteLine(message);
            sw.WriteLine();
            sw.Close();
        }

        public void LogError(string errorMessage)
        {
            StreamWriter sw = new StreamWriter(this.filePathError);
            sw.WriteLine(errorMessage);
            sw.WriteLine();
            sw.Close();
        }
    }
}