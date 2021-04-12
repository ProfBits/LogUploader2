using LogUploader.Data;
using LogUploader.Helper;
using LogUploader.Tools.Logging;

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

namespace LogUploader.GUIs
{
    public partial class LoadingBar : Form
    {
        private readonly Action<CancellationToken, Action<Delegate>, IProgress<ProgressMessage>> Work;
        private readonly CancellationTokenSource Cts = new CancellationTokenSource();

        public LoadingBar(string TaskName, Action<CancellationToken, Action<Delegate>, IProgress<ProgressMessage>> task)
        {
            InitializeComponent();
            lblTaskName.Text = TaskName;
            lblTask.Text = "";
            Work = task;
        }

        private void LblCancel_Click(object sender, EventArgs e)
        {
            Cts.Cancel();
        }

        private void LblClose_Click(object sender, EventArgs e)
        {
            Cts.Cancel();
        }

        private async void LoadingBar_Shown(object sender, EventArgs e)
        {
            await Task.Run(() => DoWork());
        }

        private void DoWork()
        {
            GP.ExecuteSecure(() => Work(Cts.Token, (a) => Invoke(a), new Progress<ProgressMessage>(ProgressHandler)));
            
            Action update = Close;
            this.Invoke(update);
        }

        private void ProgressHandler(ProgressMessage p)
        {
            Action update = () =>
            {
                lblPercent.Text = (int)(p.Percent * 100) + " %";
                lblTask.Text = p.Message;
                pLoading.Width = (int)((double)pLoadingBk.Width * p.Percent);
            };
            try
            {
                this.Invoke(update);
            }
            catch (Exception e)
            {
                Logger.LogException(e);
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
    }
}
