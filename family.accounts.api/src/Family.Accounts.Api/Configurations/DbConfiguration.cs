using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Family.Accounts.Api.Seeds;
using Family.Accounts.Core;
using Family.Accounts.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Family.Accounts.Api.Configurations
{
    public static class DbConfiguration
    {
        
        public static void AddDataBase(this WebApplicationBuilder builder, AccountsSettings settings)
        {
            builder.Services.AddDbContext<AccountsContext>(x => 
                x.UseNpgsql(settings.DatabaseConnection),ServiceLifetime.Singleton);
        }


        public static void AddMigration(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var db = services.GetRequiredService<AccountsContext>();
                    db.Database.Migrate();
                }
                catch (Exception exception)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(exception, "An error occurred migration the DB.");
                }
            }
        }

        public static void SeedData(this IHost host){
            using(var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<AccountsContext>();

                AppSeed.Seeder(context);
                PermissionSeed.Seeder(context);
                ProfileSeed.Seeder(context);
                UserProfileSeed.Seeder(context);
            }
        }
    }
}