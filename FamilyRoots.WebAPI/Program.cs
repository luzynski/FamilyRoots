using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace FamilyRoots.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning) 
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            using var host = CreateHost(args);
            await host.RunAsync();
        }

        private static IHost CreateHost(string[] args)
        {
            var builder = Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .UseStartup<Startup>()
                    .UseSerilog()
                    .UseKestrel(options =>
                    {
                        options.Limits.MinResponseDataRate = null;
                        options.Limits.MaxResponseBufferSize = 4 * 1024 * 1024;
                        options.Limits.MaxRequestBodySize = null;
                        options.AllowSynchronousIO = true;
                    }));

            return builder.Build();
        }
    }
}
