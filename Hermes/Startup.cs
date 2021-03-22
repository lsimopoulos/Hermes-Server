using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Hermes.IdentityServer;
using Hermes.Services;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace Hermes
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("secureHermes", policy =>
                {
                    policy.RequireClaim("scope", "hermes");
                });
            });

            services.AddAuthentication
        (o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddIdentityServerAuthentication(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://localhost:7001/";
                options.RequireHttpsMetadata = true;
                options.ApiName = "hermes";
                options.ApiSecret = "superdupersecret";
                IdentityModelEventSource.ShowPII = true;



                options.JwtBackChannelHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        delegate { return true; }
                };
            });

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseInformationEvents = true;

            })
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddSigningCredential(Cert.Get("Server.pfx", "GuwyTUzzDDh3UCaCmuLk"))
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryPersistedGrants()
                .AddTestUsers(Config.GetUsers());

            services.AddGrpc(options =>
            {
                options.EnableDetailedErrors = true;

            });

            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ChatterService>().RequireAuthorization("secureHermes");

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
