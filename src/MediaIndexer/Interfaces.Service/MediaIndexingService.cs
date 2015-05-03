using log4net;
using Pihalve.MediaIndexer.Application;

namespace Pihalve.MediaIndexer.Interfaces.Service
{
    public class MediaIndexingService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IFileSystemMonitor _fileSystemMonitor;

        public MediaIndexingService(IFileSystemMonitor fileSystemMonitor)
        {
            _fileSystemMonitor = fileSystemMonitor;
        }

        public void Start()
        {
            _fileSystemMonitor.Start();

            Log.Info("MediaIndexingService started");
        }

        public void Stop()
        {
            _fileSystemMonitor.Stop();

            Log.Info("MediaIndexingService stopped");
        }
    }
}
