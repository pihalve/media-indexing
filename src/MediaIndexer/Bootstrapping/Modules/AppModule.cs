using Autofac;
using Pihalve.MediaIndexer.MetaData;
using Pihalve.MediaIndexer.Raven;

namespace Pihalve.MediaIndexer.Bootstrapping.Modules
{
    public class AppModule : BootModule
    {
        protected override void Register(ContainerBuilder builder)
        {
            var watchFolder = Configuration.GetAppSetting<string>("WatchFolder");
            var watchFilter = Configuration.GetAppSetting<string>("WatchFilter");

            builder.RegisterType<ExifTagReader>().As<IExifTagReader>().InstancePerLifetimeScope();
            builder.RegisterType<IptcTagReader>().As<IIptcTagReader>().InstancePerLifetimeScope();
            builder.RegisterType<MediaItemFactory>().As<IMediaItemFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<FileSystemMonitor>().As<IFileSystemMonitor>()
            //    .WithParameters(new[]
            //        {
            //            new NamedParameter("watchFolder", watchFolder), 
            //            new NamedParameter("watchFilter", watchFilter)
            //        })
            //    .InstancePerLifetimeScope();
            builder.RegisterType<DummyFileSystemMonitor>().As<IFileSystemMonitor>().InstancePerLifetimeScope();
            builder.RegisterType<MediaItemBulkIndexer>().As<IBulkIndexer>()
                .WithParameters(new[]
                    {
                        new NamedParameter("watchFolder", watchFolder), 
                        new NamedParameter("watchFilter", watchFilter)
                    })
                .InstancePerLifetimeScope();
            builder.RegisterType<MediaIndexingService>().InstancePerLifetimeScope();
        }
    }

    public class DummyFileSystemMonitor : IFileSystemMonitor
    {
        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
