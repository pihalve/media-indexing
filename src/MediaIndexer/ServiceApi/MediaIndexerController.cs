using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Pihalve.MediaIndexer.ServiceApi
{
    public class MediaIndexerController : ApiController
    {
        private readonly IBulkIndexer _bulkIndexer;

        public MediaIndexerController(IBulkIndexer bulkIndexer)
        {
            _bulkIndexer = bulkIndexer;
        }

        [Route("mediaindexer")]
        public string Get()
        {
            return "Hello world!";
        }

        [Route("mediaindexer/reindexall")]
        [HttpGet]
        public HttpResponseMessage ReindexAll()
        {
            try
            {
                _bulkIndexer.ReindexAll();
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Succeeded");
        }
    }
}
