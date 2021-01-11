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

        public int Type { get; set; }
        public int NodeId { get; set; }
    }
}
