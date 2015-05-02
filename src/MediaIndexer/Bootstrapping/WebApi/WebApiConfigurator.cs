using System;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.SelfHost;
using log4net;

namespace Pihalve.MediaIndexer.Bootstrapping.WebApi
{
    public class WebApiConfigurator
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpSelfHostServer Server { get; private set; }

        public IDependencyResolver DependencyResolver { get; set; }

        public string Scheme { get; set; }

        public string Domain { get; set; }

        public int Port { get; set; }

        public Action<HttpRouteCollection> RouteConfigurer { get; set; }

        public Action<HttpSelfHostConfiguration> ServerConfigurer { get; set; }

        public WebApiConfigurator()
        {
            Scheme = "http";
            Domain = "localhost";
            Port = 8080;
        }

        public WebApiConfigurator UseDependencyResolver(IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;
            return this;
        }

        public WebApiConfigurator ConfigureRoutes(Action<HttpRouteCollection> route)
        {
            RouteConfigurer = route;
            return this;
        }

        public WebApiConfigurator ConfigureServer(Action<HttpSelfHostConfiguration> config)
        {
            ServerConfigurer = config;
            return this;
        }

        public WebApiConfigurator OnLocalhost(int port = 8080)
        {
            return OnHost("http", "localhost", port);
        }

        public WebApiConfigurator OnHost(string scheme = null, string domain = null, int port = 8080)
        {
            Scheme = !string.IsNullOrEmpty(scheme) ? scheme : Scheme;
            Domain = !string.IsNullOrEmpty(domain) ? domain : Domain;
            Port = port;
            return this;
        }

        public HttpSelfHostServer Build()
        {
            //LogWriter logWriter = HostLogger.Get(typeof(WebApiConfigurator));
            Uri uri = new UriBuilder(Scheme, Domain, Port).Uri;
            Log.Debug(string.Format("Configuring WebAPI Selfhost for URI: {0}", uri));
            HttpSelfHostConfiguration configuration = new HttpSelfHostConfiguration(uri);
            if (DependencyResolver != null)
            {
                configuration.DependencyResolver = DependencyResolver;
            }
            if (ServerConfigurer != null)
            {
                ServerConfigurer(configuration);
            }
            if (RouteConfigurer != null)
            {
                RouteConfigurer(configuration.Routes);
            }
            else
            {
                configuration.MapHttpAttributeRoutes();
            }
            Server = new HttpSelfHostServer(configuration);
            Log.Info(string.Format("WebAPI Selfhost server configured and listening on: {0}", uri));
            return Server;
        }
    }
}
