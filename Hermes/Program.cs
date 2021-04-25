using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace Hermes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
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
                                    listenOptions.Protocols = HttpProtocols.Http1;
                                });
                         
                            options.ListenAnyIP(5001, listenOptions =>
                            {
                                listenOptions.UseHttps("Server.pfx", "GuwyTUzzDDh3UCaCmuLk");
                                listenOptions.Protocols = HttpProtocols.Http2;
                            });

                            options.Limits.MinRequestBodyDataRate = null;
                            options.Limits.MaxRequestBodySize = null;
                            options.Limits.MaxRequestBufferSize = null;
                        });
                });
    }
}
