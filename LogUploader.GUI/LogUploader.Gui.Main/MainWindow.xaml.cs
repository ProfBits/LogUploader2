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
        MaxHeight = SystemParameters.WorkArea.Bottom + 10;
    }
}
