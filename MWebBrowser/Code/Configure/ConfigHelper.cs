using System;
using System.IO;

namespace MWebBrowser.Code.Configure
{
    public static class ConfigHelper
    {
        public static ConfigEntity Config { get; private set; }

        public static void LoadLocalConfig()
        {
            ConfigEntity config = null;
            if (string.IsNullOrEmpty(AppDomain.CurrentDomain.BaseDirectory))
            {
                config = new ConfigEntity();
            }
            else
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.json");
                if (File.Exists(path))
                {
                    try
                    {
                        using StreamReader streamReader = new StreamReader(path, System.Text.Encoding.UTF8);
                        string json = streamReader.ReadToEnd();
                        config = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigEntity>(json);
                    }
                    catch
                    {
                        File.Delete(path);
                    }
                }
                config ??= new ConfigEntity();
            }
            Config = config;
            CheckConfig();
        }

        private static void CheckConfig()
        {
            if (string.IsNullOrEmpty(Config.DownLoadPath) || Directory.Exists(Config.DownLoadPath))
            {
                Config.DownLoadPath = KnownFolderHelper.GetDownload();
            }
        }
    }
}
