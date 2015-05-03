using System.Linq;
using Pihalve.MediaIndexer.Domain.Media;
using Raven.Client.Indexes;

namespace Pihalve.MediaIndexer.Infrastructure.Raven.Indexes
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
