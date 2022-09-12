namespace LogUploader
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "main";

            //RunSplashScreen(args);
            Gui.Main.MainWindowFactory.Run();
            
        }

        private static void RunSplashScreen(string[] args)
        {
            Thread splashScreenThread = new Thread(() => SplashScreenThread(args));
            splashScreenThread.Name = "LaodingScreen";
            splashScreenThread.IsBackground = false;
            splashScreenThread.SetApartmentState(ApartmentState.STA);
            splashScreenThread.Start();
            splashScreenThread.Join();
        }

        private static void SplashScreenThread(string[] args)
        {
            var splashScreen = Gui.LoadingScreen.LoadingScreenFactory.CreateSplashScreen();
            splashScreen.Show();
            var loadingTask = splashScreen.Run(async (p, ct) =>
            {
                await LoadingSequence(args, p, ct);
                splashScreen.Close();
            });
            System.Windows.Threading.Dispatcher.Run();

        }

        static async Task LoadingSequence(string[] args, IProgress<IProgressMessage> progress, CancellationToken ct)
        {
            for (double i = 0; i <= 1 ; i += 0.005)
            {
                progress.Report(new ProgressMessage($"Progressing {i}", i));
                Console.WriteLine($"Progressing {i}");
                if (ct.IsCancellationRequested)
                {
                    Console.WriteLine("Aborting");
                    break;
                }
                await Task.Delay(100);
            }
        }
    }
}