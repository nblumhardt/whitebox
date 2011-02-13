using System.Collections.Generic;
using System.Linq;
using Whitebox.Core.Application;

namespace Whitebox.Profiler.Display
{
    static class ComponentExtensions
    {
        public static IEnumerable<string> DescribeServices(this Component component)
        {
            return component.Services.Select(svc => component.DescribeService(svc));
        }

        static string DescribeService(this Component component, Service service)
        {
            if (service.IsTypedService && service.ServiceType == component.LimitType)
                return "self";
            return service.Description;
        }
    }
}
