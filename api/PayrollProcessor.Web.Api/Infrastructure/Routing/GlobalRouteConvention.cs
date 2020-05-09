using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

/// <summary>
/// https://stackoverflow.com/a/58406404/939634
/// </summary>
namespace PayrollProcessor.Web.Api.Infrastructure.Routing
{
    public class GlobalRouteConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel routePrefix;

        public GlobalRouteConvention(IRouteTemplateProvider route)
        {
            if (route is null)
            {
                throw new ArgumentNullException(nameof(route));
            }

            routePrefix = new AttributeRouteModel(route);
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var selector in application.Controllers.SelectMany(c => c.Selectors))
            {
                if (selector.AttributeRouteModel != null)
                {
                    selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(routePrefix, selector.AttributeRouteModel);
                }
                else
                {
                    selector.AttributeRouteModel = routePrefix;
                }
            }
        }
    }

    public static class MvcOptionsRouteExtensions
    {
        public static void UseGlobalRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            if (routeAttribute is null)
            {
                throw new ArgumentNullException(nameof(routeAttribute));
            }

            opts.Conventions.Add(new GlobalRouteConvention(routeAttribute));
        }

        public static void UseGlobalRoutePrefix(this MvcOptions opts, string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException($"{nameof(prefix)} cannot be empty", nameof(prefix));
            }

            opts.UseGlobalRoutePrefix(new RouteAttribute(prefix));
        }
    }
}
