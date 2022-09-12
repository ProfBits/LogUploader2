using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogUploader.Gui.LoadingScreen;

public static class LoadingScreenFactory
{
    public static ISplashScreenViewModel CreateSplashScreen()
    {
        var splashScreen = new SplashScreen();
        var splashScreenViewModel = new SplashScreenViewModel(splashScreen);
        splashScreen.DataContext = splashScreenViewModel;
        return splashScreenViewModel;
    } 
}
