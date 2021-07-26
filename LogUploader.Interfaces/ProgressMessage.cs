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
            Message = message ?? throw new ArgumentNullException($"{nameof(message)} cannot be null");
            if (percent < 0) throw new ArgumentOutOfRangeException($"{nameof(percent)} cannot be negative");
            if (string.IsNullOrWhiteSpace(Message)) throw new ArgumentOutOfRangeException($"{nameof(message)} cannot be only whitespace or empty");
        }

        public override string ToString()
        {
            return $"Progress: {Percent * 100} {Message}";
        }
    }
}
