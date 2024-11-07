using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace GorevYonetimSistemi.Business.Mappings
{
    public static class AutoMapperServiceExtension
{
    public static void ConfigureAutoMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
    }
}
}