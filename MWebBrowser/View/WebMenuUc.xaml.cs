using System;
using System.Windows;
using System.Windows.Controls;
using Cys_Controls.Code;
using Cys_CustomControls.Controls;

namespace MWebBrowser.View
{
    /// <summary>
    /// Interaction logic for WebMenu.xaml
    /// </summary>
    public partial class WebMenuUc : UserControl
    {
        public Action ZoomInEvent;
        public Action ZoomOutEvent;
        public Action ZoomResetEvent;

        public Action<string> ExecuteMenuEvent;
        public WebMenuUc()
        {
            InitializeComponent();
            this.Loaded += WebMenuUc_Loaded;
        }

        private void WebMenuUc_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ZoomMenuItem.ZoomInCommand = new BaseCommand<object>(ZoomIn);
            ZoomMenuItem.ZoomOutCommand = new BaseCommand<object>(ZoomOut);
        }

        private void ZoomIn(object obj)
        {
            ZoomInEvent?.Invoke();
        }

        private void ZoomOut(object obj)
        {
            ZoomOutEvent?.Invoke();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is MMenuItem menuItem)) return;
            if (menuItem.Tag == null) return;
            ExecuteMenuEvent?.Invoke(menuItem.Tag.ToString());
        }

        public void ZoomCallBack(string zoomRatio)
        {
            ZoomMenuItem.ZoomRatio = zoomRatio;
        }
    }
}
