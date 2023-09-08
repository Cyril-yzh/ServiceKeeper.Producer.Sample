using ServiceKeeper.Core;
using ServiceKeeper.Core.DependencyInjection;
using ServiceKeeper.Producer.Sample.Domain;
using ServiceKeeper.Producer.Sample.Domain.DependencyInjection;
using ServiceKeeper.UI;
using ServiceKeeper.UI.DependencyInjection;
using StackExchange.Redis;

namespace ServiceKeeper.Producer.Sample.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string url = "http://192.168.23.4:17777/";
            builder.WebHost.UseUrls(url);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            Configuration(builder);
            RegistryService(builder, url);

            var app = builder.Build();
            InitializeService(app);
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        public static void Configuration(WebApplicationBuilder builder)
        {
            //后期转Docker环境变量+统一Configurator
            builder.Services.Configure<LocalSourceOptions>(options =>
            {
                options.RootDir = "Config";
                options.ServiceConfigSaveName = "ServiceConfig";
                options.TaskEntitiesSaveName = "TaskEntities";
            });
            builder.Services.Configure<ServiceOptions>(options =>
            {
                options.MQHostName = "vyzh2019";
                options.MQExchangeName = "echangeEventBusDemo1";
                options.MQUserName = "admin";
                options.MQPassword = "Aa111111";
                options.ServiceDescription = "服务A";
            });
            builder.Services.Configure<ServiceKeeperUIOptions>(options =>
            {
                options.CreateJwtSecretKey("CreateJwtSecretKey");
                options.AllowedCredentials.TryAdd("username", "password");
            });
        }
        public static void RegistryService(WebApplicationBuilder builder, string url)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("192.168.23.4:6379,password=Sivic2812"));

            //serviceKeeper注入
            builder.Services.AddProducerServiceKeeper(null);
            builder.Services.AddServiceKeeperUI();
            builder.Services.AddApplication();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(url, "http://127.0.0.1:5500", "http://localhost:5500", "http://192.168.23.4:5500", "null").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
            });
        }

        public static void InitializeService(WebApplication app)
        {
            app.UseCors();
            app.UseProducerServiceKeeper();
            app.UseApplication();
            app.UseServiceKeeperUI();

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}