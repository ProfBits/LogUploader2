using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LogUploader.Gui.Shared;

namespace LogUploader.Gui.Main;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
internal partial class MainWindow : MainWindowBase
{
    public MainWindow()
    {
        InitializeComponent();
    }
}

internal abstract class MainWindowBase : ViewBase<MainWindowBase, MainWindowViewModel, MainWindowController, MainWindowStrings>
{

}


internal class MainWindowViewModel : ViewModelBase<MainWindowBase, MainWindowViewModel, MainWindowController, MainWindowStrings>
{
    public MainWindowViewModel()
        : this(null, null, null)
    {

    }

    public MainWindowViewModel(MainWindowBase view, MainWindowController controller, MainWindowStrings stringProvider) : base(view, controller, stringProvider)
    {
    }
}

internal class MainWindowController : ControllerBase<MainWindowBase, MainWindowViewModel, MainWindowController, MainWindowStrings>
{
}

internal class MainWindowStrings : StringProviderBase<MainWindowBase, MainWindowViewModel, MainWindowController, MainWindowStrings>
{

}

public static class MainWindowFactory
{
    public static Window CreateMainWindow()
    {
        var view = new MainWindow();
        var controller = new MainWindowController();
        var strings = new MainWindowStrings();
        var viewModel = new MainWindowViewModel(view, controller, strings);
        view.DataContext = viewModel;
        return view;
    }

    public static void Run()
    {
        var app = new Application();
        
        app.Run(CreateMainWindow());
    }
}
