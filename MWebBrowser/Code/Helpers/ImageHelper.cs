using System;
using System.Text.RegularExpressions;
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
                    new BitmapImage(new Uri(httpUrl, UriKind.RelativeOrAbsolute));
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

        public static ImageSource GetFavicon(string url)
        {
            try
            {
                var pattern = @"(\w+:\/\/)([^/:]+)(:\d*)?";
                var address = url;
                var matches = Regex.Matches(address, pattern);
                return matches.Count <= 0 ? null : ImageHelper.GetBitmapFrame($"{matches[0]}/favicon.ico");
            }
            catch (Exception e)
            {
                return ImageHelper.DefaultFavicon;
            }
        }
    }
}
