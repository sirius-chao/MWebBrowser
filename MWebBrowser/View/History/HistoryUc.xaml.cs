using Cys_Controls.Code;
using MWebBrowser.ViewModel;
using System.Windows.Controls;

namespace MWebBrowser.View
{
    /// <summary>
    /// Interaction logic for HistoryUc.xaml
    /// </summary>
    public partial class HistoryUc : UserControl
    {
        private readonly HistoryViewModel _viewModel;
        private double _offset;
        public HistoryUc()
        {
            InitializeComponent();
            _viewModel = new HistoryViewModel();
            this.DataContext = _viewModel;
            HistoryListBox.DataContext = _viewModel;
        }
        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (!(sender is ScrollViewer scrollViewer)) return;
            if (_offset > scrollViewer.VerticalOffset) return;
            _offset = scrollViewer.VerticalOffset;
            if ((int)scrollViewer.VerticalOffset >= (scrollViewer.ScrollableHeight - 3))
            {
                _viewModel.GetHistoryList();
            }
        }
        private void HistoryButton_OnChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            ScrollViewer sv = ControlHelper.FindVisualChild<ScrollViewer>(HistoryListBox);
            if (sv != null)
            {
                sv.ScrollToTop();
                sv.ScrollChanged -= ScrollChanged;
                sv.ScrollChanged += ScrollChanged;
            }
            _viewModel.ReSet();
            _viewModel.GetHistoryList();
        }

        private void HistoryButton_OnUnchecked(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
