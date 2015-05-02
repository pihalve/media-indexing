using System;
using System.IO;
using log4net;
using Pihalve.MediaIndexer.Raven.Configuration;
using Raven.Client;
using Raven.Client.Document;

namespace Pihalve.MediaIndexer.Raven
{
    public class MediaItemBulkIndexer : IBulkIndexer
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IDocumentStore _store;
        private readonly IMediaItemFactory _mediaItemFactory;
        private readonly string _watchFolder;
        private readonly string[] _watchExtensions;

        public MediaItemBulkIndexer(IDocumentStore store, IMediaItemFactory mediaItemFactory, string watchFolder, string watchFilter)
        {
            _store = store;
            _mediaItemFactory = mediaItemFactory;
            _watchFolder = watchFolder;
            //_watchFolder = @"D:\Media";
            _watchExtensions = watchFilter.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public void ReindexAll()
        {
            DatabaseManager.ClearCollection(_store, "MediaItems");

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

                            if (Log.IsDebugEnabled) Log.DebugFormat("File indexed: {0}", file);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.ErrorFormat("Failed indexing file: {0}. {1}", file, ex.Message);
                    }
                }
            }
        }
    }
}
