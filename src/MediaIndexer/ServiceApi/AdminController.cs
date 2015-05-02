using System.Web.Http;

namespace Pihalve.MediaIndexer.ServiceApi
{
    public class AdminController : ApiController
    {
        [Route("api/admin")]
        public string Get()
        {
            return "Hello world!";
        }
    }
}
