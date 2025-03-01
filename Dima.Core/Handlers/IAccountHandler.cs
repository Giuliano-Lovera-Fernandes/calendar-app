using Dima.Core.Models;
using Dima.Core.Models.Account;
using Dima.Core.Requests.Account;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Handlers
{
    public interface IAccountHandler
    {
        Task<Response<string>> LoginAsync(LoginRequest request);
        Task<Response<string>> RegisterAsync(RegisterRequest request);
        //Task<Response<List<User>>> GetAllUsersAsync(LoginRequest request);
        Task<PagedResponse<List<User>>> GetAllUsersAsync(GetAllUsersRequest request);
        Task LogoutAsync();
    }
}
