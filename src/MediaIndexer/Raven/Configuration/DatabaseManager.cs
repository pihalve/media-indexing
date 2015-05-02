using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pihalve.MediaIndexer.Entities;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Embedded;

namespace Pihalve.MediaIndexer.Raven.Configuration
{
    public static class DatabaseManager
    {
        private const string SystemDatabaseName = "System";
        private const string MediaIndexerDatabaseName = "MediaIndexer";

        public static void InitializeDatabase(string dataDirectory, int databaseServerPort, EmbeddableDocumentStore store)
        {
            store.DataDirectory = Path.Combine(dataDirectory, SystemDatabaseName);
            store.DefaultDatabase = MediaIndexerDatabaseName;
            store.UseEmbeddedHttpServer = true;
            store.Configuration.Port = databaseServerPort;
            store.Configuration.CreateAutoIndexesForAdHocQueriesIfNeeded = false;
            store.EnlistInDistributedTransactions = false;
            store.Initialize();
        }

        public static void CreateDatabaseIfNotExists(string dataDirectory)
        {
            using (var store = new EmbeddableDocumentStore())
            {
                store.UseEmbeddedHttpServer = true;
                store.DataDirectory = Path.Combine(dataDirectory, SystemDatabaseName);
                store.Initialize();

                var dbNames = store.DatabaseCommands.GlobalAdmin.GetDatabaseNames(10);
                if (!dbNames.Contains(MediaIndexerDatabaseName))
                {
                    store.DatabaseCommands.GlobalAdmin.CreateDatabase(new DatabaseDocument
                    {
                        Id = MediaIndexerDatabaseName,
                        Settings = new Dictionary<string, string>
                        {
                            {"Raven/DataDir", Path.Combine("~", MediaIndexerDatabaseName)}
                        }
                    });
                }
            }
        }

        public static void ClearCollection(IDocumentStore store, string collectionName)
        {
            using (var session = store.OpenSession())
            {
                session.Advanced.DocumentStore.DatabaseCommands.DeleteByIndex(
                    "Raven/DocumentsByEntityName",
                    new IndexQuery { Query = "Tag:" + collectionName },
                    new BulkOperationOptions { AllowStale = true });
            }
        }
    }
}
