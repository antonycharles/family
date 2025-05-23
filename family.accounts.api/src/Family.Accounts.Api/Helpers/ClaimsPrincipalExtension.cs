using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Family.Accounts.Application.Extensions;

namespace Family.Accounts.Api.Helpers
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetUserType(this ClaimsPrincipal claimsPrincipal)
        {
            var typeClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Type);
            return typeClaim?.Value ?? string.Empty;
        }
        public static Guid GetId(this ClaimsPrincipal claimsPrincipal)
        {
            var idClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Id);
            return idClaim?.Value != null ? new Guid(idClaim.Value) : Guid.Empty;
        }
    }
}