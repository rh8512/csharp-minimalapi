public class SimpleFileLoggerProvider : ILoggerProvider
{
    public static SimpleFileLoggerProvider Instance { get; } = new();

    public ILogger CreateLogger(string categoryName)
    {
        return new SimpleFileLogger();
    }

    void IDisposable.Dispose() { }
}