using MWebBrowser.ViewModel.Setting.SearchEngine;
using System.Windows.Controls;

namespace MWebBrowser.View.Setting.SearchEngine
{
    /// <summary>
    /// SearchEngineUc.xaml 的交互逻辑
    /// </summary>
    public partial class SearchEngineUc : UserControl
    {
        private readonly SearchEngineViewModel viewModel;
        public SearchEngineUc()
        {
            InitializeComponent();
            viewModel= new SearchEngineViewModel();
            this.DataContext = viewModel;
        }
    }
}
