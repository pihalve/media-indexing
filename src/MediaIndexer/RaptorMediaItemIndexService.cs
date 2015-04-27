using System;
using System.Collections.Generic;
using System.Linq;
using Pihalve.MediaIndexer.Entities;
using Pihalve.MediaIndexer.Views;
using RaptorDB.Common;

namespace Pihalve.MediaIndexer
{
    public class RaptorMediaItemIndexService : IMediaItemIndexService
    {
        private readonly IRaptorDB _raptorDb;

        public RaptorMediaItemIndexService(IRaptorDB raptorDb)
        {
            _raptorDb = raptorDb;
        }

        public void Save(MediaItem item)
        {
            _raptorDb.Save(item.Id, item);
        }

        public void Delete(Guid itemId)
        {
            _raptorDb.Delete(itemId);
        }

        public MediaItem Fetch(Guid itemId)
        {
            return (MediaItem)_raptorDb.Fetch(itemId);
        }

        public IEnumerable<MediaItemViewRowSchema> Query(string filePath)
        {
            var result = _raptorDb.Query<MediaItemViewRowSchema>(x => x != null && x.FilePath == filePath);
            return result != null ? result.Rows : Enumerable.Empty<MediaItemViewRowSchema>();
        }

        public MediaItem Find(string filePath)
        {
            var mediaItemView = Query(filePath).FirstOrDefault();
            return mediaItemView == null ? null : Fetch(mediaItemView.docid);
        }
    }
}
