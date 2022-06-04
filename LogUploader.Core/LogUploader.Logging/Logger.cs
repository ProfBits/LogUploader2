using System.Runtime.CompilerServices;

using LogUploader.Injection;

using Serilog;
using Serilog.Events;

namespace LogUploader.Logging;

internal class Logger : ILogger
{
    private readonly Serilog.ILogger _logger;
    
    private const string MessagePrefix = "{CallerFile}{CallerMember}{CallerLine} ";

    [InjectionConstructor]
    private Logger() { }

    private Logger(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    private static object?[] PrefixCalerPropertyValues(object?[] orginalParameters, string cfp, string cmn, int cln)
    {
        if (orginalParameters == null)
        {
            return new object[] {cfp, cmn, cln};
        }
        else
        {
            var newParameters = new object?[orginalParameters.Length + 3];
            newParameters[0] = cfp;
            newParameters[1] = cmn;
            newParameters[2] = cln;
            Array.Copy(orginalParameters, 0, newParameters, 3, orginalParameters.Length);
            return newParameters;
        }
    }

    public void Verbose(string message, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Verbose(MessagePrefix + message, new object[] { cfp, cmn, cln });
    }

    public void Verbose(string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Verbose(MessagePrefix + messageTemplate, PrefixCalerPropertyValues(propertyValues, cfp, cmn, cln));
    }

    public void Verbose(Exception exception, string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Verbose(exception, MessagePrefix + messageTemplate, PrefixCalerPropertyValues(propertyValues, cfp, cmn, cln));
    }


    public void Debug(string message, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Debug(MessagePrefix + message, new object[] { cfp, cmn, cln });
    }

    public void Debug(string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Debug(MessagePrefix + messageTemplate, PrefixCalerPropertyValues(propertyValues, cfp, cmn, cln));
    }

    public void Debug(Exception exception, string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Debug(exception, MessagePrefix + messageTemplate, PrefixCalerPropertyValues(propertyValues, cfp, cmn, cln));
    }


    public void Information(string message, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Information(MessagePrefix + message, new object[] { cfp, cmn, cln });
    }

    public void Information(string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Information(MessagePrefix + messageTemplate, PrefixCalerPropertyValues(propertyValues, cfp, cmn, cln));
    }

    public void Information(Exception exception, string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Information(exception, MessagePrefix + messageTemplate, PrefixCalerPropertyValues(propertyValues, cfp, cmn, cln));
    }


    public void Warning(string message, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Warning(MessagePrefix + message, new object[] { cfp, cmn, cln });
    }

    public void Warning(string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Warning(MessagePrefix + messageTemplate, PrefixCalerPropertyValues(propertyValues, cfp, cmn, cln));
    }

    public void Warning(Exception exception, string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Warning(exception, MessagePrefix + messageTemplate, PrefixCalerPropertyValues(propertyValues, cfp, cmn, cln));
    }


    public void Error(string message, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Error(MessagePrefix + message, new object[] { cfp, cmn, cln });
    }

    public void Error(string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Error(MessagePrefix + messageTemplate, PrefixCalerPropertyValues(propertyValues, cfp, cmn, cln));
    }

    public void Error(Exception exception, string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Error(exception, MessagePrefix + messageTemplate, PrefixCalerPropertyValues(propertyValues, cfp, cmn, cln));
    }


    public void Fatal(string message, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Fatal(MessagePrefix + message, new object[] { cfp, cmn, cln });
    }

    public void Fatal(string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Fatal(MessagePrefix + messageTemplate, PrefixCalerPropertyValues(propertyValues, cfp, cmn, cln));
    }

    public void Fatal(Exception exception, string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1)
    {
        _logger.Fatal(exception, MessagePrefix + messageTemplate, PrefixCalerPropertyValues(propertyValues, cfp, cmn, cln));
    }
}
