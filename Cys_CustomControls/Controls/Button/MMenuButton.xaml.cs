using System;
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
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.Button"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.Button;assembly=Cys_CustomControls.Controls.Button"
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
    ///     <MyNamespace:MMenuButton/>
    ///
    /// </summary>
    public class MMenuButton : Button
    {
        static MMenuButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MMenuButton),
                new FrameworkPropertyMetadata(typeof(MMenuButton)));
        }

        #region UI Element

        private Grid _backGrid;
        private GradientStop _gradientStop;

        #endregion
        #region DependencyProperty

        #region == Icon ==

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon",
            typeof(ImageSource), typeof(MMenuButton),
            new PropertyMetadata(null));

        /// <summary>
        /// Icon 图标
        /// </summary>
        public ImageSource Icon
        {
            get => (ImageSource) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        #endregion

        #region == IconBackground ==

        public static readonly DependencyProperty IconBackgroundProperty = DependencyProperty.Register("IconBackground",
            typeof(Brush), typeof(MMenuButton),
            new PropertyMetadata(null));

        /// <summary>
        /// Icon 图标
        /// </summary>
        public Brush IconBackground
        {
            get => (Brush) GetValue(IconBackgroundProperty);
            set => SetValue(IconBackgroundProperty, value);
        }

#endregion

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitControl();
        }

        private void InitControl()
        {
            _backGrid = GetTemplateChild("PART_BackGrid") as Grid;
            _gradientStop = GetTemplateChild("PART_GradientStop") as GradientStop;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            try
            {
                var point = e.GetPosition(_backGrid);
                var offset = point.X / _backGrid.ActualWidth;
                if (offset > 1)
                {
                    offset = 1;
                }
                else if (offset < 0)
                {
                    offset = 0;
                }
                _gradientStop.Offset = offset;
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
