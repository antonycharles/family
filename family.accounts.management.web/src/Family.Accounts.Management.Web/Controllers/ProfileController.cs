using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Family.Accounts.Management.Infrastructure.Exceptions;
using Family.Accounts.Management.Infrastructure.Repositories;
using Family.Accounts.Management.Infrastructure.Requests;
using Family.Accounts.Management.Infrastructure.Responses;
using Family.Accounts.Management.Web.Helpers;
using Family.Accounts.Management.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Family.Accounts.Management.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IProfileRepository _profileRepository;
        private readonly IAppRepository _appRepository;

        public ProfileController(
            ILogger<ProfileController> logger,
            IProfileRepository profileRepository,
            IAppRepository appRepository)
        {
            _logger = logger;
            _profileRepository = profileRepository;
            _appRepository = appRepository;
        }

        [AuthorizeRole(RoleConstants.ProfileRole.List)]
        public async Task<IActionResult> IndexAsync(ProfilePaginatedRequest? request)
        {
            try
            {
                request.PageSize = 1000;
                var app = await _appRepository.GetByIdAsync(request.AppId.Value);  
                var profiles = await _profileRepository.GetAsync(request);
                return View(new AppProfileViewModel {
                    App = app,
                    Profiles = profiles 
                });

            }
            catch(ExternalApiException ex){
                ModelState.AddModelError("Error", ex.Message);
                return View();
            }
            catch(Exception ex){
                return View(new PaginatedResponse<ProfileResponse>(){
                    Request = new PaginatedRequest(),
                });
            }
        }

        [HttpGet]
        [AuthorizeRole(RoleConstants.ProfileRole.List)]
        public async Task<IActionResult> DetailsAsync(Guid id){
            try
            {
                var profile = await _profileRepository.GetByIdAsync(id);
                return View(profile);
            }
            catch(Exception ex)
            {
                HttpContext.AddMessageError(ex.Message);
                return RedirectToAction("Index");
            }
        }
    }
}