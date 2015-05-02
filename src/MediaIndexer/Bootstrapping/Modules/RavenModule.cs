using Autofac;
using Pihalve.MediaIndexer.Raven.Configuration;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Database.Server;

namespace Pihalve.MediaIndexer.Bootstrapping.Modules
{
    public class RavenModule : BootModule
    {
        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterType<EmbeddableDocumentStore>().As<IDocumentStore>().SingleInstance();
        }

        public override void Configure(IContainer rootContainer)
        {
            var dataDirectory = Configuration.GetAppSetting<string>("DataDirectory");
            var databaseServerPort = Configuration.GetAppSetting<int>("DatabaseServerPort");

            NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(databaseServerPort);

            DatabaseManager.CreateDatabaseIfNotExists(dataDirectory);

            var store = (EmbeddableDocumentStore)rootContainer.Resolve<IDocumentStore>();
            DatabaseManager.InitializeDatabase(dataDirectory, databaseServerPort, store);

            //IndexCreation.CreateIndexes(GetType().Assembly, store);
        }
    }
}
