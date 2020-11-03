using System;
using Cys_Controls.Code;
using System.Windows;
using System.Windows.Controls;

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
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.ProgressBar"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.ProgressBar;assembly=Cys_CustomControls.Controls.ProgressBar"
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
    ///     <MyNamespace:MProgressBar/>
    ///
    /// </summary>
    public class MProgressBar : ProgressBar
    {
        #region == StyleType 控件样式==
        /// <summary>
        /// StyleType 控件样式
        /// </summary>
        public static readonly DependencyProperty StyleTypeProperty = DependencyProperty.Register("StyleType", typeof(StyleType), typeof(MProgressBar), new PropertyMetadata(StyleType.Default));
        public StyleType StyleType
        {
            get => (StyleType)GetValue(StyleTypeProperty);
            set => SetValue(StyleTypeProperty, value);
        }
        #endregion == StyleType 控件样式==

        #region == CornerRadius 圆角 ==
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MProgressBar),
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

        #region ==PercentContent==
        /// <summary>
        /// CheckedContent
        /// </summary>
        public static readonly DependencyProperty PercentContentProperty = DependencyProperty.Register("PercentContent", typeof(string), typeof(MProgressBar), new PropertyMetadata(""));
        public string PercentContent
        {
            get => (string)GetValue(PercentContentProperty);
            set => SetValue(PercentContentProperty, value);
        }
        #endregion ==PercentContent==
        static MProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MProgressBar), new FrameworkPropertyMetadata(typeof(MProgressBar)));
        }
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
            this.SetResourceReference(ForegroundProperty, "ColorBrush.FontPrimaryColor");
            this.SetResourceReference(BackgroundProperty, StyleType == StyleType.Default ? "ColorBrush.FontDefaultColor" : $"ColorBrush.{StyleType}BackgroundColor");
            this.SetResourceReference(BorderBrushProperty, $"ColorBrush.{StyleType}BorderBrushColor");
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            SetPercentContent();
        }

        private void SetPercentContent()
        {
            if (Math.Abs(Value - Maximum) <= 0)
            {
                PercentContent = "100%";
            }
            else
            {
                double m = (Value / Maximum) * 100;
                PercentContent = m.ToString("0.0") + "%";
            }
        }
    }
}
