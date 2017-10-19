namespace JiongXiaGu.Unity
{
    public interface ILogger
    {
        void Log(string message);
        void LogSuccessful(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}
