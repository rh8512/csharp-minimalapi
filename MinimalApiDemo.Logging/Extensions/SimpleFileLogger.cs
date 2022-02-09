using System;

public class SimpleFileLogger : ILogger
{
    public IDisposable BeginScope<TState>(TState state)=> default!;
    

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);

        if (string.IsNullOrEmpty(message) && exception is null) return;

        Console.WriteLine("My custom logger: " + message);

        using StreamWriter w = File.AppendText("logs.txt");

        w.WriteLine(message);
    }
}