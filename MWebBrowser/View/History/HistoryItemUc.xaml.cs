using Cys_Controls.Code;
using MWebBrowser.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
            if (!(this.DataContext is HistoryItemViewModel viewModel)) return;

            viewModel.BackColorBrush = Application.Current.MainWindow?.FindResource("WebBrowserBrushes.HistoryBackgroundOver") as SolidColorBrush;
            viewModel.DateVisible = Visibility.Collapsed;
            viewModel.CloseVisible = Visibility.Visible;
        }

        private void History_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!(this.DataContext is HistoryItemViewModel viewModel)) return;

            viewModel.BackColorBrush = Application.Current.MainWindow?.FindResource("WebBrowserBrushes.HistoryBackground") as SolidColorBrush;
            viewModel.DateVisible = Visibility.Visible;
            viewModel.CloseVisible = Visibility.Collapsed;
        }

        private void History_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(this.DataContext is HistoryItemViewModel viewModel)) return;
            try
            { 
               var uc = ControlHelper.FindVisualChild<WebTabControlUc>(Application.Current.MainWindow);
               uc.TabItemAdd(viewModel.Url);
            }
            catch (Exception ex)
            {
            
            }
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(this.DataContext is HistoryItemViewModel viewModel)) return;
            var uc = ControlHelper.FindVisualChild<WebTabControlUc>(Application.Current.MainWindow);
            var historyUc = ControlHelper.FindVisualChild<HistoryUc>(uc);
            if (historyUc?.DataContext is HistoryViewModel hvm)
            {
                hvm.DeleteHistoryItem(viewModel);
            }
        }
    }
}
