using Cys_Controls.Code;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        private const double PlaceHolderMinWidth = 200;
        private const double TabItemMaxWidth = 240;
        private UniformGrid _partHeaderPanel;
        private ColumnDefinition _partHeaderPanelColumn;

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

        #region == TabItemRemoveCommand==
        /// <summary>
        /// StyleType 控件样式
        /// </summary>
        public static readonly DependencyProperty TabItemRemoveCommandProperty = DependencyProperty.Register("TabItemRemoveCommand", typeof(ICommand), typeof(MTabControl), new PropertyMetadata(null));

        public ICommand TabItemRemoveCommand
        {
            get => (ICommand)GetValue(TabItemRemoveCommandProperty);
            set => SetValue(TabItemRemoveCommandProperty, value);
        }
        #endregion

        #region == TabItemAddCommand==
        /// <summary>
        /// StyleType 控件样式
        /// </summary>
        public static readonly DependencyProperty TabItemAddCommandProperty = DependencyProperty.Register("TabItemAddCommand", typeof(ICommand), typeof(MTabControl), new PropertyMetadata(null));

        public ICommand TabItemAddCommand
        {
            get => (ICommand)GetValue(TabItemAddCommandProperty);
            set => SetValue(TabItemAddCommandProperty, value);
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitCommand();
            InitControl();
            TabItemAdd(1);
        }
        private void InitControl()
        {
            _partHeaderPanel = GetTemplateChild("PART_HeaderPanel") as UniformGrid;
            _partHeaderPanelColumn = GetTemplateChild("PART_HeaderPanelColumn") as ColumnDefinition;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            SetHeaderPanelWidth();
        }

        private void SetHeaderPanelWidth()
        {
            if (this.ActualWidth <= 200) return;
            double totalWidth = this.Items.Count * TabItemMaxWidth;
            _partHeaderPanelColumn.Width = totalWidth > this.ActualWidth - PlaceHolderMinWidth
                ? new GridLength(this.ActualWidth - PlaceHolderMinWidth) : new GridLength(totalWidth);
        }
        private void InitCommand()
        {
            TabItemAddCommand = new BaseCommand<object>(TabItemAdd);
            TabItemRemoveCommand = new BaseCommand<object>(TabItemRemove);
        }

        private void TabItemRemove(object obj)
        {
            if (obj is TabItem item)
            {
                this.Items.Remove(item);
            }
            SetHeaderPanelWidth();
        }
        private void TabItemAdd(object obj)
        {
            TabItem item = new TabItem {Header = "新标签页"};
            this.Items.Add(item);
            SetHeaderPanelWidth();
        }
    }
}
