using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Interfaces
{
    public interface Log
    {
        IBoss Boss { get; }
        DateTime Date { get; }
        int SizeKb { get; }
        bool EvtcExists { get; }
    }
}

