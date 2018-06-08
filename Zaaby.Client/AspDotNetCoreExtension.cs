﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Zaaby.Client
{
    public static class AspDotNetCoreExtension
    {
        public static IServiceCollection UseZaabyClient(this IServiceCollection serviceCollection,
            Dictionary<string, List<string>> baseUrls)
        {
            if (baseUrls == null) return serviceCollection;

            var interfaces =
                ServiceTypeRepository.GetZaabyApplicationServiceTypes(type => baseUrls.ContainsKey(type.Namespace));

            var client = new ZaabyClient(interfaces
                .Where(@interface => baseUrls.ContainsKey(@interface.Namespace))
                .Select(@interface => @interface.Namespace)
                .Distinct()
                .ToDictionary(k => k, v => baseUrls[v]));
            var dynamicType = client.GetType();
            var methodInfo = dynamicType.GetMethod("GetService");
            foreach (var interfaceType in interfaces)
            {
                var proxy = methodInfo.MakeGenericMethod(interfaceType).Invoke(client, null);
                serviceCollection.AddScoped(interfaceType, p => proxy);
            }

            return serviceCollection;
        }
    }
}