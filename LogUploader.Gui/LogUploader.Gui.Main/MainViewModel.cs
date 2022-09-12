using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using LogUploader.Gui.Shared;
using LogUploader.Injection;
using LogUploader.Localization;

using MaterialDesignThemes.Wpf;

using IServiceProvider = LogUploader.Injection.IServiceProvider;

namespace LogUploader.Gui.Main
{
    public class MainWindowBase : ViewBase<MainWindowBase, MainViewModel, MainController, MainStringProvider>
    {
        protected void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 2)
                {
                    ((MainViewModel)DataContext).Controller.ToggleWindowStateCommand?.Execute(null);
                }
                DragMove();
            }
        }
    }

    public class MainViewModel : ViewModelBase<MainWindowBase, MainViewModel, MainController, MainStringProvider>, IMainView
    {
        private WindowState windowState = WindowState.Normal;
        private PackIconKind maximizeIcon = PackIconKind.WindowMaximize;

        public WindowState WindowState { get => windowState;
            set
            {   
                SetProperty(ref windowState, value);
                MaximizeIcon = value switch
                {
                    WindowState.Normal => PackIconKind.WindowMaximize,
                    WindowState.Maximized => PackIconKind.WindowRestore,
                    _ => MaximizeIcon,
                };
            }
        }
        public PackIconKind MaximizeIcon { get => maximizeIcon; set => SetProperty(ref maximizeIcon, value); }

        public ObservableCollection<DisplayLog> LogData { get; set; } = new ObservableCollection<DisplayLog>();

        public MainViewModel() : base(null, new MainController(), new MainStringProvider(new DummyLocalisation()))
        {
            LogData.Add(new DisplayLog("Log1", true, 0, false, TimeSpan.FromSeconds(325), DateTime.Now, new List<Profession>() { Profession.Scourge, Profession.Heral, Profession.Mechanist, Profession.Scourge }));
            LogData.Add(new DisplayLog("Log2", false, 42.1f, true, TimeSpan.FromSeconds(66), DateTime.Now, new List<Profession>() { Profession.Scourge, Profession.Scourge, Profession.Mechanist, Profession.Scourge }));
            LogData.Add(new DisplayLog("Log3", true, 0, true, TimeSpan.FromSeconds(256), DateTime.Now, new List<Profession>() { Profession.Scourge, Profession.Heral, Profession.Mechanist, Profession.Scourge }));
            LogData.Add(new DisplayLog("Log4", false, 26.128f, false, TimeSpan.FromSeconds(468), DateTime.Now, new List<Profession>() { Profession.Mechanist, Profession.Heral, Profession.Mechanist, Profession.Mechanist, Profession.Mechanist }));
        }

        public MainViewModel(MainWindowBase view, MainController controller, MainStringProvider stringProvider) : base(view, controller, stringProvider)
        {
            LogData.Add(new DisplayLog("Log1", true, 0, false, TimeSpan.FromSeconds(325), DateTime.Now, new List<Profession>() { Profession.Scourge, Profession.Heral, Profession.Mechanist, Profession.Scourge }));
            LogData.Add(new DisplayLog("Log2", false, 42.1f, true, TimeSpan.FromSeconds(66), DateTime.Now, new List<Profession>() { Profession.Scourge, Profession.Scourge, Profession.Mechanist, Profession.Scourge }));
            LogData.Add(new DisplayLog("Log3", true, 0, true, TimeSpan.FromSeconds(256), DateTime.Now, new List<Profession>() { Profession.Scourge, Profession.Heral, Profession.Mechanist, Profession.Scourge }));
            LogData.Add(new DisplayLog("Log4", false, 26.128f, false, TimeSpan.FromSeconds(468), DateTime.Now, new List<Profession>() { Profession.Mechanist, Profession.Heral, Profession.Mechanist, Profession.Mechanist, Profession.Mechanist }));
        }

        public void Run()
        {
            var app = new Application();

            app.Run(View);
        }

        public class DisplayLog : NotifyPropertyChangedBase
        {
            private string encounter;
            private bool success;
            private float hpLeft;
            private bool cM;
            private TimeSpan duration;
            private DateTime date;
            private ObservableCollection<DisplayProfession> professions;

            public DisplayLog(string encounter, bool success, float hpLeft, bool cM, TimeSpan duration, DateTime date, IEnumerable<Profession> professions)
            {
                this.encounter = encounter;
                this.success = success;
                this.hpLeft = hpLeft;
                this.cM = cM;
                this.duration = duration;
                this.date = date;
                this.professions = new ObservableCollection<DisplayProfession>(professions.OrderBy(p => (int)p).Select(p => ProfessionMap[p]));
            }

            private Dictionary<Profession, DisplayProfession> ProfessionMap = new Dictionary<Profession, DisplayProfession>()
            {
                { Profession.Scourge, new DisplayProfession() { ToolTip="Scourge", Icon=new BitmapImage(new Uri(@"C:\Users\shube\source\repos\LogUploader2_2\LogUploader\images\Professions\Scourge.png")) } },
                { Profession.Heral, new DisplayProfession() { ToolTip="Herald", Icon=new BitmapImage(new Uri(@"C:\Users\shube\source\repos\LogUploader2_2\LogUploader\images\Professions\Herald.png")) } },
                { Profession.Mechanist, new DisplayProfession() { ToolTip="Mechanist", Icon=new BitmapImage(new Uri(@"C:\Users\shube\source\repos\LogUploader2_2\LogUploader\images\Professions\Mechanist.png")) } }
            };


            public string Encounter { get => encounter; set => SetProperty(ref encounter, value); }
            private bool Success
            {
                get => success;
                set
                {
                    success = value;
                    UpdateResult(value, HpLeft);
                }
            }
            private float HpLeft
            {
                get => hpLeft;
                set
                {
                    hpLeft = value;
                    UpdateResult(Success, value);
                }
            }
            private void UpdateResult(bool success, float hpLeft)
            {
                if (success)
                {
                    Result = "Kill";
                }
                Result = $"Fail ({hpLeft:0.00}%)";
            }
            public string Result { get => encounter; set => SetProperty(ref encounter, value); }
            public bool CM { get => cM; set => SetProperty(ref cM, value); }
            public TimeSpan Duration { get => duration; set => SetProperty(ref duration, value); }
            public DateTime Date { get => date; set => SetProperty(ref date, value); }
            public ObservableCollection<DisplayProfession> Professions { get => professions; set => SetProperty(ref professions, value); }
        }

        public enum Profession
        {
            Scourge,
            Heral,
            Mechanist,
        }

        public class DisplayProfession
        {
            public string ToolTip { get; init; }
            public ImageSource Icon { get; init; }
        }
    }

    public interface IMainView
    {
        public void Run();
    }

    public class MainController : ControllerBase<MainWindowBase, MainViewModel, MainController, MainStringProvider>
    {
        public ICommand? MinimizeCommand { get; private set; }
        public ICommand? ToggleWindowStateCommand { get; private set; }
        public ICommand? CloseWindowCommand { get; private set; }

        public override void InitCommands(MainViewModel viewModel)
        {
            base.InitCommands(viewModel);
            MinimizeCommand = new ActionCommand(args => {
                viewModel.WindowState = WindowState.Minimized;
            });
            ToggleWindowStateCommand = new ActionCommand(args => {
                viewModel.WindowState = viewModel.WindowState switch
                {
                    WindowState.Normal => WindowState.Maximized,
                    WindowState.Maximized => WindowState.Normal,
                    _ => viewModel.WindowState,
                };
            });
            CloseWindowCommand = new ActionCommand(args =>
            {
                viewModel.View.Close();
            });
        }
    }

    public class MainStringProvider : StringProviderBase<MainWindowBase, MainViewModel, MainController, MainStringProvider>
    {
        public MainStringProvider(ILocalisation localisation) : base(localisation, "MainWindow")
        {
        }

        public string Title { get => ResolveWithDefault("Title", "LogUploader"); }
        
        public string Menu_Log { get => ResolveWithDefault("Menu_Log", "Log"); }
        public string Menu_Tools { get => ResolveWithDefault("Menu_Tools", "Tools"); }
        public string Menu_Settings { get => ResolveWithDefault("Menu_Settings", "Settings"); }
        public string Menu_About { get => ResolveWithDefault("Menu_About", "About"); }
        
        public string Details_Header { get => ResolveWithDefault("Details_Header", "Details"); }
        
        public string Filter_Header { get => ResolveWithDefault("Details_Header", "Filter"); }
        
        public string QuickActions_Header { get => ResolveWithDefault("QuickActions_Header", "Quick Actions"); }

        public string Grid_Boss_Header { get => ResolveWithDefault("Grid_Boss_Header", "Encounter"); }
        public string Grid_Result_Header { get => ResolveWithDefault("Grid_Result_Header", "Result"); }
        public string Grid_CM_Header { get => ResolveWithDefault("Grid_CM_Header", "CM"); }
        public string Grid_Date_Header { get => ResolveWithDefault("Grid_Date_Header", "Date"); }
        public string Grid_Duration_Header { get => ResolveWithDefault("Grid_Duration_Header", "Duration"); }
        public string Grid_Players_Header { get => ResolveWithDefault("Grid_Players_Header", "Payers"); }

        public string Footer_LogsTotal { get => ResolveWithDefault("Footer_LogsTotal", "Logs"); }
        public string Footer_LogsSelected { get => ResolveWithDefault("Footer_LogsSelected", "Selected"); }
        public string Footer_TasksOf { get => ResolveWithDefault("Footer_TasksOf", "of"); }

    }

    [Service(ServiceType.Singelton)]
    public interface IMainWindowFactory
    {
        IMainView CreateMainView(IProgress<double> progress);
    }

    public class MainWindowFactory : IMainWindowFactory
    {
        private readonly IServiceProvider _serviceProvider;

        [InjectionConstructor]
        public MainWindowFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public IMainView CreateMainView(IProgress<double> progress)
        {
            progress.Report(0);
            var view = new MainWindow();
            progress.Report(0.25);
            var controller = new MainController();
            var stringProvider = new MainStringProvider(_serviceProvider.Create<ILocalisation>());
            progress.Report(0.5);
            var viewModel = new MainViewModel(view, controller, stringProvider);
            controller.InitCommands(viewModel);
            progress.Report(0.75);
            view.DataContext = viewModel;
            progress.Report(1);
            return viewModel;
        }
    }

    public class Registrator : IServiceRegistrator
    {
        public async Task Load(IServiceCollection serviceCollection)
        {
            await Task.Run(() =>
            {
                serviceCollection.Add<IMainWindowFactory, MainWindowFactory>();
            });
        }
    }
}