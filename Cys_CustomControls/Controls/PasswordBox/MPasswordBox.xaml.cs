using System;
using Cys_Controls.Code;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

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
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.Password"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.Password;assembly=Cys_CustomControls.Controls.Password"
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
    ///     <MyNamespace:MPassword/>
    ///
    /// </summary>
    public class MPasswordBox : TextBox
    {
        static MPasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MPasswordBox), new FrameworkPropertyMetadata(typeof(MPasswordBox)));
        }

        private int _lastOffset;
        readonly StringBuilder _passwordBuilder = new StringBuilder();
        private bool _isResponseChange = true;

        private ToggleButton _partEye;

        #region == StyleType 控件样式==
        /// <summary>
        /// StyleType 控件样式
        /// </summary>
        public static readonly DependencyProperty StyleTypeProperty = DependencyProperty.Register("StyleType", typeof(StyleType), typeof(MPasswordBox), new PropertyMetadata(StyleType.Default));
        public StyleType StyleType
        {
            get => (StyleType)GetValue(StyleTypeProperty);
            set => SetValue(StyleTypeProperty, value);
        }
        #endregion == StyleType 控件样式==

        #region == Watermark 水印==
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(MPasswordBox));

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
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(BitmapImage), typeof(MPasswordBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Icon 图标
        /// </summary>
        public BitmapImage Icon
        {
            get => (BitmapImage)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        #endregion == Icon 图标==

        #region == FocusedBrush 光标集中==
        public static readonly DependencyProperty FocusedBrushProperty = DependencyProperty.Register("FocusedBrush", typeof(Brush), typeof(MPasswordBox),
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
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MPasswordBox),
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

        #region ==Password==
        /// <summary>
        /// Password
        /// </summary>
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(MPasswordBox), new PropertyMetadata(""));
        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set
            {
                SetValue(PasswordProperty, value);
                OnPasswordChanged();
            }
        }

        private void OnPasswordChanged()
        {

        }

        #endregion ==PercentContent==

        #region ==PasswordChar==
        /// <summary>
        /// Password
        /// </summary>
        public static readonly DependencyProperty PasswordCharProperty = DependencyProperty.Register("PasswordChar", typeof(string), typeof(MPasswordBox), new PropertyMetadata("*"));
        public string PasswordChar
        {
            get => (string)GetValue(PasswordCharProperty);
            set => SetValue(PasswordCharProperty, value);
        }
        #endregion ==PasswordChar==

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (!_isResponseChange)
                return;
            foreach (var c in e.Changes)
            {
                if (c.RemovedLength > 0)
                {
                    Password = Password.Remove(c.Offset, c.RemovedLength);
                    _lastOffset = c.Offset - c.RemovedLength;
                }
                if (c.AddedLength > 0)
                {
                    Password = Password.Insert(c.Offset, Text.Substring(c.Offset, c.AddedLength));
                    _lastOffset = c.Offset;
                }
            }
            TextChangedInvoke(new Action(() =>
            {
                base.Text = ConvertToPasswordChar(Password.Length);
            }));
            this.SelectionStart = _lastOffset + 1;
        }

        private string ConvertToPasswordChar(int length)
        {
            _passwordBuilder?.Clear();
            for (var i = 0; i < length; i++)
                _passwordBuilder.Append(PasswordChar);
            return _passwordBuilder.ToString();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitControl();
            InitResourceData();
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

        private void InitControl()
        {
            _partEye = GetTemplateChild("PART_Eye") as ToggleButton;
            _partEye.Unchecked += PartEyeUnchecked;

            if (!string.IsNullOrEmpty(Password))
            {
                TextChangedInvoke(new Action(() =>
                  {
                      base.Text = ConvertToPasswordChar(Password.Length);
                  }));
            }
        }

        private void PartEyeUnchecked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Password))
                TextChangedInvoke(new Action(() =>
                {
                    base.Text = string.Empty;
                }));

            if (Text.Length == Password.Length)
                return;

            TextChangedInvoke(new Action(() =>
            {
                base.Text = ConvertToPasswordChar(Password.Length);
            }));
        }

        /// <summary>
        /// 屏蔽非主动改变Text事件
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        private void TextChangedInvoke(Delegate method, params object[] args)
        {
            _isResponseChange = false;
            method.DynamicInvoke(args);
            _isResponseChange = true;
        }

        /// <summary>
        /// 屏蔽Text暴露 Password
        /// </summary>
        private new string Text => base.Text;
    }
}
