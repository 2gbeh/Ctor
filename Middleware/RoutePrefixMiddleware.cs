using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Ctor.Middleware;

public class RoutePrefixMiddleware : IApplicationModelConvention
{
    private readonly AttributeRouteModel _routePrefix;

    public RoutePrefixMiddleware(string prefix)
    {
        _routePrefix = new AttributeRouteModel(new Microsoft.AspNetCore.Mvc.RouteAttribute(prefix));
    }

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            foreach (var selector in controller.Selectors)
            {
                if (selector.AttributeRouteModel != null)
                {
                    // Combine existing route with global prefix
                    selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(
                        _routePrefix,
                        selector.AttributeRouteModel
                    );
                }
                else
                {
                    // Add prefix if no route defined
                    selector.AttributeRouteModel = _routePrefix;
                }
            }
        }
    }
}
