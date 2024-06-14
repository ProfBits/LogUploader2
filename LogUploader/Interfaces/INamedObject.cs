using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Interfaces
{
    public interface INamedObject
    {
        string NameEN { get; }
        string NameDE { get; }

        string Name { get; }

        string GetName(eLanguage language);

        bool HasName(string name);
        bool ContainsName(string str);
    }
}
