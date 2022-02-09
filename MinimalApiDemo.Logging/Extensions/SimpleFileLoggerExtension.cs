using Microsoft.Extensions.Logging;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder)
    {
        builder.AddProvider(SimpleFileLoggerProvider.Instance);

        return builder;
    }
}
