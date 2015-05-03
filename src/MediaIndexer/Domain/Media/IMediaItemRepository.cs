using System.Collections.Generic;

namespace Pihalve.MediaIndexer.Domain.Media
{
    public interface IMediaItemRepository
    {
        IEnumerable<MediaItem> QueryByTags(string[] tags);
    }
}
