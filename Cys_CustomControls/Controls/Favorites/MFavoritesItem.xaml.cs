using Cys_Controls.Code;
using System.Windows;
using System.Windows.Controls;
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
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.Favorites"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.Favorites;assembly=Cys_CustomControls.Controls.Favorites"
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
    ///     <MyNamespace:MFavoritesItem/>
    ///
    /// </summary>
    public class MFavoritesItem : MenuItem
    {
        static MFavoritesItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MFavoritesItem), new FrameworkPropertyMetadata(typeof(MFavoritesItem)));
        }

        #region == DependencyProperty==
        #region == CornerRadius==
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MFavoritesItem),
            new PropertyMetadata(null));

        /// <summary>
        /// CornerRadius
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        #endregion

        #region == PopupWidth==
        public static readonly DependencyProperty PopupWidthProperty = DependencyProperty.Register("PopupWidth", typeof(double), typeof(MFavoritesItem),
            new PropertyMetadata(null));

        /// <summary>
        /// PopupWidth
        /// </summary>
        public double PopupWidth
        {
            get => (double)GetValue(PopupWidthProperty);
            set => SetValue(PopupWidthProperty, value);
        }
        #endregion

        /// <summary>
        /// IconForeground 字体图标前景色
        /// </summary>
        public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register("IconForeground", typeof(Brush), typeof(MFavoritesItem));
        public Brush IconForeground
        {
            get => (Brush)GetValue(IconForegroundProperty);
            set => SetValue(IconForegroundProperty, value);
        }

        /// <summary>
        /// ItemMargin 
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register("ItemMargin", typeof(Thickness), typeof(MFavoritesItem));
        public Thickness ItemMargin
        {
            get => (Thickness)GetValue(ItemMarginProperty);
            set => SetValue(ItemMarginProperty, value);
        }

        /// <summary>
        /// TextMaxWidth 
        /// </summary>
        public static readonly DependencyProperty TextMaxWidthProperty = DependencyProperty.Register("TextMaxWidth", typeof(double), typeof(MFavoritesItem));
        public double TextMaxWidth
        {
            get => (double)GetValue(TextMaxWidthProperty);
            set => SetValue(TextMaxWidthProperty, value);
        }
        #endregion

        public int Type { get; set; }

        public int Level { get; set; }
        public int NodeId { get; set; }
    }
}