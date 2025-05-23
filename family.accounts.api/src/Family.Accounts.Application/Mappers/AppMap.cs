using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Family.Accounts.Core.Entities;
using Family.Accounts.Core.Enums;
using Family.Accounts.Core.Requests;
using Family.Accounts.Core.Responses;

namespace Family.Accounts.Application.Mappers
{
    public static class AppMap
    {
        public static App ToApp(this AppRequest request) => new App
        {
            Name = request.Name.Trim(),
            Code = request.Code.Value,
            Slug = request.Slug.Trim(),
            CallbackUrl = request.CallbackUrl?.Trim(),
            Status = request.Status ?? StatusEnum.Active,
        };

        public static AppResponse ToAppResponse(this App app) => new AppResponse{
            Id = app.Id,
            Code = app.Code,
            Name = app.Name,
            Slug = app.Slug,
            CallbackUrl = app.CallbackUrl,
            Status = app.Status
        };

        public static void Update(this App app, AppRequest request)
        {
            app.Code = request.Code.Value;
            app.Name = request.Name.Trim();
            app.Slug = request.Slug.Trim();
            app.Status = request.Status.Value;
            app.CallbackUrl = request.CallbackUrl?.Trim();
            app.UpdatedAt = DateTime.UtcNow;
        }
    }
}