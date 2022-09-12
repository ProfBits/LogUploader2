namespace LogUploader;

public class ProgressMessage : IProgressMessage
{
    public ProgressMessage(string message, double progress)
    {
        Message = message;
        Progress = progress;
    }

    public string Message { get; }
    public double Progress { get; }
}
