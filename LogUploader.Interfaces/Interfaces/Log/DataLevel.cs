namespace LogUploader.Interfaces
{
    public enum DataLevel : byte
    {
        NONE = 0,
        V1, /*Before json data*/
        V2, /*json data*/
        V3, /*all in log db v2*/
    }
}

