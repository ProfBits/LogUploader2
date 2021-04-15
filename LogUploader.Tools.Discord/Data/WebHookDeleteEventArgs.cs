using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.Discord
{

    public class WebHookDeleteEventArgs : EventArgs
    {
        public WebHookDeleteEventArgs(long iD)
        {
            ID = iD;
        }

        public long ID { get; }
    }
}
