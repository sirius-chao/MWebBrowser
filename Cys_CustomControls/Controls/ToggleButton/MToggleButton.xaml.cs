using Cys_Controls.Code;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

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
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.ToggleButton"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.ToggleButton;assembly=Cys_CustomControls.Controls.ToggleButton"
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
    ///     <MyNamespace:MToggleButton/>
    ///
    /// </summary>
    public class MToggleButton : ToggleButton
    {
        static MToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MToggleButton), new FrameworkPropertyMetadata(typeof(MToggleButton)));
        }

        private Storyboard _checkedStoryboard;
        private Storyboard _unCheckedStoryboard;
        private TextBlock _partDisplayText;
        private Ellipse _partSlider;

        #region    == CheckedCommand ==

        public static readonly DependencyProperty CheckedCommandProperty =
            DependencyProperty.Register("CheckedCommand", typeof(ICommand), typeof(MToggleButton));
        public ICommand CheckedCommand
        {
            get => (ICommand)GetValue(CheckedCommandProperty);
            private set => SetValue(CheckedCommandProperty, value);
        }
        #endregion == CheckedCommand ==

        #region ==DependencyProperty==

        #region ==StyleType 控件样式==
        /// <summary>
        /// StyleType
        /// </summary>
        public static readonly DependencyProperty StyleTypeProperty = DependencyProperty.Register("StyleType", typeof(StyleType), typeof(MToggleButton), new PropertyMetadata(StyleType.Default));
        public StyleType StyleType
        {
            get => (StyleType)GetValue(StyleTypeProperty);
            set => SetValue(StyleTypeProperty, value);
        }
        #endregion ==StyleType==

        #region ==CheckedContent Checked内容==
        /// <summary>
        /// CheckedContent
        /// </summary>
        public static readonly DependencyProperty CheckedContentProperty = DependencyProperty.Register("CheckedContent", typeof(string), typeof(MToggleButton), new PropertyMetadata(""));
        public string CheckedContent
        {
            get => (string)GetValue(CheckedContentProperty);
            set => SetValue(CheckedContentProperty, value);
        }
        #endregion ==CheckedContent==

        #region ==CornerRadius 圆角半径==
        /// <summary>
        /// CornerRadius
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MToggleButton));
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            private set => SetValue(CornerRadiusProperty, value);
        }

        #endregion ==CornerRadius==

        #region ==SliderDiameter 滑块直径==
        /// <summary>
        /// SliderRadius
        /// </summary>
        public static readonly DependencyProperty SliderDiameterProperty =
            DependencyProperty.Register("SliderDiameter", typeof(double), typeof(MToggleButton));
        public double SliderDiameter
        {
            get => (double)GetValue(SliderDiameterProperty);
            private set => SetValue(SliderDiameterProperty, value);
        }

        #endregion ==SliderDiameter==

        #region ==SliderDiameter 滑块距离==
        /// <summary>
        /// SliderRadius
        /// </summary>
        public static readonly DependencyProperty SliderDistanceProperty =
            DependencyProperty.Register("SliderDistance", typeof(Thickness), typeof(MToggleButton));

        public Thickness SliderDistance
        {
            get => (Thickness)GetValue(SliderDistanceProperty);
            private set => SetValue(SliderDistanceProperty, value);
        }

        #endregion ==SliderDiameter==
        #endregion


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitResourceData();
            InitControl();
            InitData();
            InitAnimation();
            this.Checked += MToggleButton_Checked;
            this.Unchecked += MToggleButton_Unchecked;
        }

        private void MToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            _unCheckedStoryboard?.Begin();
        }

        private void MToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            _checkedStoryboard?.Begin();
        }

        /// <summary>
        /// 建立 DynamicResource 绑定
        /// </summary>
        private void InitResourceData()
        {
            this.SetResourceReference(ForegroundProperty, "ColorBrush.FontDefaultColor");
            this.SetResourceReference(BorderBrushProperty, $"ColorBrush.{StyleType}BorderBrushColor");
            this.SetResourceReference(BackgroundProperty, StyleType == StyleType.Default ? "ColorBrush.FontDefaultColor" : $"ColorBrush.{StyleType}BackgroundColor");
        }

        private void InitControl()
        {
            _partDisplayText = GetTemplateChild("PART_DisplayText") as TextBlock;
            _partSlider = GetTemplateChild("PART_Slider") as Ellipse;
        }

        /// <summary>
        /// 初始化数据值
        /// </summary>
        private void InitData()
        {
            CornerRadius = new CornerRadius(Height / 2);
            SliderDiameter = Height - 1;
            SliderDistance = new Thickness(Width - SliderDiameter, 0, 0, 0);
            _partDisplayText.Margin = new Thickness(SliderDiameter, 0, 0, 0);
        }

        /// <summary>
        /// 初始化动画
        /// </summary>
        private void InitAnimation()
        {
            //UI中Storyboard 绑定依赖属性会有跨线程方位错误
            _checkedStoryboard = new Storyboard();
            _checkedStoryboard.Children.Add(CreateSliderAnimation(true));
            _checkedStoryboard.Children.Add(CreateTextAnimation(true));
            _checkedStoryboard.Children.Add(CreateTextSliderAnimation(true));

            _unCheckedStoryboard = new Storyboard();
            _unCheckedStoryboard.Children.Add(CreateSliderAnimation(false));
            _unCheckedStoryboard.Children.Add(CreateTextAnimation(false));
            _unCheckedStoryboard.Children.Add(CreateTextSliderAnimation(false));
        }

        /// <summary>
        /// 创建滑块关键帧动画
        /// </summary>
        private ThicknessAnimationUsingKeyFrames CreateSliderAnimation(bool isChecked)
        {
            var thicknessAnimationUsingKeyFrames = new ThicknessAnimationUsingKeyFrames();
            Storyboard.SetTarget(thicknessAnimationUsingKeyFrames, _partSlider);
            Storyboard.SetTargetProperty(thicknessAnimationUsingKeyFrames, new PropertyPath("(FrameworkElement.Margin)"));
            var easingThicknessKeyFrame = new EasingThicknessKeyFrame(isChecked ? SliderDistance : new Thickness(0), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.4)));
            thicknessAnimationUsingKeyFrames.KeyFrames.Add(easingThicknessKeyFrame);
            return thicknessAnimationUsingKeyFrames;
        }

        /// <summary>
        /// 创建文本动画
        /// </summary>
        /// <param name="isChecked"></param>
        /// <returns></returns>
        private StringAnimationUsingKeyFrames CreateTextAnimation(bool isChecked)
        {
            var stringAnimationUsingKeyFrames = new StringAnimationUsingKeyFrames();
            Storyboard.SetTarget(stringAnimationUsingKeyFrames, _partDisplayText);
            Storyboard.SetTargetProperty(stringAnimationUsingKeyFrames, new PropertyPath("(TextBlock.Text)"));
            var discreteStringKeyFrame = new DiscreteStringKeyFrame(isChecked ? CheckedContent : Content.ToString(), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2)));
            stringAnimationUsingKeyFrames.KeyFrames.Add(discreteStringKeyFrame);
            return stringAnimationUsingKeyFrames;
        }

        /// <summary>
        /// 创建文本滑动关键帧动画
        /// </summary>
        private ThicknessAnimationUsingKeyFrames CreateTextSliderAnimation(bool isChecked)
        {
            var thicknessAnimationUsingKeyFrames = new ThicknessAnimationUsingKeyFrames();
            Storyboard.SetTarget(thicknessAnimationUsingKeyFrames, _partDisplayText);
            Storyboard.SetTargetProperty(thicknessAnimationUsingKeyFrames, new PropertyPath("(FrameworkElement.Margin)"));
            var easingThicknessKeyFrame = new EasingThicknessKeyFrame(isChecked ? new Thickness(0, 0, SliderDiameter, 0) : new Thickness(SliderDiameter, 0, 0, 0), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.4)));
            thicknessAnimationUsingKeyFrames.KeyFrames.Add(easingThicknessKeyFrame);
            return thicknessAnimationUsingKeyFrames;
        }
    }
}
