using Cys_Controls.Code;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.TabControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Cys_CustomControls.Controls.TabControl;assembly=Cys_CustomControls.Controls.TabControl"
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
    ///     <MyNamespace:MTabControl/>
    ///
    /// </summary>
    public class MTabControl : TabControl
    {
        static MTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MTabControl), new FrameworkPropertyMetadata(typeof(MTabControl)));
        }      

        #region == StyleType 控件样式==
        /// <summary>
        /// StyleType 控件样式
        /// </summary>
        public static readonly DependencyProperty StyleTypeProperty = DependencyProperty.Register("StyleType", typeof(StyleType), typeof(MTabControl), new PropertyMetadata(StyleType.Default));
        public StyleType StyleType
        {
            get => (StyleType)GetValue(StyleTypeProperty);
            set => SetValue(StyleTypeProperty, value);
        }
        #endregion

        #region == TabCloseCommand==
        /// <summary>
        /// StyleType 控件样式
        /// </summary>
        public static readonly DependencyProperty TabItemCloseCommandProperty = DependencyProperty.Register("TabItemCloseCommand", typeof(ICommand), typeof(MTabControl), new PropertyMetadata(null));

        public ICommand TabItemCloseCommand
        {
            get => (ICommand)GetValue(TabItemCloseCommandProperty);
            set => SetValue(TabItemCloseCommandProperty, value);
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitCommand();
        }

        private void InitCommand()
        {
            TabItemCloseCommand = new BaseCommand<object>(TabItemClose);
        }

        private void TabItemClose(object obj)
        {
            if (obj is TabItem item)
            {
                this.Items.Remove(item);
            }
        }
    }
}
