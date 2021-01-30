using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.Licenses
{
    public sealed class MakeSFXLicense : ISoftwareLicense
    {
        public string Product { get => "Make SFX"; }
        public string Owner { get => "© RevoCue AI s.r.o. 2012 - 2020"; }
        public string Type { get => "Free"; }
        public string Text { get => @"The program is provided free of charge and royalty free even for commercial use."; }
    }
}
