using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Arekbor.TouchBase.Application.Common.Mappings;

public static class MapsterConfiguration
{
    public static void AddMapster(this IServiceCollection services, Assembly assembly)
    {
        TypeAdapterConfig.GlobalSettings.Scan(assembly);
    }
}