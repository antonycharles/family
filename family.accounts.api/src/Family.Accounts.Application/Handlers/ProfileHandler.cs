using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Family.Accounts.Application.Mappers;
using Family.Accounts.Core.Entities;
using Family.Accounts.Core.Enums;
using Family.Accounts.Core.Exceptions;
using Family.Accounts.Core.Handlers;
using Family.Accounts.Core.Requests;
using Family.Accounts.Core.Responses;
using Family.Accounts.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Family.Accounts.Application.Handlers
{
    public class ProfileHandler : IProfileHandler
    {
        private readonly AccountsContext _context;

        public ProfileHandler(AccountsContext context){
            _context = context;
        }

        public async Task<ProfileResponse> CreateAsync(ProfileRequest request)
        {
            var profile = request.ToProfile();
            await ValidExists(profile);
            _context.Profiles.Add(profile);
            
            UpdatePermission(profile, request);
            await _context.SaveChangesAsync();

            return profile.ToProfileResponse();
        }

        public async Task DeleteAsync(Guid id)
        {
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(w => w.Id == id && w.Status == StatusEnum.Active);

            if(profile == null)
                throw new NotFoundException("Profile not found");

            profile.Status = StatusEnum.Inactive;

            _context.Update(profile);

            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResponse<ProfileResponse>> GetAsync(PaginatedRequest request)
        {
            var query = _context.Profiles.AsNoTracking()
                .Where(w => w.Status == StatusEnum.Active);

            if(request.Search is not null)
                query = query.Where(w => w.Name.ToLower() == request.Search.ToLower());

            var profiles = await query
            .OrderBy(o => o.Name)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
            
            var totalItems = await query.CountAsync();
            
            var response = profiles.Select(s => s.ToProfileResponse()).ToList();

            return new PaginatedResponse<ProfileResponse>(response, totalItems, request);
        }

        public async Task<ProfileResponse> GetByIdAsync(Guid id)
        {
            var profile = await _context.Profiles.AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id && w.Status == StatusEnum.Active);

            if(profile == null)
                throw new NotFoundException("Profile not found");

            return profile.ToProfileResponse();
        }

        public async Task UpdateAsync(Guid id, ProfileRequest request)
        {
            var profile = await _context.Profiles
                .Include(i => i.ProfilePermissions)
                .FirstOrDefaultAsync(w => w.Id == id && w.Status == StatusEnum.Active);

            if(profile == null)
                throw new NotFoundException("Profile not found");

            profile.Update(request);

            await ValidExists(profile);
            
            _context.Profiles.Update(profile);

            UpdatePermission(profile, request);
            
            await _context.SaveChangesAsync();
        }


        private async Task ValidExists(Profile profile){
            var exist = await _context.Profiles.AsNoTracking()
                .AnyAsync(w => w.Name == profile.Name && w.Status == StatusEnum.Active);

                if(exist)
                    throw new BusinessException("Profile name exists");
        }

        private void UpdatePermission(Profile profile, ProfileRequest request){

            if(profile.ProfilePermissions != null && profile.ProfilePermissions.Count > 0){
                foreach(var profielPermission in profile.ProfilePermissions)
                    profielPermission.Status = StatusEnum.Inactive;
            }

            if(request.PermissionIds == null || request.PermissionIds.Count() == 0)
                return; 

            foreach(var permissionId in request.PermissionIds){
                var exist = profile.ProfilePermissions?.FirstOrDefault(w => w.PermissionId == permissionId);

                if(exist != null)
                    exist.Status = StatusEnum.Active;
                else{
                    _context.ProfilePermissions.Add(new ProfilePermission{
                        ProfileId = profile.Id,
                        PermissionId = permissionId,
                    });
                }
            }
        }
    }
}