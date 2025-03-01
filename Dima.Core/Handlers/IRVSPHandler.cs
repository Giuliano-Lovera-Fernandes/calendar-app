using Dima.Core.Models;
using Dima.Core.Requests.Events;
using Dima.Core.Requests.RVSPs;
using Dima.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Handlers
{
    public interface IRVSPHandler
    {
        Task<Response<RVSP?>> CreateAsync(CreateRVSPRequest request);
        Task<Response<RVSP?>> UpdateAsync(UpdateRVSPRequest request);
        Task<Response<RVSP?>> DeleteAsync(DeleteRVSPRequest request);
        Task<Response<RVSP?>> GetByIdAsync(GetRVSPByIdRequest request);
        Task<PagedResponse<List<RVSP>>> GetAllAsync(GetAllRVSPsRequest request);
    }
}
