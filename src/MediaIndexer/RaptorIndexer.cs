using Pihalve.MediaIndexer.Entities;
using RaptorDB.Common;

namespace Pihalve.MediaIndexer
{
    public class RaptorIndexer : IIndexer
    {
        private readonly IRaptorDB _raptorDb;

        public RaptorIndexer(IRaptorDB raptorDb)
        {
            _raptorDb = raptorDb;
        }

        public void Save(MediaItem item)
        {
            _raptorDb.Save(item.Id, item);
        }
    }
}
