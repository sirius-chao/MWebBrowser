using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Cys_DataRepository
{
    internal class CommonOperator
    {
        public static bool SaveDataJson<T>(T data, string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            if (data == null)
                return false;
            try
            {
                var json = JsonConvert.SerializeObject(data);
                using var streamWriter = new StreamWriter(path, false, Encoding.UTF8);
                streamWriter.Write(json);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static T GetDataJson<T>(string path)
        {
            T t = default;
            if (!File.Exists(path)) return t;
            try
            {
                using var streamReader = new StreamReader(path, Encoding.UTF8);
                var json = streamReader.ReadToEnd();
                t = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception e)
                {
                }
            }
            return t;
        }
    }
}