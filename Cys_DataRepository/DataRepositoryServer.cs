namespace Cys_DataRepository
{
    public class DataRepositoryServer
    {
        public static DataRepositoryServer Instance { get; } = new DataRepositoryServer();

        private readonly DownloadDataRepository _downloadData = new DownloadDataRepository();
        public DownloadDataRepository DownloadData => _downloadData;
    }
}
