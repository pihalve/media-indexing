using System.Linq;
using Pihalve.MediaIndexer.Entities;
using Raven.Client.Indexes;

namespace Pihalve.MediaIndexer.Raven.Indexes
{
    public class MediaItems_ByKeywords : AbstractIndexCreationTask<MediaItem>
    {
        public MediaItems_ByKeywords()
        {
            Map = mediaItems => mediaItems
                .Select(mediaItem => new
                {
                    mediaItem.Keywords
                });
        }
    }
}
