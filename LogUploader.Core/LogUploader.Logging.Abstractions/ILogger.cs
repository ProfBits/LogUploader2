using LogUploader.Injection;

using System.Runtime.CompilerServices;

namespace LogUploader.Logging;

[Service(ServiceType.Singelton)]
public interface ILogger
{
    void Debug(string message, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Debug(string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Debug(Exception exception, string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Error(string message, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Error(string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Error(Exception exception, string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Fatal(string message, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Fatal(string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Fatal(Exception exception, string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Information(string message, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Information(string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Information(Exception exception, string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Verbose(string message, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Verbose(string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Verbose(Exception exception, string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Warning(string message, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Warning(string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
    void Warning(Exception exception, string messageTemplate, object?[] propertyValues, [CallerFilePath] string cfp = "", [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = -1);
}
