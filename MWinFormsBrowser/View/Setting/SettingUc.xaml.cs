using MWinFormsBrowser.ViewModel;
using MWinFormsBrowser.ViewModel.Setting;
using System.Windows.Controls;

namespace MWinFormsBrowser.View
{
    /// <summary>
    /// Interaction logic for DownloadShowAllUc.xaml
    /// </summary>
    public partial class SettingUc : UserControl
    {
        private readonly SettingViewModel settingViewModel;
        public SettingUc()
        {
            InitializeComponent();
            settingViewModel = new SettingViewModel();
            this.DataContext = settingViewModel; 
        }
    }
}
