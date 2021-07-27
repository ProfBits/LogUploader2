using Extensiones;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data
{
    public class CombBoxView : Attribute
    {
        public CombBoxView(string name)
        {
            Name = name;
        }

        public string Name { get; }

    }
}
