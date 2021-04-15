using System.Collections.Generic;
using System.Drawing;

namespace LogUploader.Tools.Discord
{
    public interface IEmbed
    {
        IAuthor Author { get; set; }
        Color Color { get; set; }
        string Descriptione { get; set; }
        List<IField> Fields { get; set; }
        IFooter Footer { get; set; }
        IImage Image { get; set; }
        int JsonColor { get; set; }
        IThumbmail Thumbmail { get; set; }
        string Title { get; set; }
        string URL { get; set; }

        string ToString();
    }
}