using Microsoft.EntityFrameworkCore;
using Service02.Features.Command.CreateEvent;
using Service02.Models;
using Service02.Models.Models;
using Service02.Services;
using Service02.Services.ConnectionService;
using Service02.Services.EventQueueService;
using Service02.Services.IpService;
using Service02.Services.UserService;
using System.Reflection;

namespace Service02
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies([
                    Assembly.GetExecutingAssembly(),
                    typeof(CreateEventHandler).Assembly,
                    ]));

            IConfiguration configuration = builder.Configuration;
            string connectionString = configuration.GetValue<string>("Service02:ConnectionString");

            builder.Services.AddDbContextPool<PostgresContext>(options => options.UseNpgsql(connectionString));
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IIpService, IpService>();
            builder.Services.AddScoped<IConnectionService, ConnectionService>();
            builder.Services.AddScoped<IEventQueueService, EventQueueService>();
            builder.Services.AddSingleton<EventQueue>(); 
            builder.Services.AddSingleton<MessageProcessor>(); 
            builder.Services.Configure<ConnectOption>(builder.Configuration.GetSection("Service02"));

            var app = builder.Build();

            app.UseAuthorization();

            app.MapControllers();

            var processor = app.Services.GetRequiredService<MessageProcessor>();
            processor.StartProcessing();
            app.Lifetime.ApplicationStopping.Register(processor.StopProcessing);

            app.Run();
        }
    }
}
