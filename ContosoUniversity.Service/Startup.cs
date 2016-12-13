using System;
using System.Web.Http;
using System.Configuration;
using System.Data.Entity.Infrastructure.Interception;
using ContosoUniversity.Infrastructure.Database;
using Unity.WebApi;
using Owin;

namespace ContosoUniversity.Service
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            var isDatabaseLoggingEnabled = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableDatabaseLogging"])
                && Convert.ToBoolean(ConfigurationManager.AppSettings["EnableDatabaseLogging"]);

            if (isDatabaseLoggingEnabled)
                DbInterception.Add(new SchoolInterceptorLogging());

            // Configure Web API for self-host. 
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.DependencyResolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());
            appBuilder.UseWebApi(config);
        }
    }
}