using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.Licenses
{
    public interface ISoftwareLicense
    {
        string Product { get; }
        string Owner { get; }
        string Type { get; }
        string Text { get; }
    }
}
