using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cys_CustomControls.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.Menu"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.Menu;assembly=Cys_CustomControls.Controls.Menu"
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
    ///     <MyNamespace:MMenuItem/>
    ///
    /// </summary>
    public class MMenuItem : MenuItem
    {
        static MMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MMenuItem), new FrameworkPropertyMetadata(typeof(MMenuItem)));
        }

        #region == DependencyProperty==
        #region == CornerRadius==
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MMenuItem),
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
        public static readonly DependencyProperty PopupWidthProperty = DependencyProperty.Register("PopupWidth", typeof(double), typeof(MMenuItem),
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

        #region == IconFontSize==
        public static readonly DependencyProperty IconFontSizeProperty = DependencyProperty.Register("IconFontSize", typeof(double), typeof(MMenuItem),
            new PropertyMetadata(null));

        /// <summary>
        /// PopupWidth
        /// </summary>
        public double IconFontSize
        {
            get => (double)GetValue(IconFontSizeProperty);
            set => SetValue(IconFontSizeProperty, value);
        }
        #endregion

        #region == RoleEx==
        public static readonly DependencyProperty RoleExProperty = DependencyProperty.Register("RoleEx", typeof(MenuItemRoleEx), typeof(MMenuItem),
            new PropertyMetadata(MenuItemRoleEx.None));

        /// <summary>
        /// RoleEx
        /// </summary>
        public MenuItemRoleEx RoleEx
        {
            get => (MenuItemRoleEx)GetValue(RoleExProperty);
            set => SetValue(RoleExProperty, value);
        }
        #endregion

        #region == ZoomInCommand==
        /// <summary>
        /// ZoomInCommand
        /// </summary>
        public static readonly DependencyProperty ZoomInCommandProperty = DependencyProperty.Register("ZoomInCommand", typeof(ICommand), typeof(MMenuItem), new PropertyMetadata(null));

        public ICommand ZoomInCommand
        {
            get => (ICommand)GetValue(ZoomInCommandProperty);
            set => SetValue(ZoomInCommandProperty, value);
        }
        #endregion

        #region == ZoomOutCommand==
        /// <summary>
        /// ZoomOutCommand
        /// </summary>
        public static readonly DependencyProperty ZoomOutCommandProperty = DependencyProperty.Register("ZoomOutCommand", typeof(ICommand), typeof(MMenuItem), new PropertyMetadata(null));

        public ICommand ZoomOutCommand
        {
            get => (ICommand)GetValue(ZoomOutCommandProperty);
            set => SetValue(ZoomOutCommandProperty, value);
        }
        #endregion

        #region == ZoomRatio 缩放比例 ==
        public static readonly DependencyProperty ZoomRatioProperty = DependencyProperty.Register("ZoomRatio", typeof(string), typeof(MMenuItem));
        /// <summary>
        /// ZoomLevelType 缩放比例
        /// </summary>
        public string ZoomRatio
        {
            get => (string)GetValue(ZoomRatioProperty);
            set => SetValue(ZoomRatioProperty, value);
        }
        #endregion

        #endregion

    }
}
