using System.Data;

namespace LogUploader.Interfaces
{
    public interface ILogUploaderData
    {
        DataTable FileData { set; get; }
        DataTable UploadedData { set; get; }
    }
}
