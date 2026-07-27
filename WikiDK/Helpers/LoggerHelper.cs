namespace WikiDK.Helpers
{
    public class LoggerHelper<T> (ILogger<T> logger)
    {
        public bool LogInformation(string message, params object?[] values)
        {
            if (logger != null && logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(message, values);
                return true;
            }
            return false;
        }
    }
}
