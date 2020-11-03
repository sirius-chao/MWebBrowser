using Cys_Controls.Code;
using System.Windows;
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
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.CheckBox"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.CheckBox;assembly=Cys_CustomControls.Controls.CheckBox"
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
    ///     <MyNamespace:MCheckBox/>
    ///
    /// </summary>
    public class MCheckBox : System.Windows.Controls.CheckBox
    {
        static MCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MCheckBox), new FrameworkPropertyMetadata(typeof(MCheckBox)));
        }

        #region ==StyleType 控件样式==
        /// <summary>
        /// StyleType 控件样式
        /// </summary>
        public static readonly DependencyProperty StyleTypeProperty = DependencyProperty.Register("StyleType", typeof(StyleType), typeof(MCheckBox), new PropertyMetadata(StyleType.Default));
        public StyleType StyleType
        {
            get => (StyleType)GetValue(StyleTypeProperty);
            set => SetValue(StyleTypeProperty, value);
        }
        #endregion ==StyleType 控件样式==

        #region ==CheckedBackground 选中==
        public static readonly DependencyProperty CheckedBackgroundProperty = DependencyProperty.Register("CheckedBackground", typeof(Brush), typeof(MCheckBox),
            new PropertyMetadata());


        /// <summary>
        /// 光标集中
        /// </summary>
        public Brush CheckedBackground
        {
            get => (Brush)GetValue(CheckedBackgroundProperty);
            set => SetValue(CheckedBackgroundProperty, value);
        }
        #endregion ==FocusedBrush ==

        #region ==CheckMarkBrush 对勾画刷==
        public static readonly DependencyProperty CheckMarkBrushProperty = DependencyProperty.Register("CheckMarkBrush", typeof(Brush), typeof(MCheckBox),
            new PropertyMetadata());


        /// <summary>
        /// 对勾颜色
        /// </summary>
        public Brush CheckMarkBrush
        {
            get => (Brush)GetValue(CheckMarkBrushProperty);
            set => SetValue(CheckMarkBrushProperty, value);
        }
        #endregion == CheckMarkBrush ==

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
            this.SetResourceReference(ForegroundProperty, "ColorBrush.FontDefaultColor");
            this.SetResourceReference(CheckMarkBrushProperty, StyleType == StyleType.Default ? "ColorBrush.FontDefaultColor" : "ColorBrush.FontPrimaryColor");
            this.SetResourceReference(BorderBrushProperty, $"ColorBrush.{StyleType}BorderBrushColor");
            this.SetResourceReference(CheckedBackgroundProperty, $"ColorBrush.{StyleType}BackgroundColor");
        }
    }
}
