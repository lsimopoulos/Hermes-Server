using Hermes.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Hermes
{
    public static class MigrateManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using var hermesContext = scope.ServiceProvider.GetRequiredService<HermesContext>();
                try
                {
                    hermesContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
            return host;
        }
    }
}
