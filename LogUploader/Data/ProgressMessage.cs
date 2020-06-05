using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data
{
    public class ProgressMessage
    {
        public double Percent { get; private set; }
        public string Message { get; private set; }

        public ProgressMessage(double percent, string message)
        {
            Percent = percent;
            Message = message;
        }

        public override string ToString()
        {
            return $"Progress: {Percent * 100} {Message}";
        }
    }
}
