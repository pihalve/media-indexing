using Topshelf;
using Topshelf.HostConfigurators;

namespace Pihalve.MediaIndexer
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            HostFactory.Run(InitService);
        }

        private static void InitService(HostConfigurator config)
        {
            config.Service<MediaIndexingService>(sc =>
            {
                sc.ConstructUsing(() => new MediaIndexingService());
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
