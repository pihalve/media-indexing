using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using log4net;

namespace Pihalve.MediaIndexer.ServiceApi
{
    public class MediaIndexerController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMediaItemImporter _importer;
        private readonly IMediaItemRepository _repository;

        public MediaIndexerController(IMediaItemImporter importer, IMediaItemRepository repository)
        {
            _importer = importer;
            _repository = repository;
        }

        [Route("mediaindexer/query")]
        [HttpGet]
        public HttpResponseMessage QueryByTags([ModelBinder]string[] tag)
        {
            try
            {
                var results = _repository.QueryByTags(tag);

                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                Log.Error("QueryByTags failed", ex);
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
                Log.Error("Import failed", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
