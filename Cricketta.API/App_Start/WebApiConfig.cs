using Cricketta.Data.Base;
using Cricketta.Data.Data;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
            config.Routes.MapHttpRoute("AdvancePathMapping2", "api/{controller}/{id}/{action}/{value}", defaults: new { id = RouteParameter.Optional, value= RouteParameter.Optional });

            // config.Routes.MapHttpRoute("DefaultApiWithAction", "Api/{controller}/{action}/{value}");
            var container = new UnityContainer();
            SetDependency(container);
            config.DependencyResolver = new UnityResolver(container);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new IsoDateTimeConverter());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = serializerSettings;

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
            container.RegisterType<ILeagueMatchRepository, LeagueMatchRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IMatchRepository, MatchRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITeamRepository, TeamRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IPlayerRepository, PlayerRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITeamPlayerRepository, TeamPlayerRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ILeagueScoreCardRepository, LeagueScoreCardRepository>(new HierarchicalLifetimeManager());
        }
    }
}


