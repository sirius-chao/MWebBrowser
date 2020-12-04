using System;
using Cys_Common.Enum;

namespace Cys_DataRepository
{
    public  static class FileDataPath
    {
        public static string GetFilePath(DataFileType type)
        {
            string filePath = null;
            string BaseFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}Data\\";
            filePath = type switch
            {
                DataFileType.Download => $"{BaseFilePath}Download.json",
                _ => filePath
            };
            return filePath;
        }
    }
}
