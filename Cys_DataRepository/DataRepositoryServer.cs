namespace Cys_DataRepository
{
    public class DataRepositoryServer
    {
        public static DataRepositoryServer Instance { get; } = new DataRepositoryServer();

        public DownloadDataRepository DownloadData { get; } = new DownloadDataRepository();
        public FavoritesDataRepository FavoritesData { get; } = new FavoritesDataRepository();
    }
}
