//#define PERF_MEAS

using LogUploader.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogUploader.GUI
{
    public partial class LoadingScreenUI : Form
    {
        private readonly Progress<ProgressMessage> progress = new Progress<ProgressMessage>();
        private readonly Func<IProgress<ProgressMessage>, CancellationToken, CancellationTokenSource, Task> StartSequenc;
        private readonly int m_width1;
        private readonly CancellationTokenSource m_cts;
        private Task m_startSequence;

        public LoadingScreenUI(Func<IProgress<ProgressMessage>, CancellationToken, CancellationTokenSource, Task> startSequenc)
        {
            StartSequenc = startSequenc;

            InitializeComponent();

            m_width1 = Width + 1;

            m_cts = new CancellationTokenSource();

            progress = new Progress<ProgressMessage>();
            progress.ProgressChanged += Progress_ProgressChanged;

#if DEBUG
            lblBeta.Text = "DEBUG";
#elif ALPHA
            lblBeta.Text = "ALPHA";
#elif BETA
            lblBeta.Text = "BETA";
#else
            lblBeta.Visible = false;
#endif
        }

#if PERF_MEAS
        static System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
#endif
        private void Progress_ProgressChanged(object sender, ProgressMessage e)
        {
#if PERF_MEAS
            if (!sw.IsRunning)
                sw.Start();
#endif
            Action a = () =>
            {
                var oldWidth = pLoading.Width;
                pLoading.Width = (int)Math.Round(m_width1 * e.Percent);
                lblTask.Text = e.Message;
                lblPercent.Text = $"{Math.Round(e.Percent * 100)} %";
#if PERF_MEAS
                Console.WriteLine($"###\t{sw.ElapsedMilliseconds}\t{e.Percent}\t{e.Message}");
#endif
            };
            try
            {
                Invoke(a);
            }
            catch (ObjectDisposedException) { }
        }


        private void LblClose_Click(object sender, EventArgs e)
        {
            lblClose.Cursor = Cursors.WaitCursor;
            lblTask.Text = "Cancel operation...";
            m_cts.Cancel();
            m_startSequence?.Wait();
            //Close();
        }

        private void LoadApplication(object sender, EventArgs e)
        {
            bgWorkerMain.RunWorkerAsync();
        }


        private Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager CreateTaskbarManager()
        {
            if (Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.IsPlatformSupported)
            {
                return Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
            }
            else
            {
                // Platform does not support the feature.
                return null;
            }
        }

#region DragWindow

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private void DragWindow(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

#endregion

        private async void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            await Task.Delay(250);
            var task = StartSequenc(progress, m_cts.Token, m_cts);
            m_startSequence = task;
            await task;
            ((IProgress<ProgressMessage>)progress).Report(new ProgressMessage(0.99, "Finishing up..."));
            await Task.Delay(250);
            Action a = Close;
            Invoke(a);
        }
    }
}
