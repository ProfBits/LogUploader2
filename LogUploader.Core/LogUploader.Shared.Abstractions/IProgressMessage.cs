namespace LogUploader;

public interface IProgressMessage
{
    string Message { get; }
    double Progress { get; }
}
