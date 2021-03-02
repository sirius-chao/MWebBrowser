using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.TreeView"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.TreeView;assembly=Cys_CustomControls.Controls.TreeView"
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
    ///     <MyNamespace:MTreeViewItem/>
    ///
    /// </summary>
    public class MTreeViewItem : TreeViewItem
    {
        static MTreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MTreeViewItem), new FrameworkPropertyMetadata(typeof(MTreeViewItem)));
        }

        /// <summary>
        /// Icon
        /// </summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(string), typeof(MTreeViewItem));
        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        /// <summary>
        /// IsExpanded
        /// </summary>
        public static readonly DependencyProperty IsExpandedIconProperty = DependencyProperty.Register("IsExpandedIcon", typeof(string), typeof(MTreeViewItem));
        public string IsExpandedIcon
        {
            get => (string)GetValue(IsExpandedIconProperty);
            set => SetValue(IsExpandedIconProperty, value);
        }

        /// <summary>
        /// IconForeground 字体图标前景色
        /// </summary>
        public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register("IconForeground", typeof(Brush), typeof(MTreeViewItem));
        public Brush IconForeground
        {
            get => (Brush)GetValue(IconForegroundProperty);
            set => SetValue(IconForegroundProperty, value);
        }

        /// <summary>
        /// ItemMargin 
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register("ItemMargin", typeof(Thickness), typeof(MTreeViewItem));
        public Thickness ItemMargin
        {
            get => (Thickness)GetValue(ItemMarginProperty);
            set => SetValue(ItemMarginProperty, value);
        }

        /// <summary>
        /// TextMaxWidth 
        /// </summary>
        public static readonly DependencyProperty TextMaxWidthProperty = DependencyProperty.Register("TextMaxWidth", typeof(double), typeof(MTreeViewItem));
        public double TextMaxWidth
        {
            get => (double)GetValue(TextMaxWidthProperty);
            set => SetValue(TextMaxWidthProperty, value);
        }

        /// <summary>
        /// IsEdit 
        /// </summary>
        public static readonly DependencyProperty IsEditProperty = DependencyProperty.Register("IsEdit", typeof(bool), typeof(MTreeViewItem), new UIPropertyMetadata(IsEditUpdate));
        public bool IsEdit
        {
            get => (bool)GetValue(IsEditProperty);
            set => SetValue(IsEditProperty, value);
        }

        /// <summary>
        /// EditText
        /// </summary>
        public static readonly DependencyProperty EditTextProperty = DependencyProperty.Register("EditText", typeof(string), typeof(MTreeViewItem));

        private static void IsEditUpdate(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is MTreeViewItem item)) return;
            if (!item.IsEdit) return;
            var textBox = ControlHelper.FindVisualChild<TextBox>(d);
            textBox.Focus();//不好用后期处理
            FocusManager.SetFocusedElement(d,textBox);//不好用后期处理
        }

        public string EditText
        {
            get => (string)GetValue(EditTextProperty);
            set => SetValue(EditTextProperty, value);
        }
        public int Type { get; set; }

        public int Level { get; set; }
        public int NodeId { get; set; }
    }
}
