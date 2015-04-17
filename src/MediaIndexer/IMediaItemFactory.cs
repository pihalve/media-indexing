using Pihalve.MediaIndexer.Entities;

namespace Pihalve.MediaIndexer
{
    public interface IMediaItemFactory
    {
        MediaItem Create(string filePath);
    }
}
