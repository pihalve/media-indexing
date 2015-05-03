using System.Linq;
using Raven.Abstractions.Data;
using Raven.Client;

namespace Pihalve.MediaIndexer.Infrastructure.Raven
{
    public static class DocumentStoreExtensions
    {
        public static void ClearCollection(this IDocumentStore store, string collectionName)
        {
            using (var session = store.OpenSession())
            {
                var operation = session.Advanced.DocumentStore.DatabaseCommands.DeleteByIndex(
                    "Raven/DocumentsByEntityName",
                    new IndexQuery { Query = "Tag:" + collectionName },
                    new BulkOperationOptions { AllowStale = true });

                operation.WaitForCompletion();
            }
        }

        public static bool DatabaseExists(this IDocumentStore store, string databaseName)
        {
            var dbNames = store.DatabaseCommands.GlobalAdmin.GetDatabaseNames(10); //yes, 10 is a magic number - we are forced by raven api to supply a number here
            return dbNames.Contains(databaseName);
        }
    }
}
