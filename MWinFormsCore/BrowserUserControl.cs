using CefSharp.WinForms;
using MWinFormsCore.CustomCef;

namespace MWinFormsCore
{
    public partial class BrowserUserControl : UserControl
    {
        public CustomWebBrowser CefWebBrowser;
        public BrowserUserControl()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            CefWebBrowser = new CustomWebBrowser();
            CefWebBrowser.Dock = DockStyle.Fill;
            browserSplitContainer.Panel1.Controls.Add(CefWebBrowser);
        }
        public void ShowDevToolsDocked()
        {
            this.Invoke(() =>
            {
                if (browserSplitContainer.Panel2Collapsed)
                {
                    browserSplitContainer.Panel2Collapsed = false;
                }
                Control devToolsControl = GetDevToolsControl();
                if (devToolsControl == null || devToolsControl.IsDisposed)
                {
                    CefWebBrowser.ShowDevToolsDocked(browserSplitContainer.Panel2, nameof(devToolsControl), DockStyle.Fill);
                }
            });
        }
        public void CloseDevToolsDocked()
        {
            Control devToolsControl = GetDevToolsControl();
            browserSplitContainer.Panel2.Controls.Remove(devToolsControl);
            devToolsControl?.Dispose();
            if (!browserSplitContainer.Panel2Collapsed)
            {
                browserSplitContainer.Panel2Collapsed = true;
            }
        }
        private Control GetDevToolsControl()
        {
            Control devToolsControl = browserSplitContainer.Panel2.Controls.Find(nameof(devToolsControl), false).FirstOrDefault();
            return devToolsControl;
        }
    }
}
