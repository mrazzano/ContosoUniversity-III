using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Optimization;
using ContosoUniversity.ActionFilters;

namespace ContosoUniversity
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            GlobalFilters.Filters.Add(new WhitespaceFilterAttribute());
        }
    }
}
