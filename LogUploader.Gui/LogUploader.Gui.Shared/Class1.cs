using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace LogUploader.Gui.Shared
{
    public abstract class ViewBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider> : Window
        where TViewBase : ViewBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TViewModelBase : ViewModelBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TControllerBase : ControllerBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TStringProvider : StringProviderBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
    {
    }

    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public abstract class ViewModelBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider> : NotifyPropertyChangedBase
        where TViewBase : ViewBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TViewModelBase : ViewModelBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TControllerBase : ControllerBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TStringProvider : StringProviderBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
    {
        public ViewModelBase(TViewBase view, TControllerBase controller, TStringProvider stringProvider)
        {
            View = view;
            Controller = controller;
            StringProvider = stringProvider;

            stringProvider.LanguageChanged += (_, _) => OnPropertyChanged(nameof(StringProvider));
        }

        public TViewBase View { get; }
        public TControllerBase Controller { get; }
        public TStringProvider StringProvider { get; }
    }

    public abstract class ControllerBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider> : NotifyPropertyChangedBase
        where TViewBase : ViewBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TViewModelBase : ViewModelBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TControllerBase : ControllerBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TStringProvider : StringProviderBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
    {
        public TViewModelBase? ViewModel { get; internal set; }
    }

    public abstract class StringProviderBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TViewBase : ViewBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TViewModelBase : ViewModelBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TControllerBase : ControllerBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TStringProvider : StringProviderBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
    {

        public event LanguageChangedHandler? LanguageChanged;
    }

    public delegate void LanguageChangedHandler(EventArgs e, object sender );
}
