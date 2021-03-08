using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MWebBrowser.ViewModel;

namespace MWebBrowser.View.History
{
    /// <summary>
    /// Interaction logic for HistoryItemUc.xaml
    /// </summary>
    public partial class HistoryItemUc : UserControl
    {
        public HistoryItemUc()
        {
            InitializeComponent();
        }

        private void History_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (this.DataContext is HistoryItemViewModel viewModel)
            {
                viewModel.BackColorBrush = Application.Current.MainWindow?.FindResource("WebBrowserBrushes.HistoryBackgroundOver") as SolidColorBrush;
                viewModel.DateVisible = Visibility.Collapsed;
                viewModel.CloseVisible = Visibility.Visible;
            }
        }

        private void History_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (this.DataContext is HistoryItemViewModel viewModel)
            {
                viewModel.BackColorBrush = Application.Current.MainWindow?.FindResource("WebBrowserBrushes.HistoryBackground") as SolidColorBrush;
                viewModel.DateVisible = Visibility.Visible;
                viewModel.CloseVisible = Visibility.Collapsed;
            }
        }

        private void History_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
