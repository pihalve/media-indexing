using Pihalve.MediaIndexer.Entities;

namespace Pihalve.MediaIndexer
{
    public interface IIndexer
    {
        void Save(MediaItem item);
    }
}
