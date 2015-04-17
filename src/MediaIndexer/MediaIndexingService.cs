using Autofac;
using log4net;
using Pihalve.MediaIndexer.Bootstrapping;

namespace Pihalve.MediaIndexer
{
    public class MediaIndexingService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BootStrapper _bootstrapper;
        private ILifetimeScope _scope;

        public void Start()
        {
            Log.Info("MediaIndexingService start");

            _bootstrapper = new BootStrapper();
            var container = _bootstrapper.Boot();
            _scope = container.BeginLifetimeScope();
            _scope.Resolve<IFileSystemMonitor>().Start();
        }

        public void Stop()
        {
            if (_scope != null)
            {
                _scope.Dispose();
            }

            if (_bootstrapper != null)
            {
                _bootstrapper.Dispose();
            }

            Log.Info("MediaIndexingService stop");
        }
    }
}
