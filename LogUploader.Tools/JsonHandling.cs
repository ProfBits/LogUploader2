using System.IO;
using System.Text;

namespace LogUploader.Tools
{
    public static class JsonHandling
    {
        internal static readonly Encoding FILE_ENCODING = Encoding.GetEncoding("iso-8859-1");

        public static string ReadJsonFile(string path)
        {
            return File.ReadAllText(path, FILE_ENCODING);
        }

        public static void WriteJsonFile(string path, string text)
        {
            File.WriteAllText(path, text, FILE_ENCODING);
        }
    }
}
