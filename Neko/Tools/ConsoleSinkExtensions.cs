using Serilog;
using Serilog.Configuration;

namespace Neko.Tools; 

public static class ConsoleSinkExtensions
{
    public static LoggerConfiguration GameConsole(
        this LoggerSinkConfiguration loggerConfiguration,
        IFormatProvider formatProvider = null)
    {
        return loggerConfiguration.Sink(new ConsoleSink(formatProvider));
    }
}