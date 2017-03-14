using Cricketta.Data.Base;
using Cricketta.Data.Data;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Cricketta.API
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
            config.Routes.MapHttpRoute("AdvancePathMapping", "api/{controller}/{id}/{action}", defaults: new { id = RouteParameter.Optional });

            // config.Routes.MapHttpRoute("DefaultApiWithAction", "Api/{controller}/{action}/{value}");
            var container = new UnityContainer();
            SetDependency(container);
            config.DependencyResolver = new UnityResolver(container);

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.Filters.Add(new ExceptionHandlingAttribute());
        }

        private static void SetDependency(IUnityContainer container)
        {
            container.RegisterType<IUnitofWork, UnitofWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IDatabaseFactory, DatabaseFactory>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ILeagueRepository, LeagueRepository>(new HierarchicalLifetimeManager());
        }
    }
}
