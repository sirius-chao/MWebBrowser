using Cys_CustomControls.Controls;
using MWebBrowser.Code.Helpers;

namespace MWebBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MMainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ImageHelper.InitImages();
        }
    }
}
