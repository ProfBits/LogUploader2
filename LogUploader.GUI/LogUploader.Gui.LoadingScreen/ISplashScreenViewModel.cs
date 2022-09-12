using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace LogUploader.Gui.LoadingScreen;

public interface ISplashScreenViewModel : INotifyPropertyChanged
{
    void Close();
    Task Run(Func<IProgress<IProgressMessage>, CancellationToken, Task> callback);
    void Show();
}