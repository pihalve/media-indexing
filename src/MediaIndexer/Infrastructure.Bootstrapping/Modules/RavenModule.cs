using Autofac;
using Pihalve.MediaIndexer.Infrastructure.Raven;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Database.Server;

namespace Pihalve.MediaIndexer.Infrastructure.Bootstrapping.Modules
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

            new RavenDocumentsByEntityName().Execute(store); // ensure this raven studio index is present
            IndexCreation.CreateIndexes(GetType().Assembly, store);
        }
    }
}
