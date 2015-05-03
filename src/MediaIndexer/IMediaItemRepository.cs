using System.Collections.Generic;
using Pihalve.MediaIndexer.Entities;

namespace Pihalve.MediaIndexer
{
    public interface IMediaItemRepository
    {
        IEnumerable<MediaItem> QueryByTags(string[] tags);
    }
}
