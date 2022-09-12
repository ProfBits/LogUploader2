using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;
using LogUploader.Localization;
using System.Windows.Input;

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
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = (newValue);
                OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        public TViewModelBase? ViewModel { get; private set; }

        public virtual void InitCommands(TViewModelBase viewModel)
        {
            ViewModel = viewModel;
        }
    }

    public abstract class StringProviderBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TViewBase : ViewBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TViewModelBase : ViewModelBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TControllerBase : ControllerBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
        where TStringProvider : StringProviderBase<TViewBase, TViewModelBase, TControllerBase, TStringProvider>
    {
        private readonly ILocalStrings _localisation;
        private readonly string _section;

        protected StringProviderBase(ILocalisation localisation, string section)
        {
            _localisation = localisation.Strings;
            _section = section;
            localisation.LocalisationChanged += (_, _) => OnLanguageChanged();
        }

        protected string ResolveWithDefault(string key, string fallback)
        {
            return _localisation.ResolveStringOrDefault(_section, key, fallback);
        }

        protected string ResolveWithDefault(string section, string key, string fallback)
        {
            return _localisation.ResolveStringOrDefault(section, key, fallback);
        }

        public event LanguageChangedHandler? LanguageChanged;

        private void OnLanguageChanged()
        {
            LanguageChanged?.Invoke(this, new EventArgs());
        }
    }

    public delegate void LanguageChangedHandler(object sender, EventArgs e);

    public class ActionCommand : ICommand
    {
        private readonly Action<object?> _exectueCallback;

        public ActionCommand(Action<object?> exectueCallback)
        {
            _exectueCallback = exectueCallback;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _exectueCallback(parameter);
        }
    }

    public class DummyLocalisation : ILocalisation
    {
        public ILocalStrings Strings { get; } = new DummyLocalStrings();

        public event LocalisationChangedHandler? LocalisationChanged;
    }

    public class DummyLocalStrings : ILocalStrings
    {
        public string ResolveStringOrDefault(string section, string key, string defaultValue)
        {
            return defaultValue;
        }
    }
}
