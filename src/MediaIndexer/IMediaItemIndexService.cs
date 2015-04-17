using System;
using System.Collections.Generic;
using Pihalve.MediaIndexer.Entities;
using Pihalve.MediaIndexer.Views;

namespace Pihalve.MediaIndexer
{
    public interface IMediaItemIndexService
    {
        void Save(MediaItem item);
        void Delete(Guid itemId);
        MediaItem Fetch(Guid itemId);
        IEnumerable<MediaItemViewRowSchema> Query(string filePath);
        MediaItem Find(string filePath);
    }
}
