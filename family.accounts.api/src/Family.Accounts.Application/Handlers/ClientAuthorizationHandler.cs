using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Family.Accounts.Application.Providers;
using Family.Accounts.Core.Enums;
using Family.Accounts.Core.Exceptions;
using Family.Accounts.Core.Handlers;
using Family.Accounts.Core.Requests;
using Family.Accounts.Core.Responses;
using Family.Accounts.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Family.Accounts.Application.Handlers
{
    public class ClientAuthorizationHandler : IClientAuthorizationHandler
    {
        private const string MSG_CLIENT_INVALID = "Client or secret invalid";
        private const string MSG_CLIENT_INACTIVE = "Client is inactive";
        private const string MSG_CLIENT_NOT_HAVE_PROFILE = "Client not have profile for the appId";

        private readonly AccountsContext _context;
        private readonly IPasswordProvider _passwordProvider;
        private readonly ITokenHandler _tokenHandler;

        public ClientAuthorizationHandler(
            AccountsContext context,
            IPasswordProvider passwordProvider,
            ITokenHandler tokenHandler)
        {   
            _context = context;
            _passwordProvider = passwordProvider;
            _tokenHandler = tokenHandler;
        }

        public async Task<AuthenticationResponse> AuthenticationAsync(ClientAuthenticationRequest request)
        {
            var client = await _context.Clients.AsNoTracking()
                .Include(i => i.ClientProfiles.Where(w => w.Status == StatusEnum.Active && w.IsDeleted == false))
                .ThenInclude(i => i.Profile)
                .ThenInclude(i => i.ProfilePermissions.Where(w => w.Status == StatusEnum.Active && w.IsDeleted == false))
                .ThenInclude(i => i.Permission)
                .Include(i => i.ClientProfiles.Where(w => w.Status == StatusEnum.Active && w.IsDeleted == false))
                .ThenInclude(i => i.Profile)
                .ThenInclude(i => i.App)
                .FirstOrDefaultAsync(w => w.Id == request.ClientId);

            if(client == null)
                throw new BusinessException(MSG_CLIENT_INVALID);

            var passwordHash = _passwordProvider.HashPassword(request.ClientSecret);

            if(client.Password != passwordHash)
                throw new BusinessException(MSG_CLIENT_INVALID);

            if(client.ClientProfiles == null || client.ClientProfiles.Count == 0)
                throw new BusinessException(MSG_CLIENT_INVALID);

            if(client.Status == StatusEnum.Inactive)
                throw new BusinessException(MSG_CLIENT_INACTIVE);

            if(
                client.ClientProfiles == null || 
                client.ClientProfiles.Any(w => w.Profile.App.Slug == request.AppSlug && w.Profile.Status == StatusEnum.Active) == false)
                throw new BusinessException(MSG_CLIENT_NOT_HAVE_PROFILE);


            client.ClientProfiles = client.ClientProfiles.Where(w => w.Profile.App.Slug == request.AppSlug).ToList();

            return _tokenHandler.GenerateToken(client);
        }
    }
}