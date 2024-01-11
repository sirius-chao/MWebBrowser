using CefSharp;
using Cys_Controls.Code;
using MWebBrowser.Code.Helpers;
using MWebBrowser.ViewModel;
using MWPFCore.Code.CustomCef;
using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace MWebBrowser.View
{
    /// <summary>
    /// Interaction logic for WebTabItem.xaml
    /// </summary>
    public partial class WebTabItemUc : System.Windows.Controls.UserControl
    {
        public CustomWebBrowser CefWebBrowser;
        public WebTabItemViewModel ViewModel;
        public Action<object, System.Windows.Forms.MouseEventArgs> WebMouseWheelEvent;

        public Action SetCurrentEvent;
        private readonly double _zoomLevelIncrement = 0.2;//默认为0.1


        public WebTabItemUc()
        {
            ViewModel = new WebTabItemViewModel();
            this.DataContext = ViewModel;
            InitializeComponent();
            InitWebBrowser();
        }


        private void CefWebBrowser_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            WebMouseWheelEvent?.Invoke(sender, e);
        }


        private void CefWebBrowser_PreviewKeyDown(object sender, System.Windows.Forms.PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                this.CefWebBrowser.Reload();
            }

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control
                && (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0))
            {
                var uc = ControlHelper.FindVisualParent<WebTabControlUc>(this);
                uc?.SearchText.ZoomResetCommand.Execute(null);
            }
        }

        private void CefWebBrowser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            string address = CefWebBrowser.Address;
            ViewModel.Title = e.Title;
            DispatcherHelper.UIDispatcher.Invoke(() =>
            {
                ViewModel.Favicon = ImageHelper.GetFavicon(address);
            });
            ViewModel.CurrentUrl = address;
            SetCurrentEvent?.Invoke();
        }
        private void InitWebBrowser()
        {
            CefWebBrowser = new CustomWebBrowser();
            formsHost.Child = CefWebBrowser;
            CefWebBrowser.IsBrowserInitializedChanged += CefWebBrowser_IsBrowserInitializedChanged;
            this.CefWebBrowser.TitleChanged += CefWebBrowser_TitleChanged;
            this.CefWebBrowser.PreviewKeyDown += CefWebBrowser_PreviewKeyDown;
           // this.CefWebBrowser.ZoomLevelIncrement = _zoomLevelIncrement;
            this.CefWebBrowser.MouseWheel += CefWebBrowser_MouseWheel;
        }

        private void CefWebBrowser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            try
            {
                DispatcherHelper.UIDispatcher.Invoke(() =>
                {
                    if (!CefWebBrowser.IsBrowserInitialized) return;
                    CefWebBrowser.Focus();//浏览器初始化完毕后获得焦点
                    if (!string.IsNullOrEmpty(ViewModel.CurrentUrl))
                    {
                        Load(ViewModel.CurrentUrl);
                    }
                });
            }
            catch (Exception ex)
            {

            }
        }

        public void Load(string url)
        {
            CefWebBrowser.Load(url);
        }

        public void Dispose()
        {
            CefWebBrowser?.Dispose();
            CefWebBrowser = null;
        }
    }
}
