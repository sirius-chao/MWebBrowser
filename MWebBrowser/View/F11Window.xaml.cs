using System.Windows;
using System.Windows.Forms.Integration;

namespace MWebBrowser.View
{
    /// <summary>
    /// F11Window.xaml 的交互逻辑
    /// </summary>
    public partial class F11Window : Window
    {
        public WindowsFormsHost WebFormsHost { get; }
        public F11Window()
        {
            InitializeComponent();
            WebFormsHost = new WindowsFormsHost();
            InitData();
        }

        private void InitData()
        {
            Parent.Children.Add(WebFormsHost);
        }
    }
}
