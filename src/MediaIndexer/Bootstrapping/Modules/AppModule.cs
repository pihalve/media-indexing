using Autofac;
using Pihalve.MediaIndexer.MetaData;

namespace Pihalve.MediaIndexer.Bootstrapping.Modules
{
    public class AppModule : BootModule
    {
        protected override void Register(ContainerBuilder builder)
        {
            var watchFolder = Configuration.GetAppSetting<string>("WatchFolder");
            var watchFilter = Configuration.GetAppSetting<string>("WatchFilter");

            builder.RegisterType<RaptorMediaItemIndexService>().As<IMediaItemIndexService>().InstancePerLifetimeScope();
            builder.RegisterType<ExifTagReader>().As<IExifTagReader>().InstancePerLifetimeScope();
            builder.RegisterType<IptcTagReader>().As<IIptcTagReader>().InstancePerLifetimeScope();
            builder.RegisterType<MediaItemFactory>().As<IMediaItemFactory>().InstancePerLifetimeScope();
            builder.RegisterType<FileSystemMonitor>().As<IFileSystemMonitor>()
                .WithParameters(new[]
                    {
                        new NamedParameter("watchFolder", watchFolder), 
                        new NamedParameter("watchFilter", watchFilter)
                    })
                .InstancePerLifetimeScope();
        }
    }
}
