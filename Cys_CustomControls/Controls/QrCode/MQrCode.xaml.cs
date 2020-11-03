using Cys_Controls.Code;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Cys_CustomControls.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.QrCode"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.QrCode;assembly=Cys_CustomControls.Controls.QrCode"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:MQrCode/>
    ///
    /// </summary>
    public class MQrCode : ContentControl
    {
        static MQrCode()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MQrCode), new FrameworkPropertyMetadata(typeof(MQrCode)));
        }
        #region == DependencyProperty==
        #region == StyleType 控件样式==
        /// <summary>
        /// StyleType 控件样式
        /// </summary>
        public static readonly DependencyProperty StyleTypeProperty = DependencyProperty.Register("StyleType", typeof(StyleType), typeof(MQrCode), new PropertyMetadata(StyleType.Default));

        public StyleType StyleType
        {
            get => (StyleType)GetValue(StyleTypeProperty);
            set => SetValue(StyleTypeProperty, value);
        }
        #endregion

        #region == CornerRadius==
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MQrCode),
            new PropertyMetadata(null));

        /// <summary>
        /// CornerRadius
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        #endregion

        #region ImageSource

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(MQrCode), null);

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }
        #endregion

        #region LogoImageSource
        public static readonly DependencyProperty LogoImageSourceProperty =
            DependencyProperty.Register("LogoImageSource", typeof(ImageSource), typeof(MQrCode), null);

        public ImageSource LogoImageSource
        {
            get => (ImageSource)GetValue(LogoImageSourceProperty);
            set => SetValue(LogoImageSourceProperty, value);
        }
        #endregion


        #region == LogoWidth ==
        public new static readonly DependencyProperty LogoWidthProperty = DependencyProperty.Register("LogoWidth", typeof(double), typeof(MQrCode),
            new PropertyMetadata());
        /// <summary>
        /// LogoWidth
        /// </summary>
        public new double LogoWidth
        {
            get => (double)GetValue(LogoWidthProperty);
            set => SetValue(LogoWidthProperty, value);
        }
        #endregion

        #region == LogoHeight ==
        public static readonly DependencyProperty LogoHeightProperty = DependencyProperty.Register("LogoHeight", typeof(double), typeof(MQrCode),
            new PropertyMetadata());
        /// <summary>
        /// LogoHeight
        /// </summary>
        public double LogoHeight
        {
            get => (double)GetValue(LogoHeightProperty);
            set => SetValue(LogoHeightProperty, value);
        }
        #endregion
        #endregion
    }
}
