namespace Pihalve.MediaIndexer.Domain.Media
{
    public interface IMediaItemFactory
    {
        MediaItem Create(string filePath);
    }
}
