using System.Web.Http;
using Autofac;
using log4net.Config;
using Pihalve.MediaIndexer.Bootstrapping;
using Pihalve.MediaIndexer.Bootstrapping.WebApi;
using Topshelf;
using Topshelf.Autofac;
using Topshelf.HostConfigurators;

namespace Pihalve.MediaIndexer
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            using (var bootstrapper = new BootStrapper())
            {
                var container = bootstrapper.Boot();

                HostFactory.Run(config => InitService(config, container));
            }
        }

        private static void InitService(HostConfigurator config, ILifetimeScope container)
        {
            config.UseAutofacContainer(container);

            config.Service<MediaIndexingService>(sc =>
            {
                sc.ConstructUsingAutofacContainer();
                sc.WhenStarted(s => s.Start());
                sc.WhenStopped(s => s.Stop());

                sc.WebApiEndpoint(api => api
                    .OnLocalhost() // defaults to port 8080
                    .ConfigureServer(c => c.MapHttpAttributeRoutes())
                    .UseDependencyResolver(new AutofacWebApiDependencyResolver(container)));
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
