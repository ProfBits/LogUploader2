using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Interfaces.Interfaces
{
    interface Log
    {
        //Boss Boss { get; }
        DateTime Date { get; }
        int SizeKb { get; }
        string EvtcPath { get; }
    }
}

