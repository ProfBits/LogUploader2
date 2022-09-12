using LogUploader.Gui.Main;
using LogUploader.Injection;

namespace LogUploader
{
    internal class Program
    {
        private static readonly object loadingLock = new object();

        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "main";

            lock (loadingLock)
            {
                RunSplashScreen(args);
            }
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
                try
                {
                    await LoadingSequence(args, p, ct);
                }
                finally
                {
                    splashScreen.Close();
                }
            });

            try
            {
                System.Windows.Threading.Dispatcher.Run();
            }
            catch (InvalidOperationException) { }
            loadingTask.Wait();
        }

        private static async Task LoadingSequence(string[] args, IProgress<IProgressMessage> progress, CancellationToken ct)
        {
            progress.Report(new ProgressMessage("Initialize", 0));
            var io = LogUploader.IO.FileIOFactory.GetInstance();
            var logger = LogUploader.Logging.LoggerFactory.CreateLogger(io);

            progress.Report(new ProgressMessage("Initialize", 0.05));
            var sc = new ServiceCollection();
            var dpLoader = new DependencyLoader();
            await dpLoader.Load(sc, logger, progress.Split(0.05, 0.25), ct);
            progress.Report(new ProgressMessage("Linking Modules", 0.25));
            var services = sc.BuildProvider();
            progress.Report(new ProgressMessage("Loading Modules", 0.35));
            var loadingProgress = progress.Split(0.35, 0.95);
            var delta = 1.0 / services.CountOfLoadedServices;
            foreach (var (module, i) in services.GetLoadedServices().WithIndex())
            {
                await module.Load(ct, loadingProgress.Split("Loading Module " + module.Name() + " - ", delta * i, delta * (i + 1)));
            }
            progress.Report(new ProgressMessage("Creating UI", 0.95));
            var mainWindowThread = new Thread(() =>
            {
                var mainView = services.Create<IMainWindowFactory>().CreateMainView(progress.WithStaticMessage("Creating UI", 0.95, 0.99));
                lock(loadingLock)
                {
                    mainView.Run();
                };
            });
            mainWindowThread.Name = "UI_Main";
            mainWindowThread.IsBackground = false;
            mainWindowThread.SetApartmentState(ApartmentState.STA);
            mainWindowThread.Start();

            progress.Report(new ProgressMessage("Finishing up", 1));
        }
    }
}