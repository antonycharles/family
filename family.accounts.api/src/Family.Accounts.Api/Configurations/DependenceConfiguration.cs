using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Family.Accounts.Api.Helpers;
using Family.Accounts.Application.Handlers;
using Family.Accounts.Application.Providers;
using Family.Accounts.Core.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Family.Accounts.Api.Configurations
{
    public static class DependenceConfiguration
    {
        public static void AddDependence(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IPasswordProvider,PasswordProvider>();


            builder.Services.AddScoped<IClaimsTransformation, ClaimsTranformer>();
            builder.Services.AddTransient<CustomJwtBearerHandler>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddScheme<JwtBearerOptions, CustomJwtBearerHandler>(JwtBearerDefaults.AuthenticationScheme, options => { });

            //GERA-COMMANDS-ADD-REPOSITORY
            builder.Services.AddTransient<IUserHandler,UserHandler>();
            builder.Services.AddTransient<IUserProfileHandler,UserProfileHandler>();
            builder.Services.AddTransient<IUserHandler,UserHandler>();
            builder.Services.AddTransient<IPermissionHandler,PermissionHandler>();
            builder.Services.AddTransient<IProfileHandler,ProfileHandler>();
            builder.Services.AddTransient<IAppHandler,AppHandler>();
            builder.Services.AddTransient<IClientHandler,ClientHandler>();
            builder.Services.AddTransient<IClientProfileHandler,ClientProfileHandler>();
            builder.Services.AddTransient<ITokenHandler,TokenHandler>();
            builder.Services.AddTransient<ITokenKeyHandler,TokenKeyHandler>();
            builder.Services.AddTransient<IClientAuthorizationHandler, ClientAuthorizationHandler>();
            builder.Services.AddTransient<IUserAuthorizationHandler, UserAuthorizationHandler>();
        }
    }
}