using LogUploader.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogUploader.Interfaces.Factroies
{
    public static class UiFactory
    {
        public delegate Form LoadingBarUiFactory(string TaskName, Action<CancellationToken, Action<Delegate>, IProgress<ProgressMessage>> task);
        private static LoadingBarUiFactory _loadingBarUiFactory = null;
        
        public static void SetLoadingBarUiFactory(LoadingBarUiFactory creator)
        {
            _loadingBarUiFactory = creator ?? throw new ArgumentNullException("Factory delegate cannot be null.");
        }

        public static Form GetLoadingBarUi(string TaskName, Action<CancellationToken, Action<Delegate>, IProgress<ProgressMessage>> task)
        {
            return (_loadingBarUiFactory ?? throw new InvalidOperationException("Factory not setup currently"))(TaskName, task);
        }

    }
}
