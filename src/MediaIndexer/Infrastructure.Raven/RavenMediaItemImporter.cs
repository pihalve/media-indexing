using System;
using System.IO;
using System.Reflection;
using log4net;
using Pihalve.MediaIndexer.Application;
using Pihalve.MediaIndexer.Domain.Media;
using Pihalve.MediaIndexer.Infrastructure.FileSystem;
using Raven.Client;
using Raven.Client.Document;

namespace Pihalve.MediaIndexer.Infrastructure.Raven
{
    public class RavenMediaItemImporter : IMediaItemImporter
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IDocumentStore _store;
        private readonly IMediaItemFactory _mediaItemFactory;
        private readonly string _watchFolder;
        private readonly string[] _watchExtensions;

        public RavenMediaItemImporter(IDocumentStore store, IMediaItemFactory mediaItemFactory, string watchFolder, string watchFilter)
        {
            _store = store;
            _mediaItemFactory = mediaItemFactory;
            _watchFolder = watchFolder;
            _watchExtensions = watchFilter.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public void Import()
        {
            Log.Info("Import started...");

            _store.ClearCollection("MediaItems");

            int succeeded = 0;
            int failed = 0;
            using (BulkInsertOperation bulkInsert = _store.BulkInsert())
            {
                var files = DirectoryEx.EnumerateFiles(_watchFolder, _watchExtensions, SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    try
                    {
                        var mediaItem = _mediaItemFactory.Create(file);
                        if (mediaItem != null)
                        {
                            bulkInsert.Store(mediaItem);
                            succeeded++;
                            
                            if (Log.IsDebugEnabled) Log.DebugFormat("File indexed: {0}", file);
                        }
                    }
                    catch (Exception ex)
                    {
                        failed++;
                        Log.ErrorFormat("Failed indexing file: {0}. {1}", file, ex.Message);
                    }
                }
            }

            Log.InfoFormat("Import finished. Succeeded: {0}. Failed: {1}", succeeded, failed);
        }
    }
}
