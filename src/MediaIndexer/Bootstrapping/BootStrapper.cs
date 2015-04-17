using Autofac;
using Autofac.Util;
using Pihalve.MediaIndexer.MetaData;
using RaptorDB;
using RaptorDB.Common;

namespace Pihalve.MediaIndexer.Bootstrapping
{
    public class BootStrapper : Disposable
    {
        private IContainer _rootContainer;
        private RaptorDBServer _raptorDbServer;
        private IRaptorDB _raptorDb;

        public IContainer Boot()
        {
            var builder = new ContainerBuilder();

            var indexFolder = Configuration.GetAppSetting<string>("IndexFolder");
            var indexServerPort = Configuration.GetAppSetting<int>("IndexServerPort");
            var watchFolder = Configuration.GetAppSetting<string>("WatchFolder");
            var watchFilter = Configuration.GetAppSetting<string>("WatchFilter");

            //RaptorDB.Global.RequirePrimaryView = false;
            //var raptorDb = RaptorDB.RaptorDB.Open(indexFolder);
            //raptorDb.RegisterView(new MediaItemView());
            //_raptorDb = raptorDb;

            _raptorDbServer = new RaptorDBServer(indexServerPort, indexFolder);

            _raptorDb = new RaptorDBClient("localhost", indexServerPort, "admin", "admin");

            builder.RegisterInstance(_raptorDb).As<IRaptorDB>().ExternallyOwned();
            builder.RegisterType<RaptorMediaItemIndexService>().As<IMediaItemIndexService>().InstancePerLifetimeScope();
            builder.RegisterType<ExifTagReader>().As<IExifTagReader>().InstancePerLifetimeScope();
            builder.RegisterType<IptcTagReader>().As<IIptcTagReader>().InstancePerLifetimeScope();
            builder.RegisterType<MediaItemFactory>().As<IMediaItemFactory>().InstancePerLifetimeScope();
            builder.RegisterType<FileSystemMonitor>().As<IFileSystemMonitor>()
                .WithParameters(new []
                    {
                        new NamedParameter("watchFolder", watchFolder), 
                        new NamedParameter("watchFilter", watchFilter)
                    })
                .InstancePerLifetimeScope();

            _rootContainer = builder.Build();

            return _rootContainer;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (_raptorDb != null)
            {
                _raptorDb.Shutdown();
            }

            if (_raptorDbServer != null)
            {
                _raptorDbServer.Shutdown();
            }

            if (_rootContainer != null)
            {
                _rootContainer.Dispose();
            }
        }
    }
}
