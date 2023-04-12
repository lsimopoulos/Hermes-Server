using System.Configuration;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Serilog;

namespace Hermes
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateHostBuilder(args)
                .Build()
                .MigrateDatabase()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .ConfigureKestrel(options =>
                        {

                            options.ListenAnyIP(7001,
                                listenOptions =>
                                {
                                    listenOptions.UseHttps("Server.pfx", "GuwyTUzzDDh3UCaCmuLk");
                                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                                });

                            options.ListenAnyIP(55556, listenOptions =>
                            {
                                listenOptions.UseHttps("Server.pfx", "GuwyTUzzDDh3UCaCmuLk");
                                listenOptions.Protocols = HttpProtocols.Http2;
                            });

                            options.Limits.MinRequestBodyDataRate = null;
                            options.Limits.MaxRequestBodySize = null;
                            options.Limits.MaxRequestBufferSize = null;
                        });
                })
            .UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console());
    }
}
