using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceKeeper.Producer.Sample.Domain.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseApplication(this IApplicationBuilder appBuilder)
        {
            _ = appBuilder.ApplicationServices.GetRequiredService<TaskEntityDomainService>();
            return appBuilder;
        }
    }
}
