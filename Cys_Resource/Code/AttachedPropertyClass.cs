using System.Windows;
using System.Windows.Media;

namespace Cys_Resource.Code
{
    public class AttachedPropertyClass
    {
        #region Path Data Property
        public static readonly DependencyProperty DataPathProperty =
            DependencyProperty.RegisterAttached("DataPath", typeof(Geometry), typeof(AttachedPropertyClass), null);
        public static Geometry GetDataPath(DependencyObject dpo) => (Geometry)dpo.GetValue(DataPathProperty);
        public static void SetDataPath(DependencyObject dpo, Geometry value) => dpo.SetValue(DataPathProperty, value);
        #endregion

        #region Text Font Icon

        public static readonly DependencyProperty FontIconProperty =
            DependencyProperty.RegisterAttached("FontIcon", typeof(string), typeof(AttachedPropertyClass), null);
        public static string GetFontIcon(DependencyObject dpo) => (string)dpo.GetValue(FontIconProperty);
        public static void SetFontIcon(DependencyObject dpo, string value) => dpo.SetValue(FontIconProperty, value);

        #endregion

        #region ImageSource

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.RegisterAttached("ImageSource", typeof(ImageSource), typeof(AttachedPropertyClass), null);
        public static ImageSource GetImageSource(DependencyObject dpo) => (ImageSource)dpo.GetValue(ImageSourceProperty);
        public static void SetImageSource(DependencyObject dpo, ImageSource value) => dpo.SetValue(ImageSourceProperty, value);

        #endregion

        #region Background

        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached("Background", typeof(Brush), typeof(AttachedPropertyClass), null);
        public static Brush GetBackground(DependencyObject dpo) => (Brush)dpo.GetValue(BackgroundProperty);
        public static void SetBackground(DependencyObject dpo, Brush value) => dpo.SetValue(BackgroundProperty, value);

        #endregion
    }
}
