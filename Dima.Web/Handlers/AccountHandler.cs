using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Models.Account;
using Dima.Core.Requests.Account;
using Dima.Core.Responses;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;

namespace Dima.Web.Handlers
{
    public class AccountHandler(IHttpClientFactory httpClientFactory) : IAccountHandler
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);        

        public async Task<PagedResponse<List<User>>> GetAllUsersAsync(GetAllUsersRequest request)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<PagedResponse<List<User>>>("v1/identity/users");

                return response ?? new PagedResponse<List<User>>(null, 400, "Não foi possível obter os usuários");
            }
            catch (HttpRequestException ex)
            {
                return new PagedResponse<List<User>>(null, 500, $"Erro na requisição: {ex.Message}");
            }
            catch (Exception ex)
            {
                return new PagedResponse<List<User>>(null, 500, $"Erro inesperado: {ex.Message}");
            }
        }        

        public async Task<Response<string>> LoginAsync(LoginRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("v1/identity/login?useCookies=true", request);
            return result.IsSuccessStatusCode
                ? new Response<string>("Login realizado com sucesso", 200, "")
                : new Response<string>(null , 400, "Não foi possível realizar login");
        }

        public async Task LogoutAsync()
        {
            var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
            await _httpClient.PostAsJsonAsync("v1/identity/logout", emptyContent);
        }

        public async Task<Response<string>> RegisterAsync(RegisterRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("v1/identity/register", request);
            return result.IsSuccessStatusCode
                ? new Response<string>("Cadastro realizado com sucesso!", 201, "Cadastro realizado com sucesso!")
                : new Response<string>(null, 400, "Não foi possível realizar seu cadastro");
        }        
    }
}
