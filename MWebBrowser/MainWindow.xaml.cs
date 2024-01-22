using Cys_Common.Code.Configure;
using Cys_Common.Settings;
using Cys_CustomControls.Controls;
using Cys_DataRepository;
using Cys_Model.DataBase;
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
            ConfigHelper.LoadLocalConfig();
            InitGlobalInfo();
            this.Closing += MainWindow_Closing;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            MLogger.Info("Main Window Show Success!");
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            SaveGlobalInfo();
        }

        private void InitGlobalInfo()
        {
            DbSeed.InitData();
            GlobalInfo.DownloadSetting = DataRepositoryServer.Instance.DownloadData.GetDownloadSetting();
            GlobalInfo.FavoritesSetting = DataRepositoryServer.Instance.FavoritesData.GetFavoritesSetting();
            GlobalInfo.SearchEngineSetting = DataRepositoryServer.Instance.SearchEngineData.GetSearchEngineSetting();
        }

        private void SaveGlobalInfo()
        {
            DataRepositoryServer.Instance.DownloadData.SaveDownloadSetting();
            DataRepositoryServer.Instance.FavoritesData.SaveFavoritesSetting();
            DataRepositoryServer.Instance.SearchEngineData.SaveSearchEngineSetting();
        }
    }
}
