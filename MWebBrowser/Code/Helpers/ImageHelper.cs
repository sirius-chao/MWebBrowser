using System;
using System.Windows.Media.Imaging;

namespace MWebBrowser.Code.Helpers
{
    public static class ImageHelper
    {
        /// <summary>
        /// 获取http pic
        /// </summary>
        /// <returns></returns>
        public static BitmapFrame GetBitmapFrame(string httpUrl)
        {
            try
            {
                return string.IsNullOrEmpty(httpUrl) ? null : 
                    BitmapFrame.Create(new Uri(httpUrl), BitmapCreateOptions.None, BitmapCacheOption.Default);
            }
            catch
            {
                return null;
            }
        }
    }
}
