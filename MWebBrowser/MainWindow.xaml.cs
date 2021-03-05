using System.Windows;
using Cys_Common;
using Cys_CustomControls.Controls;
using Cys_DataRepository;
using Cys_Model.DataBase;
using MWebBrowser.Code.Configure;
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
        }

        private void SaveGlobalInfo()
        {
            DataRepositoryServer.Instance.DownloadData.SaveDownloadSetting();
            DataRepositoryServer.Instance.FavoritesData.SaveFavoritesSetting();
        }
    }
}
