using System.Web.Http;
using Newtonsoft.Json;

namespace ContosoUniversity
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
        }
    }
}
