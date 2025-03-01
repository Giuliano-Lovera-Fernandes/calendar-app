using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Events;
using Dima.Core.Responses;
using MudBlazor;
using System.Net.Http.Json;

namespace Dima.Web.Handlers
{
    public class EventoHandler : IEventHandler
    //public class EventoHandler(IHttpClientFactory httpClientFactory) : IEventHandler
    {
        //private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);

        private readonly HttpClient _client;
        private readonly ISnackbar _snackBar; // Para mostrar as mensagens

        // Construtor onde o HttpClient e o SnackBar são injetados
        public EventoHandler(IHttpClientFactory httpClientFactory, ISnackbar snackBar)
        {
            _client = httpClientFactory.CreateClient(Configuration.HttpClientName); // Inicializando HttpClient
            _snackBar = snackBar; // Inicializando ISnackbar
        }
        public async Task<Response<Event?>> CreateAsync(CreateEventRequest request)
        {
            var result = await _client.PostAsJsonAsync("v1/events", request);

            var response = await result.Content.ReadFromJsonAsync<Response<Event?>>();
                //?? new Response<Event?>(null, 400, "Falha ao criar o evento");

            // Verifica se há erros detalhados
            // Verifica se há erros detalhados
            //if (response.Message.Any())
            //{
            //    // Exibe os erros individuais no SnackBar
                
            //    _snackBar.Add(response.Message, Severity.Error);
            //}            

            return response;
        }

        public async Task<Response<Event?>> GetByIdAsync(GetEventByIdRequest request)
        {

            var result = await _client.GetFromJsonAsync<Response<Event?>>($"v1/events/{request.Id}");
            return result
                ?? new Response<Event?>(null, 400, "Não foi possível obter a evento");
        }

        public async Task<Response<Event?>> UpdateAsync(UpdateEventRequest request)
        {
            var result = await _client.PutAsJsonAsync($"v1/events/{request.Id}", request);           
            var response = await result.Content.ReadFromJsonAsync<Response<Event?>>();            

            return response;
        }

        public async Task<PagedResponse<List<Event>>> GetAllAsync(GetAllEventsRequest request)
        {
            return await _client.GetFromJsonAsync<PagedResponse<List<Event>>>("v1/events")
                ?? new PagedResponse<List<Event>>(null, 400, "Não foi possível obter os eventos");
        }

        public async Task<Response<Event?>> DeleteAsync(DeleteEventRequest request)
        {
            var result = await _client.DeleteAsync($"v1/events/{request.Id}");
            return await result.Content.ReadFromJsonAsync<Response<Event?>>()
                ?? new Response<Event?>(null, 400, "Falha ao excluir o evento");
        }        
    }    
}
