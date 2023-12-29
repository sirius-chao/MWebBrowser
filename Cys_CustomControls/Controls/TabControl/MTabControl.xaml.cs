using Cys_Controls.Code;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        public Grid PartHeaderParentGrid;
        private ColumnDefinition _partHeaderPanelColumn;

        private TabItem draggedItem;
        private Point dropstartPoint;
        public Action CloseTabEvent;
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
        /// TabItemRemoveCommand
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
        /// TabItemAddCommand
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
            AllowDrop = true;
        }
        private void InitControl()
        {
            _partHeaderPanel = GetTemplateChild("PART_HeaderPanel") as UniformGrid;
            _partHeaderPanelColumn = GetTemplateChild("PART_HeaderPanelColumn") as ColumnDefinition;
            PartHeaderParentGrid = GetTemplateChild("PART_HeaderParentGrid") as Grid;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            SetHeaderPanelWidth();
        }

        public void SetHeaderPanelWidth()
        {
            if (this.ActualWidth <= 200) return;
            double totalWidth = this.Items.Count * TabItemMaxWidth;
            _partHeaderPanelColumn.Width = totalWidth > this.ActualWidth - PlaceHolderMinWidth
                ? new GridLength
                    (this.ActualWidth - PlaceHolderMinWidth) : new GridLength(totalWidth);
        }
        private void InitCommand()
        {
            TabItemRemoveCommand = new BaseCommand<object>(TabItemRemove);
        }

        private void TabItemRemove(object obj)
        {
            if (obj is TabItem item)
            {
                this.Items.Remove(item);
            }
            SetHeaderPanelWidth();

            if (this.Items.Count <= 0)
            {
                CloseTabEvent?.Invoke();
            }
        }

        #region drap&drop
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            dropstartPoint = e.GetPosition(null);
            draggedItem = e.Source as TabItem;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton == MouseButtonState.Pressed && draggedItem != null)
            {
                Point mousePos = e.GetPosition(null);
                Vector diff = dropstartPoint - mousePos;

                if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    DragDrop.DoDragDrop(draggedItem, draggedItem, DragDropEffects.Move);
                }
            }
        }
        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            if (e.Data.GetDataPresent(typeof(TabItem)))
            {
                TabItem droppedItem = e.Data.GetData(typeof(TabItem)) as TabItem;
                if (droppedItem != null)
                {
                    int oldIndex = Items.IndexOf(droppedItem);
                    if (oldIndex != -1)
                    {
                        Items.RemoveAt(oldIndex);
                        Point mousePos = e.GetPosition(this);
                        UIElement target = VisualTreeHelper.HitTest(this, mousePos).VisualHit as UIElement;
                        TabItem newTabItem = FindParentTabItem<TabItem>(target);

                        if (newTabItem != null)
                        {
                            int newIndex = Items.IndexOf(newTabItem);

                            if (newIndex != -1)
                            {
                                Items.Insert(newIndex, droppedItem);
                            }
                        }
                    }
                }
            }
            draggedItem = null;
        }
        private T FindParentTabItem<T>(UIElement element) where T : UIElement
        {
            while (element != null && !(element is T))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
            }
            return element as T;
        }
        #endregion
    }
}
