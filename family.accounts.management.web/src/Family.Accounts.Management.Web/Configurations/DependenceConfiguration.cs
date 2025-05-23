using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Family.Accounts.Management.Infrastructure.Refits;
using Family.Accounts.Management.Infrastructure.Repositories;
using Family.Accounts.Management.Infrastructure.Settings;
using Family.Accounts.Management.Web.Handlers;
using Family.Accounts.Management.Web.Helpers;
using Microsoft.AspNetCore.Authentication;
using Refit;

namespace Family.Accounts.Management.Web.Configurations
{
    public static class DependenceConfiguration
    {
        public static void AddDependence(this WebApplicationBuilder builder, AccountsManagementSettings settings)
        {
            builder.Services.AddScoped<AuthHeaderHandler>();
            builder.Services.AddScoped<IClaimsTransformation, ClaimsTranformer>();
            builder.Services.AddScoped<IAppRepository, AppRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ILoginRepository, LoginRepository>();
            builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
            builder.Services.AddScoped<IPermissionRepository,PermissionRepository>();
            builder.Services.AddScoped<IClientRepository, ClientRepository>();

            builder.Services.AddRefitClient<IAppRefit>().ConfigureHttpClient(c =>
            {
                c.BaseAddress = new  Uri(settings.FamilyAccountsApiUrl);
            })
            .AddHttpMessageHandler<AuthHeaderHandler>();

            builder.Services.AddRefitClient<IProfileRefit>().ConfigureHttpClient(c =>
            {
                c.BaseAddress = new  Uri(settings.FamilyAccountsApiUrl);
            })
            .AddHttpMessageHandler<AuthHeaderHandler>();

            builder.Services.AddRefitClient<IPermissionRefit>().ConfigureHttpClient(c =>
            {
                c.BaseAddress = new  Uri(settings.FamilyAccountsApiUrl);
            })
            .AddHttpMessageHandler<AuthHeaderHandler>();

            builder.Services.AddRefitClient<IUserRefit>().ConfigureHttpClient(c => {
                c.BaseAddress = new Uri(settings.FamilyAccountsApiUrl);
            })
            .AddHttpMessageHandler<AuthHeaderHandler>();


            builder.Services.AddRefitClient<IClientRefit>().ConfigureHttpClient(c => {
                c.BaseAddress = new Uri(settings.FamilyAccountsApiUrl);
            })
            .AddHttpMessageHandler<AuthHeaderHandler>();
            
            builder.Services.AddRefitClient<IClientAuthorizationRefit>().ConfigureHttpClient(c =>
            {
                c.BaseAddress = new  Uri(settings.FamilyAccountsApiUrl);
            });

            builder.Services.AddRefitClient<ILoginRefit>().ConfigureHttpClient(c =>
            {
                c.BaseAddress = new  Uri(settings.FamilyAcountsLoginUrl);
            });
        }
    }
}