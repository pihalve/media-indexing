using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using log4net;
using Pihalve.MediaIndexer.Entities;
using Pihalve.MediaIndexer.Raven.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace Pihalve.MediaIndexer.ServiceApi
{
    public class MediaIndexerController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMediaItemImporter _importer;
        private readonly IDocumentStore _store;

        public MediaIndexerController(IMediaItemImporter importer, IDocumentStore store)
        {
            _importer = importer;
            _store = store;
        }

        [Route("mediaindexer/query")]
        [HttpGet]
        public HttpResponseMessage QueryByTags([ModelBinder]string[] tag)
        {
            try
            {
                using (var session = _store.OpenSession())
                {
                    RavenQueryStatistics stats;
                    var results = session
                        .Query<MediaItem, MediaItems_ByKeywords>()
                        .Statistics(out stats)
                        .Where(x => x.Keywords.ContainsAll(tag))
                        .ToArray();

                    if (Log.IsDebugEnabled)
                    {
                        Log.DebugFormat("QueryByTags '{0}' returned {1} results",
                            tag != null ? tag.Aggregate((i, j) => i + "," + j) : "",
                            stats.TotalResults);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, results);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("mediaindexer/import")]
        [HttpGet]
        public HttpResponseMessage Import()
        {
            try
            {
                _importer.Import();

                return Request.CreateResponse(HttpStatusCode.OK, "Succeeded");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
