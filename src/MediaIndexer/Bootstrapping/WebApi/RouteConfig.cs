using System.Web.Http;

namespace Pihalve.MediaIndexer.Bootstrapping.WebApi
{
    public static class RouteConfig
    {
        public static void Configure(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                "DefaultApiWithId",
                "api/{controller}"
                //"api/{controller}/{id}",
                //new { id = RouteParameter.Optional },
                //new { id = @"\d+" }
            );
        }
    }
}
