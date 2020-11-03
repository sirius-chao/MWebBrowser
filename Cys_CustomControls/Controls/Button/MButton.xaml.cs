using Cys_Controls.Code;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.Button"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.Button;assembly=Cys_CustomControls.Controls.Button"
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
    ///     <MyNamespace:MButton/>
    ///
    /// </summary>
    public class MButton : System.Windows.Controls.Button
    {
        static MButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MButton), new FrameworkPropertyMetadata(typeof(MButton)));
        }

        #region == DependencyProperty==

        #region == StyleType 控件样式==
        /// <summary>
        /// StyleType 控件样式
        /// </summary>
        public static readonly DependencyProperty StyleTypeProperty = DependencyProperty.Register("StyleType", typeof(StyleType), typeof(MButton), new PropertyMetadata(StyleType.Default));
        public StyleType StyleType
        {
            get => (StyleType)GetValue(StyleTypeProperty);
            set => SetValue(StyleTypeProperty, value);
        }
        #endregion

        #region == IsMouseBackground 鼠标停留背景画刷==
        public static readonly DependencyProperty IsMouseBackgroundProperty = DependencyProperty.Register("IsMouseBackground", typeof(Brush), typeof(MButton),
            new PropertyMetadata());


        /// <summary>
        /// 鼠标停留背景画刷
        /// </summary>
        public Brush IsMouseBackground
        {
            get => (Brush)GetValue(IsMouseBackgroundProperty);
            set => SetValue(IsMouseBackgroundProperty, value);
        }

        #endregion

        #region == IsPressedBackground 鼠标按下背景画刷==
        public static readonly DependencyProperty IsPressedBackgroundProperty = DependencyProperty.Register("IsPressedBackground", typeof(Brush), typeof(MButton),
            new PropertyMetadata());


        /// <summary>
        /// 鼠标停留背景画刷
        /// </summary>
        public Brush IsPressedBackground
        {
            get => (Brush)GetValue(IsPressedBackgroundProperty);
            set => SetValue(IsPressedBackgroundProperty, value);
        }

        #endregion

        #region == Icon 图标==
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(BitmapImage), typeof(MButton),
            new PropertyMetadata(null));

        /// <summary>
        /// Icon 图标
        /// </summary>
        public BitmapImage Icon
        {
            get => (BitmapImage)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        #endregion

        #region == CornerRadius==
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MButton),
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

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitResourceData();
        }

        /// <summary>
        /// 建立 DynamicResource 绑定
        /// </summary>
        private void InitResourceData()
        {
            this.SetResourceReference(ForegroundProperty,StyleType == StyleType.Default ? "ColorBrush.FontDefaultColor" : "ColorBrush.FontPrimaryColor");
            this.SetResourceReference(BackgroundProperty,$"ColorBrush.{StyleType}BackgroundColor");
            this.SetResourceReference(BorderBrushProperty,$"ColorBrush.{StyleType}BorderBrushColor");
            this.SetResourceReference(IsMouseBackgroundProperty, $"ColorBrush.{StyleType}BackgroundOverColor");
            this.SetResourceReference(IsPressedBackgroundProperty, $"ColorBrush.{StyleType}BackgroundPressColor");
        }
    }
}
