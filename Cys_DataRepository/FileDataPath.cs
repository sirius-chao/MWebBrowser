using System;
using Cys_Common.Enum;

namespace Cys_DataRepository
{
    public  static class FileDataPath
    {
        public static string GetFilePath(DataFileType type)
        {
            string filePath = null;
            string baseFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}Data\\";
            filePath = type switch
            {
                DataFileType.Download => $"{baseFilePath}Download.json",
                DataFileType.Favorites => $"{baseFilePath}Favorites.json",
                DataFileType.SearchEngine => $"{baseFilePath}SearchEngine.json",
                _ => null,
            };
            return filePath;
        }
    }
}
