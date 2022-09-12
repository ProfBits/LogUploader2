namespace LogUploader;

public class SynchronusProgress<T> : IProgress<T>
{
    private readonly Action<T> callback;

    public SynchronusProgress(Action<T> callback)
    {
        this.callback = callback;
    }

    public void Report(T value) => callback(value);
}
