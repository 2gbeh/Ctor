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
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            return services;
        }

        // Inner class used as a convention to apply a global route prefix
        private class RoutePrefixConvention : IApplicationModelConvention
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
    }
}
