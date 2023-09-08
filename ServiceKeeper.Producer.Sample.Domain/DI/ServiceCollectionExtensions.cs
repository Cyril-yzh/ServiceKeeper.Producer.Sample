using Microsoft.Extensions.DependencyInjection;
using ServiceKeeper;
using ServiceKeeper.Producer.Sample.Domain.EFCore;
using Microsoft.Extensions.Options;
using ServiceKeeper.Core;

namespace ServiceKeeper.Producer.Sample.Domain.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var scheduler = sp.GetRequiredService<ServiceScheduler>();
                var options = sp.GetRequiredService<IOptions<LocalSourceOptions>>().Value;
                var saver = new SaveHelper();
                return new TaskEntityDomainService(scheduler, options, saver);
            });
            //基础服务注入
            services.AddSingleton<TaskDbContext>();

            return services;
        }
    }
}
