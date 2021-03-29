namespace lab3
{
    public interface ILogger
    {
        void Log(string message); 
        void LogError(string errorMessage);
    }
}