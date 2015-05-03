using System.Collections.Generic;
using System.IO;
using Raven.Abstractions.Data;
using Raven.Client.Embedded;

namespace Pihalve.MediaIndexer.Infrastructure.Raven
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

                if (!store.DatabaseExists(MediaIndexerDatabaseName))
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
    }
}
