using System;
using System.Windows.Controls;
using Cys_Controls.Code;

namespace MWebBrowser.View.WebBrowser
{
    /// <summary>
    /// Interaction logic for WebMenu.xaml
    /// </summary>
    public partial class WebMenuUc : UserControl
    {
        public Action ZoomInEvent;
        public Action ZoomOutEvent;
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
    }
}
