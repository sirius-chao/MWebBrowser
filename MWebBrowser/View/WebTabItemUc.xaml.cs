using CefSharp;
using Cys_Controls.Code;
using MWebBrowser.Code.Helpers;
using MWebBrowser.ViewModel;
using MWinFormsCore;
using MWinFormsCore.CustomCef;
using System;
using System.Windows;
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
        BrowserUserControl browserUserControl;
        public WebTabItemViewModel ViewModel;

        public Action SetCurrentEvent;


        public WebTabItemUc()
        {
            ViewModel = new WebTabItemViewModel();
            this.DataContext = ViewModel;
            InitializeComponent();
            InitWebBrowser();
        }
        public void CefWebBrowser_PreviewKeyDown(int keyCode)
        {
            Keys key = (Keys)Enum.Parse(typeof(Keys), keyCode.ToString());
            if (key == Keys.F5)
            {
                this.CefWebBrowser.Reload();
            }

            if (key == Keys.F11)
            {
                F11Helper.F11(browserUserControl, formsHost);
            }

            if (key == Keys.F12)
            {
                this.browserUserControl.ShowDevToolsDocked();
            }

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control
                && (key == Keys.D0 || key == Keys.NumPad0))
            {
                DispatcherHelper.UIDispatcher.Invoke(() =>
                {
                    var uc = ControlHelper.FindVisualParent<WebTabControlUc>(this);
                    uc?.SearchText.ZoomResetCommand.Execute(null);
                });
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
            browserUserControl = new BrowserUserControl();
            CefWebBrowser = browserUserControl.CefWebBrowser;
            formsHost.Child = browserUserControl;
            CefWebBrowser.IsBrowserInitializedChanged += CefWebBrowser_IsBrowserInitializedChanged;
            this.CefWebBrowser.TitleChanged += CefWebBrowser_TitleChanged;
            if(this.CefWebBrowser.KeyboardHandler is CustomKeyboardHandler handler)
            {
                handler.KeyboardCallBack += CefWebBrowser_PreviewKeyDown;
            }

           // this.CefWebBrowser.ZoomLevelIncrement = _zoomLevelIncrement;
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
