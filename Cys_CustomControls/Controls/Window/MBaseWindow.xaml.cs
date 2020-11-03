using Cys_Controls.Code;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;

// ReSharper disable once CheckNamespace
namespace Cys_CustomControls.Controls
{
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.Window"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.Window;assembly=Cys_CustomControls.Controls.Window"
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
    public class MBaseWindow : Window
    {
        #region 变量
        private Rect _rect = new Rect();
        private HwndSource _hwndSource;
        public WindowState WinState = WindowState.Normal;
        #endregion
        static MBaseWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MBaseWindow), new FrameworkPropertyMetadata(typeof(MBaseWindow)));
        }

        #region UI Element

        private Grid _resizeGrid;
        private Border _borderTitle;
        private Button _closeBtn;
        private Button _minBtn;
        private Button _maxBtn;
        private Line _titleLine;

        #endregion
        #region DependencyProperty

        #region TitleForeground
        public static readonly DependencyProperty TitleForegroundProperty = DependencyProperty.Register("TitleForeground", typeof(Brush), typeof(MBaseWindow),
            new PropertyMetadata());

        /// <summary>
        /// Title前景色
        /// </summary>
        public Brush TitleForeground
        {
            get => (Brush)GetValue(TitleForegroundProperty);
            set => SetValue(TitleForegroundProperty, value);
        }
        #endregion

        #region TitleBackground
        public static readonly DependencyProperty TitleBackgroundProperty = DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(MBaseWindow),
            new PropertyMetadata());

        /// <summary>
        /// Title背景色
        /// </summary>
        public Brush TitleBackground
        {
            get => (Brush)GetValue(TitleBackgroundProperty);
            set => SetValue(TitleBackgroundProperty, value);
        }
        #endregion

        #region BottomForeground
        public static readonly DependencyProperty BottomForegroundProperty = DependencyProperty.Register("BottomForeground", typeof(Brush), typeof(MBaseWindow),
            new PropertyMetadata());

        /// <summary>
        /// Bottom前景色
        /// </summary>
        public Brush BottomForeground
        {
            get => (Brush)GetValue(BottomForegroundProperty);
            set => SetValue(BottomForegroundProperty, value);
        }
        #endregion

        #region BottomBackground
        public static readonly DependencyProperty BottomBackgroundProperty = DependencyProperty.Register("BottomBackground", typeof(Brush), typeof(MBaseWindow),
            new PropertyMetadata());

        /// <summary>
        /// Bottom背景色
        /// </summary>
        public Brush BottomBackground
        {
            get => (Brush)GetValue(BottomBackgroundProperty);
            set => SetValue(BottomBackgroundProperty, value);
        }
        #endregion

        #region CanDropBorder
        public static readonly DependencyProperty CanDropBorderProperty = DependencyProperty.Register("CanDropBorder", typeof(bool), typeof(MBaseWindow),
            new PropertyMetadata(true));

        /// <summary>
        /// 边框是否可拖拽
        /// </summary>
        public bool CanDropBorder
        {
            get => (bool)GetValue(CanDropBorderProperty);
            set => SetValue(CanDropBorderProperty, value);
        }
        #endregion

        #region 显示最大化
        public static readonly DependencyProperty HasMaximizeProperty = DependencyProperty.Register("HasMaximize", typeof(bool), typeof(MBaseWindow),
            new PropertyMetadata(true));

        /// <summary>
        /// 显示最大化
        /// </summary>
        public bool HasMaximize
        {
            get => (bool)GetValue(HasMaximizeProperty);
            set => SetValue(HasMaximizeProperty, value);
        }
        #endregion

        #region 显示最小化
        public static readonly DependencyProperty HasMinimizeProperty = DependencyProperty.Register("HasMinimize", typeof(bool), typeof(MBaseWindow),
            new PropertyMetadata(true));

        /// <summary>
        /// 显示最小化
        /// </summary>
        public bool HasMinimize
        {
            get => (bool)GetValue(HasMinimizeProperty);
            set => SetValue(HasMinimizeProperty, value);
        }
        #endregion

        #region 显示关闭
        public static readonly DependencyProperty HasCloseProperty = DependencyProperty.Register("HasClose", typeof(bool), typeof(MBaseWindow),
            new PropertyMetadata(true));

        /// <summary>
        /// 显示关闭
        /// </summary>
        public bool HasClose
        {
            get => (bool)GetValue(HasCloseProperty);
            set => SetValue(HasCloseProperty, value);
        }
        #endregion

        #endregion
        protected virtual void CloseWindow()
        {
            if (null != CloseAction)
            {
                CloseAction.Invoke(null);
            }
            else
            {
                this.Close();
            }
        }
        protected event Action<string> CloseAction;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitControl();
        }
        private void InitControl()
        {
            _resizeGrid = GetTemplateChild("PART_ResizeGrid") as Grid;
            _borderTitle = GetTemplateChild("PART_BorderTitle") as Border;
            _closeBtn = GetTemplateChild("PART_BtnClose") as Button;
            _minBtn = GetTemplateChild("PART_BtnMin") as Button;
            _maxBtn = GetTemplateChild("PART_BtnMax") as Button;
            _titleLine = GetTemplateChild("PART_TitleLine") as Line;
            _maxBtn.Click += (sender, args) => DoubleClickHeader();
            _minBtn.Click += (sender, args) => this.WindowState = WindowState.Minimized;
            _closeBtn.Click += (sender, args) => CloseWindow();
            _borderTitle.MouseLeftButtonDown += HeaderClickOrDragMove;
            _hwndSource = (HwndSource)PresentationSource.FromVisual(this);
            if (WindowState != WindowState.Normal)
            {
                HideShell();
            }
            else
            {
                ShowShell();
            }
        }

        #region 屏幕最大最小化

        /// <summary>
        /// 双击切换窗口
        /// </summary>
        public void DoubleClickHeader()
        {
            if (WinState != WindowState.Maximized)
            {
                GoFullscreen();
                HideShell();
            }
            else
            {
                ExitFullscreen();
                ShowShell();
            }
        }
        /// <summary>
        /// 进入全屏    
        /// </summary>
        public void GoFullscreen()
        {
            if (WinState == WindowState.Maximized && this.WindowState != WindowState.Minimized)
                return;
            var isFromMin = this.WindowState == WindowState.Minimized;
            this.WindowState = WindowState.Normal;
            WinState = WindowState.Maximized;


            if (!isFromMin)
            {
                _rect = new Rect(this.Left, this.Top, this.Width, this.Height);
            }
            //获取窗口句柄 
            var handle = new WindowInteropHelper(this).Handle;
            //获取当前显示器屏幕
            this.Left = SystemParameters.WorkArea.Left;
            this.Top = SystemParameters.WorkArea.Top;
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            this.ShowInTaskbar = true;
        }
        /// <summary>
        /// 还原屏幕大小
        /// </summary>
        public void ExitFullscreen()
        {
            this.WindowState = WindowState.Normal;
            WinState = WindowState.Normal;
            this.Left = _rect.Left;
            this.Top = _rect.Top;
            this.Width = _rect.Width;
            this.Height = _rect.Height;
            this.ShowInTaskbar = true;
        }

        #endregion

        #region 窗体大小 拖拽
        /// <summary>
        /// 主窗体头部点击拖拽
        /// </summary>
        public void HeaderClickOrDragMove(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;
            if (!CanDropBorder)
                return;

            if (e.ClickCount > 1)
            {
                DoubleClickHeader();
                if (WinState == WindowState.Maximized)
                {
                    HideShell();
                }
                else
                {
                    ShowShell();
                }
                e.Handled = true;
            }
            else
            {
                if (e.LeftButton == MouseButtonState.Pressed && WinState == WindowState.Normal)
                {
                    this.DragMove();
                }
            }
        }
        /// <summary>
        /// 隐藏拖拽边框改变大小
        /// </summary>
        public void HideShell()
        {
            try
            {
                _maxBtn.Tag = "Maximized";
                foreach (UIElement element in _resizeGrid.Children)
                {
                    if (!(element is Rectangle resizeRectangle)) continue;
                    resizeRectangle.PreviewMouseDown -= ResizeRectangle_PreviewMouseDown;
                    resizeRectangle.MouseEnter -= ResizeRectangle_MouseEnter;
                    resizeRectangle.MouseLeave -= ResizeRectangle_MouseLeave;
                    resizeRectangle.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception e)
            {

            }
        }
        /// <summary>
        /// 拖拽边框改变大小
        /// </summary>
        public void ShowShell()
        {
            try
            {
                _maxBtn.Tag = "";
                foreach (UIElement element in _resizeGrid.Children)
                {
                    if (!(element is Rectangle resizeRectangle)) continue;
                    resizeRectangle.Visibility = Visibility.Visible;
                    resizeRectangle.PreviewMouseDown += ResizeRectangle_PreviewMouseDown;
                    resizeRectangle.MouseEnter += ResizeRectangle_MouseEnter;
                    resizeRectangle.MouseLeave += ResizeRectangle_MouseLeave;
                }
            }
            catch (Exception e)
            {

            }
        }

        private void ResizeRectangle_MouseLeave(object sender, MouseEventArgs e) => this.Cursor = Cursors.Arrow;
        /// <summary>
        /// 按下拖动区域发生事件
        /// </summary>
        private void ResizeRectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Rectangle rectangle)) return;
            if (e.LeftButton != MouseButtonState.Pressed) return;
            if (!CanDropBorder) return;

            switch (rectangle.Name)
            {
                case "Top":
                    this.Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Top);
                    break;
                case "Bottom":
                    this.Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Bottom);
                    break;
                case "Left":
                    this.Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Left);
                    break;
                case "Right":
                    this.Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Right);
                    break;
                case "TopLeft":
                    this.Cursor = Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.TopLeft);
                    break;
                case "TopRight":
                    this.Cursor = Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.TopRight);
                    break;
                case "BottomLeft":
                    this.Cursor = Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.BottomLeft);
                    break;
                case "BottomRight":
                    this.Cursor = Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.BottomRight);
                    break;
            }
        }
        /// <summary>
        /// 重绘Window
        /// </summary>
        private void ResizeWindow(ResizeDirection direction) => SendMessage(_hwndSource.Handle, 0x112, (IntPtr)(61440 + direction), IntPtr.Zero);
        /// <summary>
        /// 拖动区域内移动前事件
        /// </summary>
        private void ResizeRectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!CanDropBorder) return;
            if (!(sender is Rectangle rectangle)) return;
            switch (rectangle.Name)
            {
                case "Top":
                    this.Cursor = Cursors.SizeNS;
                    break;
                case "Bottom":
                    this.Cursor = Cursors.SizeNS;
                    break;
                case "Left":
                    this.Cursor = Cursors.SizeWE;
                    break;
                case "Right":
                    this.Cursor = Cursors.SizeWE;
                    break;
                case "TopLeft":
                    this.Cursor = Cursors.SizeNWSE;
                    break;
                case "TopRight":
                    this.Cursor = Cursors.SizeNESW;
                    break;
                case "BottomLeft":
                    this.Cursor = Cursors.SizeNESW;
                    break;
                case "BottomRight":
                    this.Cursor = Cursors.SizeNWSE;
                    break;
                default:
                    break;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);
        #endregion
    }
}
