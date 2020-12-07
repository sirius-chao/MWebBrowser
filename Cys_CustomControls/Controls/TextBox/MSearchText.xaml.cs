using Cys_Controls.Code;
using System.Windows;
using System.Windows.Input;
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
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.TextBox"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.TextBox;assembly=Cys_CustomControls.Controls.TextBox"
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
    ///     <MyNamespace:MSearchText/>
    ///
    /// </summary>
    public class MSearchText : System.Windows.Controls.TextBox
    {
        static MSearchText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MSearchText), new FrameworkPropertyMetadata(typeof(MSearchText)));
        }

        #region == StyleType 控件样式==
        /// <summary>
        /// StyleType 控件样式
        /// </summary>
        public static readonly DependencyProperty StyleTypeProperty = DependencyProperty.Register("StyleType", typeof(StyleType), typeof(MSearchText), new PropertyMetadata(StyleType.Default));
        public StyleType StyleType
        {
            get => (StyleType)GetValue(StyleTypeProperty);
            set => SetValue(StyleTypeProperty, value);
        }
        #endregion == StyleType 控件样式==

        #region == Watermark 水印==
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(MSearchText));

        /// <summary>
        /// Watermark 水印
        /// </summary>
        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }
        #endregion == Watermark 水印==

        #region == Icon 图标==
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(MSearchText),
            new PropertyMetadata(null));

        /// <summary>
        /// Icon 图标
        /// </summary>
        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        #endregion == Icon 图标==

        #region == FocusedBrush 光标集中==
        public static readonly DependencyProperty FocusedBrushProperty = DependencyProperty.Register("FocusedBrush", typeof(Brush), typeof(MSearchText),
            new PropertyMetadata());


        /// <summary>
        /// 光标集中
        /// </summary>
        public Brush FocusedBrush
        {
            get => (Brush)GetValue(FocusedBrushProperty);
            set => SetValue(FocusedBrushProperty, value);
        }
        #endregion == FocusedBrush ==

        #region == CornerRadius 圆角 ==
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MSearchText),
            new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// CornerRadius 圆角
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        #endregion == CornerRadius 圆角 ==

        #region == ZoomLevelType 缩放类型 ==
        public static readonly DependencyProperty ZoomLevelTypeProperty = DependencyProperty.Register("ZoomLevelType", typeof(ZoomType), typeof(MSearchText));
        /// <summary>
        /// ZoomLevelType 缩放类型
        /// </summary>
        public ZoomType ZoomLevelType
        {
            get => (ZoomType)GetValue(ZoomLevelTypeProperty);
            set => SetValue(ZoomLevelTypeProperty, value);
        }
        #endregion

        #region == ZoomRatio 缩放比例 ==
        public static readonly DependencyProperty ZoomRatioProperty = DependencyProperty.Register("ZoomRatio", typeof(string), typeof(MSearchText));
        /// <summary>
        /// ZoomLevelType 缩放比例
        /// </summary>
        public string ZoomRatio
        {
            get => (string)GetValue(ZoomRatioProperty);
            set => SetValue(ZoomRatioProperty, value);
        }
        #endregion

        #region == ZoomIsChecked 缩放按钮是否选中 ==
        public static readonly DependencyProperty ZoomIsCheckedProperty = DependencyProperty.Register("ZoomIsChecked", typeof(bool), typeof(MSearchText));
        /// <summary>
        /// ZoomIsChecked 缩放按钮是否选中
        /// </summary>
        public bool ZoomIsChecked
        {
            get => (bool)GetValue(ZoomIsCheckedProperty);
            set => SetValue(ZoomIsCheckedProperty, value);
        }
        #endregion

        #region == ZoomStaysOpen 缩放Popup ==
        public static readonly DependencyProperty ZoomStaysOpenProperty = DependencyProperty.Register("ZoomStaysOpen", typeof(bool), typeof(MSearchText));
        /// <summary>
        /// ZoomStaysOpen 缩放Popup
        /// </summary>
        public bool ZoomStaysOpen
        {
            get => (bool)GetValue(ZoomStaysOpenProperty);
            set => SetValue(ZoomStaysOpenProperty, value);
        }
        #endregion

        #region == ZoomInCommand 放大Command ==
        public static readonly DependencyProperty ZoomInCommandProperty = DependencyProperty.Register("ZoomInCommand", typeof(ICommand), typeof(MSearchText));
        /// <summary>
        /// ZoomInCommand 放大Command
        /// </summary>
        public ICommand ZoomInCommand
        {
            get => (ICommand)GetValue(ZoomInCommandProperty);
            set => SetValue(ZoomInCommandProperty, value);
        }
        #endregion

        #region == ZoomInCommand 放大Command ==
        public static readonly DependencyProperty ZoomOutCommandProperty = DependencyProperty.Register("ZoomOutCommand", typeof(ICommand), typeof(MSearchText));
        /// <summary>
        /// ZoomInCommand 放大Command
        /// </summary>
        public ICommand ZoomOutCommand
        {
            get => (ICommand)GetValue(ZoomOutCommandProperty);
            set => SetValue(ZoomOutCommandProperty, value);
        }
        #endregion

        #region == ZoomResetCommand 放大Command ==
        public static readonly DependencyProperty ZoomResetCommandProperty = DependencyProperty.Register("ZoomResetCommand", typeof(ICommand), typeof(MSearchText));
        /// <summary>
        /// ZoomResetCommand 放大Command
        /// </summary>
        public ICommand ZoomResetCommand
        {
            get => (ICommand)GetValue(ZoomResetCommandProperty);
            set => SetValue(ZoomResetCommandProperty, value);
        }
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
            //this.SetResourceReference(ForegroundProperty, "ColorBrush.FontDefaultColor");
            //this.SetResourceReference(BorderBrushProperty, $"ColorBrush.{StyleType}BorderBrushColor");
            //this.SetResourceReference(FocusedBrushProperty, $"ColorBrush.{StyleType}BackgroundOverColor");
        }
    }
}
