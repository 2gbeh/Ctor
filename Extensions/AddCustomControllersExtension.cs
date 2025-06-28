using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Ctor.Extensions
{
    public static class AddCustomControllersExtension
    {
        public static IServiceCollection AddCustomControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Conventions.Insert(0, new RoutePrefixConvention("api")); // Global route prefix
                options.Conventions.Add(new ControllerNamingConvention());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            return services;
        }
    }

    // Apply a global route prefix
    public class RoutePrefixConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _routePrefix;

        public RoutePrefixConvention(string prefix)
        {
            _routePrefix = new AttributeRouteModel(new RouteAttribute(prefix));
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                foreach (var selector in controller.Selectors)
                {
                    if (selector.AttributeRouteModel != null)
                    {
                        selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel);
                    }
                    else
                    {
                        selector.AttributeRouteModel = _routePrefix;
                    }
                }
            }
        }
    }

    // Rewrite [controller] token to kebab-case:
    public class ControllerNamingConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                foreach (var selector in controller.Selectors)
                {
                    if (selector.AttributeRouteModel?.Template?.Contains("[controller]") == true)
                    {
                        var kebab = ToKebabCase(controller.ControllerName);
                        selector.AttributeRouteModel.Template = selector.AttributeRouteModel.Template.Replace("[controller]", kebab);
                    }
                }
            }
        }

        private string ToKebabCase(string value)
        {
            return string.Concat(
                value.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + char.ToLower(x) : char.ToLower(x).ToString())
            );
        }
    }
}
