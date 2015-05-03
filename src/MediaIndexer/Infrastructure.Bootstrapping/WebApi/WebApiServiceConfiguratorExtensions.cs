using System;
using Topshelf.ServiceConfigurators;

namespace Pihalve.MediaIndexer.Infrastructure.Bootstrapping.WebApi
{
    public static class WebApiServiceConfiguratorExtensions
    {
        public static ServiceConfigurator<T> WebApiEndpoint<T>(this ServiceConfigurator<T> configurator, Action<WebApiConfigurator> webConfigurator) where T : class
        {
            WebApiConfigurator config = new WebApiConfigurator();
            webConfigurator(config);
            config.Build();
            configurator.BeforeStartingService(t => config.Server.OpenAsync().Wait());
            configurator.BeforeStoppingService(t => config.Server.CloseAsync().Wait());
            return configurator;
        }
    }
}
