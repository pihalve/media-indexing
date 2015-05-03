using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Pihalve.MediaIndexer.ServiceApi
{
    public class MediaIndexerController : ApiController
    {
        private readonly IMediaItemImporter _bulkIndexer;

        public MediaIndexerController(IMediaItemImporter bulkIndexer)
        {
            _bulkIndexer = bulkIndexer;
        }

        [Route("mediaindexer")]
        public string Get()
        {
            return "Hello world!";
        }

        [Route("mediaindexer/import")]
        [HttpGet]
        public HttpResponseMessage Import()
        {
            try
            {
                _bulkIndexer.Import();
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Succeeded");
        }
    }
}
