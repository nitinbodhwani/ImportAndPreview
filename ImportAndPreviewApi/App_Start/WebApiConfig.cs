using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ImportAndPreviewApi
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			string requestOriginUrl = WebConfigurationManager.AppSettings["requestOriginUrl"];
			var corsAttr = new EnableCorsAttribute(requestOriginUrl, "*", "*");
			config.EnableCors(corsAttr);
			//config.EnableCors();
		}
	}
}
