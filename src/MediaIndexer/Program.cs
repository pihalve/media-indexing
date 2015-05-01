using Autofac;
using Pihalve.MediaIndexer.Bootstrapping;
using Topshelf;
using Topshelf.Autofac;
using Topshelf.HostConfigurators;

namespace Pihalve.MediaIndexer
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            var bootstrapper = new BootStrapper();
            var container = bootstrapper.Boot();

            HostFactory.Run(config => InitService(config, container));
        }

        private static void InitService(HostConfigurator config, ILifetimeScope container)
        {
            config.UseAutofacContainer(container);

            config.Service<MediaIndexingService>(sc =>
            {
                sc.ConstructUsingAutofacContainer();
                sc.WhenStarted(s => s.Start());
                sc.WhenStopped(s => s.Stop());
            });

            config.RunAsLocalSystem();
            config.StartAutomatically();

            config.SetDescription("Media indexing service for indexing images and videos");
            config.SetDisplayName("Media Indexing Service");
            config.SetServiceName("MediaIndexingService");

            config.UseLog4Net();
        }
    }
}
