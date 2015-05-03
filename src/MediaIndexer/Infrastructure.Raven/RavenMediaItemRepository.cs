using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Pihalve.MediaIndexer.Domain.Media;
using Pihalve.MediaIndexer.Infrastructure.Raven.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace Pihalve.MediaIndexer.Infrastructure.Raven
{
    public class RavenMediaItemRepository : IMediaItemRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IDocumentStore _store;

        public RavenMediaItemRepository(IDocumentStore store)
        {
            _store = store;
        }

        public IEnumerable<MediaItem> QueryByTags(string[] tags)
        {
            using (var session = _store.OpenSession())
            {
                RavenQueryStatistics stats;
                var results = session
                    .Query<MediaItem, MediaItems_ByKeywords>()
                    .Statistics(out stats)
                    .Where(x => x.Keywords.ContainsAll(tags))
                    .ToArray();

                if (Log.IsDebugEnabled)
                {
                    Log.DebugFormat("QueryByTags '{0}' returned {1} results",
                        tags != null ? tags.Aggregate((i, j) => i + "," + j) : "",
                        stats.TotalResults);
                }

                return results;
            }
        }
    }
}
