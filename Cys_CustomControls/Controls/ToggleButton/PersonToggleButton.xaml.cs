using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

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
    ///     <MyNamespace:PersonToggleButton/>
    ///
    /// </summary>
    public class PersonToggleButton : ToggleButton
    {
        static PersonToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PersonToggleButton), new FrameworkPropertyMetadata(typeof(PersonToggleButton)));
        }

        #region ==ButtonImage 按钮图片==
        /// <summary>
        /// ButtonImage
        /// </summary>
        public static readonly DependencyProperty ButtonImageProperty =
            DependencyProperty.Register("ButtonImage", typeof(ImageSource), typeof(PersonToggleButton));
        public ImageSource ButtonImage
        {
            get => (ImageSource)GetValue(ButtonImageProperty); 
            set => SetValue(ButtonImageProperty, value);
        }
        #endregion ==ButtonImage==
    }
}
