using ServiceKeeper.Core;
using ServiceKeeper.Core.DependencyInjection;
using ServiceKeeper.UI;
using ServiceKeeper.UI.DependencyInjection;
using StackExchange.Redis;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace ServiceKeeper.Producer.Sample.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string url = "https://192.168.23.4:17777/";
            builder.WebHost.UseUrls(url);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<ServiceKeeperOptions>(options =>
            {
                options.MQHostName = "vyzh2019";
                options.MQExchangeName = "ServiceKeeper";
                options.MQUserName = "admin";
                options.MQPassword = "password";
                options.ServiceDescription = "服务调度中心";
            });
            builder.Services.Configure<ServiceKeeperUIOptions>(options =>
            {
                options.IsTakeOverTaskScheduling = true;
                options.CreateJwtSecretKey("CreateJwtSecretKey");
                options.AllowedCredentials.TryAdd("username", "password");
                options.ServiceKeeperUrl = url;
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("192.168.23.4:6379,password=password"));

            //serviceKeeper注入
            builder.Services.AddProducerServiceKeeper(null);
            builder.Services.AddServiceKeeperUI();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
            });

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseProducerServiceKeeper();
            app.UseServiceKeeperUI();

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}