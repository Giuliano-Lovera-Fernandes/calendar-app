using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.RVSPs;
using Dima.Core.Responses;
using MudBlazor;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace Dima.Web.Handlers
{
    public class RVSPHandler : IRVSPHandler    
    {
        private readonly HttpClient _client;
        private readonly ISnackbar _snackBar; 

        public RVSPHandler(IHttpClientFactory httpClientFactory, ISnackbar snackBar)
        {
            _client = httpClientFactory.CreateClient(Configuration.HttpClientName); 
            _snackBar = snackBar; 
        }
        public async Task<Response<RVSP?>> CreateAsync(CreateRVSPRequest request)
        {
            var result = await _client.PostAsJsonAsync("v1/rvsps", request);
            var response = await result.Content.ReadFromJsonAsync<Response<RVSP?>>();                    

            return response;
            
        }
        public async Task<Response<RVSP?>> DeleteAsync(DeleteRVSPRequest request)
        {
            var result = await _client.DeleteAsync($"v1/rvsps/{request.Id}");
            return await result.Content.ReadFromJsonAsync<Response<RVSP?>>()
                ?? new Response<RVSP?>(null, 400, "Falha ao excluir a resposta ao evento");
        }

        public async Task<PagedResponse<List<RVSP>>> GetAllAsync(GetAllRVSPsRequest request)
        {
            return await _client.GetFromJsonAsync<PagedResponse<List<RVSP>>>("v1/rvsps")
                ?? new PagedResponse<List<RVSP>>(null, 400, "Não foi possível obter as respostas aos eventos");
        }

        public async Task<Response<RVSP?>> GetByIdAsync(GetRVSPByIdRequest request)
        {
            var result = await _client.GetFromJsonAsync<Response<RVSP?>>($"v1/rvsps/{request.Id}");
            return result
                ?? new Response<RVSP?>(null, 400, "Não foi possível obter a resposta ao evento");
        }

        public async Task<Response<RVSP?>> UpdateAsync(UpdateRVSPRequest request)
        {
            var result = await _client.PutAsJsonAsync($"v1/rvsps/{request.Id}", request);           
            var response = await result.Content.ReadFromJsonAsync<Response<RVSP?>>();           

            return response;
        }
    }
}
