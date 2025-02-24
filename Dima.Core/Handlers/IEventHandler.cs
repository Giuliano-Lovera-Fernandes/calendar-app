using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Events;
using Dima.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Handlers
{
    public interface IEventHandler
    {
        Task<Response<Event?>> CreateAsync(CreateEventRequest request);
        Task<Response<Event?>> UpdateAsync(UpdateEventRequest request);
        Task<Response<Event ?>> DeleteAsync(DeleteEventRequest request);
        //Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request);
        Task<PagedResponse<List<Event>>> GetAllAsync(GetAllEventsRequest request);
    }
}
