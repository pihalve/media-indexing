using Autofac;
using Pihalve.MediaIndexer.Application;
using Pihalve.MediaIndexer.Domain.Media;
using Pihalve.MediaIndexer.Domain.Metadata;
using Pihalve.MediaIndexer.Infrastructure.Metadata;
using Pihalve.MediaIndexer.Infrastructure.Raven;
using Pihalve.MediaIndexer.Interfaces.Service;

namespace Pihalve.MediaIndexer.Infrastructure.Bootstrapping.Modules
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
            builder.RegisterType<RavenMediaItemRepository>().As<IMediaItemRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RavenMediaItemImporter>().As<IMediaItemImporter>()
                .WithParameters(new[]
                    {
                        new NamedParameter("watchFolder", watchFolder), 
                        new NamedParameter("watchFilter", watchFilter)
                    })
                .InstancePerLifetimeScope();
            //builder.RegisterType<FileSystemMonitor>().As<IFileSystemMonitor>()
            //    .WithParameters(new[]
            //        {
            //            new NamedParameter("watchFolder", watchFolder), 
            //            new NamedParameter("watchFilter", watchFilter)
            //        })
            //    .InstancePerLifetimeScope();
            builder.RegisterType<DummyFileSystemMonitor>().As<IFileSystemMonitor>().InstancePerLifetimeScope();
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
