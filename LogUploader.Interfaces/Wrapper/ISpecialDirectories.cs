using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Wrapper
{
    public interface ISpecialDirectories
    {
        string AppDataLocal { get; }
        string AppDataRoaming { get; }
        string InstallFolder { get; }
    }
}
