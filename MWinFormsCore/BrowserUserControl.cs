using CefSharp;
using CefSharp.WinForms;
using MWPFCore.Code.CustomCef;

namespace MWinFormsCore
{
    public partial class BrowserUserControl : UserControl
    {
        public CustomWebBrowser Browser;
        public BrowserUserControl()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            Browser = new CustomWebBrowser();
            Browser.Dock = DockStyle.Fill;
            browserSplitContainer.Panel1.Controls.Add(Browser);
            browserSplitContainer.SplitterMoved += BrowserSplitContainer_SplitterMoved;
        }

        private void BrowserSplitContainer_SplitterMoved(object? sender, SplitterEventArgs e)
        {
            if (!browserSplitContainer.Panel2Collapsed)
            {
                if (browserSplitContainer.Panel2.Controls.Count <= 0) return;

                var devToolsControl = browserSplitContainer.Panel2.Controls[0];
                if (devToolsControl != null)
                {
                    devToolsControl.Width = browserSplitContainer.Panel2.Width;
                    devToolsControl.Height = browserSplitContainer.Panel2.Height;
                    devToolsControl.Dock = DockStyle.Fill;
                }
            }
        }

        public void ShowDevToolsDocked()
        {
            this.Invoke(() =>
            {
                if (browserSplitContainer.Panel2Collapsed)
                {
                    browserSplitContainer.Panel2Collapsed = false;
                }

                //Find devToolsControl in Controls collection
                Control devToolsControl = null;
                devToolsControl = browserSplitContainer.Panel2.Controls.Find(nameof(devToolsControl), false).FirstOrDefault();
                if (devToolsControl == null || devToolsControl.IsDisposed)
                {
                    devToolsControl = Browser.ShowDevToolsDocked(browserSplitContainer.Panel2, nameof(devToolsControl), DockStyle.Fill);

                    EventHandler devToolsPanelDisposedHandler = null;
                    devToolsPanelDisposedHandler = (s, e) =>
                    {
                        browserSplitContainer.Panel2.Controls.Remove(devToolsControl);
                        browserSplitContainer.Panel2Collapsed = true;
                        devToolsControl.Disposed -= devToolsPanelDisposedHandler;
                    };

                    //Subscribe for devToolsPanel dispose event
                    devToolsControl.Disposed += devToolsPanelDisposedHandler;
                }
            });
        }

        public void ShowDevTools()
        {
            ShowDevToolsDocked();
        }
    }
}
