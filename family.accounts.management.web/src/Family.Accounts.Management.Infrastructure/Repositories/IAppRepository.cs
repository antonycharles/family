using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Family.Accounts.Management.Infrastructure.Requests;
using Family.Accounts.Management.Infrastructure.Responses;

namespace Family.Accounts.Management.Infrastructure.Repositories
{
    public interface IAppRepository
    {
        Task<PaginatedResponse<AppResponse>> GetAsync(PaginatedRequest? request);
    }
}