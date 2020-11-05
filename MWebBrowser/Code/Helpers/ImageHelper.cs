using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MWebBrowser.Code.Helpers
{
    public static class ImageHelper
    {
        public static ImageSource DefaultFavicon { get; set; }
        /// <summary>
        /// 获取http pic
        /// </summary>
        /// <returns></returns>
        public static ImageSource GetBitmapFrame(string httpUrl)
        {
            try
            {
                return string.IsNullOrEmpty(httpUrl) ? DefaultFavicon : 
                    BitmapFrame.Create(new Uri(httpUrl), BitmapCreateOptions.None, BitmapCacheOption.Default);
            }
            catch
            {
                return DefaultFavicon;
            }
        }

        public static void InitImages()
        {
            if (Application.Current.MainWindow == null) return;
            DefaultFavicon = Application.Current.MainWindow.FindResource("DrawingImage.DefaultFavicon") as ImageSource;
        }
    }
}
