using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using LogUploader;

namespace LogUploader.Gui.LoadingScreen;

internal class SplashScreenViewModel : ISplashScreenViewModel
{
    private string currentProgressText;
    private double currentProgress;
    private readonly Window window;
    private readonly CancellationTokenSource cts = new CancellationTokenSource();
    private readonly IProgress<IProgressMessage> progressHandler;

    public SplashScreenViewModel() : this(new SplashScreen())
    {
        currentProgress = 0.3;
    }

    public SplashScreenViewModel(Window window)
    {
        currentProgress = 0;
        currentProgressText = "Loading...";
        CloseCommand = new CustomCloseCommand(() =>
        {
            cts.Cancel();
            currentProgressText = "Aborting...";
            OnPropertyChanged(nameof(CurrentProgressText));

            Close();
        });
        progressHandler = new SynchronusProgress<IProgressMessage>(msg =>
        {
            CurrentProgressText = msg.Message;
            CurrentProgress = msg.Progress;
        });
        this.window = window;
        this.window.Closed += (_, _) => window.Dispatcher.InvokeShutdown();

    }

    public string Edition { get; }
#if DEBUG
        = "DEBUG";
#else
    = "";
#endif

    public string CurrentProgressText
    {
        get => currentProgressText; private set
        {
            if (!cts.IsCancellationRequested)
                SetProperty(ref currentProgressText, value);
        }
    }
    public double CurrentProgress
    {
        get => currentProgress; private set
        {
            if (!cts.IsCancellationRequested)
                SetProperty(ref currentProgress, value);
        }
    }
    public ICommand CloseCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void SetProperty<T>(ref T property, T newValue, [CallerMemberName] string cmn = "")
    {
        property = newValue;
        OnPropertyChanged(cmn);
    }

    protected void OnPropertyChanged(string cmn)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(cmn));
    }

    public void Show() => window.Show();
    public void Close() => window.Dispatcher.Invoke(window.Close);
    public Task Run(Func<IProgress<IProgressMessage>, CancellationToken, Task> callback)
    {
        return callback(progressHandler, cts.Token);
    }
}

internal class CustomCloseCommand : ICommand
{
    private Action callback;

    public CustomCloseCommand(Action value)
    {
        this.callback = value;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        callback();
    }
}