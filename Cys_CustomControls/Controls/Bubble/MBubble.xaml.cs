using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Cys_Controls.Code;

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
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.Bubble"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.Bubble;assembly=Cys_CustomControls.Controls.Bubble"
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
    ///     <MyNamespace:MBubble/>
    ///
    /// </summary>
    public class MBubble : ContentControl
    {
        static MBubble()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MBubble), new FrameworkPropertyMetadata(typeof(MBubble)));
        }
        #region DependencyProperty

        #region == StyleType 控件样式==
        /// <summary>
        /// StyleType 控件样式
        /// </summary>
        public static readonly DependencyProperty StyleTypeProperty = DependencyProperty.Register("StyleType", typeof(StyleType), typeof(MBubble), new PropertyMetadata(StyleType.Default));
        public StyleType StyleType
        {
            get => (StyleType)GetValue(StyleTypeProperty);
            set => SetValue(StyleTypeProperty, value);
        }
        #endregion == StyleType 控件样式==

        #region == IsOpen ==
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(MBubble),
            new PropertyMetadata());
        /// <summary>
        /// IsOpen
        /// </summary>
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }
        #endregion

        #region == StaysOpen ==
        public static readonly DependencyProperty StaysOpenProperty = DependencyProperty.Register("StaysOpen", typeof(bool), typeof(MBubble),
            new PropertyMetadata());
        /// <summary>
        /// StaysOpen
        /// </summary>
        public bool StaysOpen
        {
            get => (bool)GetValue(StaysOpenProperty);
            set => SetValue(StaysOpenProperty, value);
        }
        #endregion

        #region == AllowsTransparency ==
        public static readonly DependencyProperty AllowsTransparencyProperty = DependencyProperty.Register("AllowsTransparency", typeof(bool), typeof(MBubble),
            new PropertyMetadata());
        /// <summary>
        /// AllowsTransparency
        /// </summary>
        public bool AllowsTransparency
        {
            get => (bool)GetValue(AllowsTransparencyProperty);
            set => SetValue(AllowsTransparencyProperty, value);
        }
        #endregion

        #region == PlacementTarget ==
        public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.Register("PlacementTarget", typeof(UIElement), typeof(MBubble),
            new PropertyMetadata());
        /// <summary>
        /// PlacementTarget
        /// </summary>
        public UIElement PlacementTarget
        {
            get => (UIElement)GetValue(PlacementTargetProperty);
            set => SetValue(PlacementTargetProperty, value);
        }
        #endregion

        #region == Placement ==
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register("Placement", typeof(PlacementMode), typeof(MBubble),
            new PropertyMetadata());
        /// <summary>
        /// Placement
        /// </summary>
        public PlacementMode Placement
        {
            get => (PlacementMode)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
        }
        #endregion

        #region == HorizontalOffset ==
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(MBubble),
            new PropertyMetadata());
        /// <summary>
        /// HorizontalOffset
        /// </summary>
        public double HorizontalOffset
        {
            get => (double)GetValue(HorizontalOffsetProperty);
            set => SetValue(HorizontalOffsetProperty, value);
        }
        #endregion

        #region == VerticalOffset ==
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(MBubble),
            new PropertyMetadata());
        /// <summary>
        /// VerticalOffset
        /// </summary>
        public double VerticalOffset
        {
            get => (double)GetValue(VerticalOffsetProperty);
            set => SetValue(VerticalOffsetProperty, value);
        }

        #endregion

        #region == Width ==
        public new static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(MBubble),
            new PropertyMetadata());
        /// <summary>
        /// Width
        /// </summary>
        public new double Width
        {
            get => (double)GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }
        #endregion

        #region == Height ==
        public new static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(double), typeof(MBubble),
            new PropertyMetadata());
        /// <summary>
        /// Height
        /// </summary>
        public new double Height
        {
            get => (double)GetValue(HeightProperty);
            set => SetValue(HeightProperty, value);
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
            this.SetResourceReference(ForegroundProperty, StyleType == StyleType.Default ? "ColorBrush.FontDefaultColor" : "ColorBrush.FontPrimaryColor");
            this.SetResourceReference(BackgroundProperty, $"ColorBrush.{StyleType}BackgroundColor");
            this.SetResourceReference(BorderBrushProperty, $"ColorBrush.{StyleType}BorderBrushColor");
        }

    }
}
