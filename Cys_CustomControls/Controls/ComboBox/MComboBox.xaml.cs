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
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.ComboBox"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.ComboBox;assembly=Cys_CustomControls.Controls.ComboBox"
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
    ///     <MyNamespace:MComboBox/>
    ///
    /// </summary>
    public class MComboBox : ComboBox
    {
        static MComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MComboBox), new FrameworkPropertyMetadata(typeof(MComboBox)));
        }

        #region == StyleType 控件样式==
        /// <summary>
        /// StyleType 控件样式
        /// </summary>
        public static readonly DependencyProperty StyleTypeProperty = DependencyProperty.Register("StyleType", typeof(StyleType), typeof(MComboBox), new PropertyMetadata(StyleType.Default));
        public StyleType StyleType
        {
            get => (StyleType)GetValue(StyleTypeProperty);
            set => SetValue(StyleTypeProperty, value);
        }
        #endregion == StyleType 控件样式==

        #region == FocusedBrush 光标集中==
        public static readonly DependencyProperty FocusedBrushProperty = DependencyProperty.Register("FocusedBrush", typeof(Brush), typeof(MComboBox),
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

      

        #region == Radius 圆角半径 ==
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(MComboBox));

        /// <summary>
        /// CornerRadius 圆角
        /// </summary>
        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }
        #endregion == CornerRadius 圆角 ==

        #region == CornerRadius 圆角 ==
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MComboBox),
            new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// CornerRadius 圆角
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            private set => SetValue(CornerRadiusProperty, value);
        }
        #endregion == CornerRadius 圆角 ==

        #region == ItemMargin ==
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register("ItemMargin", typeof(Thickness), typeof(MComboBox));

        /// <summary>
        /// Item第一个和最后一个间距
        /// </summary>
        public Thickness ItemMargin
        {
            get => (Thickness)GetValue(ItemMarginProperty);
            set => SetValue(ItemMarginProperty, value);
        }
        #endregion == ItemMargin ==

        #region == Watermark ==
        /// <summary>
        /// 水印
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(MComboBox));

        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }
        #endregion == Watermark ==

        #region == MaxLength ==
        /// <summary>
        /// 最大长度
        /// </summary>
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(double), typeof(MComboBox));

        public double MaxLength
        {
            get => (double)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }
        #endregion == MaxLength ==

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitResourceData();
            InitData();
        }
        /// <summary>
        /// 建立 DynamicResource 绑定
        /// </summary>
        private void InitResourceData()
        {
            this.SetResourceReference(ForegroundProperty, "ColorBrush.FontDefaultColor");
            this.SetResourceReference(BorderBrushProperty, $"ColorBrush.{StyleType}BorderBrushColor");
            this.SetResourceReference(FocusedBrushProperty, $"ColorBrush.{StyleType}BackgroundOverColor");
        }

        private void InitData()
        {
            CornerRadius = new CornerRadius(Radius);
            ItemMargin = new Thickness(0, Radius, 0, Radius);
        }
    }
}
