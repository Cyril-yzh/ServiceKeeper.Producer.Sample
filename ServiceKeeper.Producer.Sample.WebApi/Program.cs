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
                options.MQExchangeName = "echangeEventBusDemo1";
                options.MQUserName = "admin";
                options.MQPassword = "Aa111111";
                options.ServiceDescription = "服务调度中心";
            });
            builder.Services.Configure<ServiceKeeperUIOptions>(options =>
            {
                options.IsTakeOverTaskScheduling = true;
                options.CreateJwtSecretKey("CreateJwtSecretKey");
                options.AllowedCredentials.TryAdd("username", "password");
                options.AllowedCredentials.TryAdd("yuzehao", "Aa111111");
                options.ServiceKeeperUrl = url;
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("192.168.23.4:6379,password=Sivic2812"));

            //serviceKeeper注入
            builder.Services.AddProducerServiceKeeper(null);
            builder.Services.AddServiceKeeperUI();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                    //builder.WithOrigins(/*url,*/"https://192.168.23.4:17777/", "http://127.0.0.1:5500", "http://localhost:5500", "http://192.168.23.4:5500", "http://127.0.0.1:5501", "http://localhost:5501", "http://192.168.23.4:5501", "null").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
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

            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}