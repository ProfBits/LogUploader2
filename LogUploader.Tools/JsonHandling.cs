using System.IO;
using System.Text;

namespace LogUploader.Tools
{
    public static class JsonHandling
    {
        public static string ReadJsonFile(string path)
        {
            return File.ReadAllText(path, Encoding.GetEncoding("iso-8859-1"));
        }
        public static void WriteJsonFile(string path, string text)
        {
            File.WriteAllText(path, text, Encoding.GetEncoding("iso-8859-1"));
        }
    }
}
