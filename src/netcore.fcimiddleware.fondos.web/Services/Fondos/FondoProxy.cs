using Microsoft.Extensions.Options;
using netcore.fcimiddleware.fondos.web.Models.Shared;
using netcore.fcimiddleware.fondos.web.Models.V1.Fondos;
using netcore.fcimiddleware.fondos.web.Services.Proxies;
using System.Text;
using System.Text.Json;

namespace netcore.fcimiddleware.fondos.web.Services.Fondos
{
    public class FondoProxy : IFondoProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;
        private readonly ILogger<FondoProxy> _logger;
        private string _entidad = "Fondo";
        private string _version = "v1";

        public FondoProxy(
            IOptions<ApiUrls> apiUrls,
            HttpClient httpClient, 
            ILogger<FondoProxy> logger)
        {
            _apiUrls = apiUrls.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> Create(Fondo request)
        {
            StringContent content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
                );

            return await _httpClient.PostAsync(_apiUrls.FondoApiUrl + $"api/{_version}/{_entidad}", content);
        }

        public async Task<HttpResponseMessage> Create(FondoCreateRequest request)
        {
            StringContent content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
                );

            return await _httpClient.PostAsync(_apiUrls.FondoApiUrl + $"api/{_version}/{_entidad}", content);
        }

        public async Task<HttpResponseMessage> Delete(Fondo request)
        {
            return await _httpClient.DeleteAsync(_apiUrls.FondoApiUrl + $"api/{_version}/{_entidad}/{request.Id}");
        }

        public async Task<HttpResponseMessage> GetById(Fondo request)
        {
            return await _httpClient.GetAsync(_apiUrls.FondoApiUrl + $"api/{_version}/{_entidad}/id?Id={request.Id}");
        }

        public async Task<HttpResponseMessage> Pagination(PaginationQueryRequest request)
        {
            return
                await _httpClient
                .GetAsync(
                    _apiUrls.FondoApiUrl + $"api/{_version}/{_entidad}/pagination?PageIndex={request.PageIndex}&PageSize={request.PageSize}&Search={request.Search}&Sort={request.Sort}");
        }

        public async Task<HttpResponseMessage> Update(Fondo request)
        {
            StringContent content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
                );

            return await _httpClient.PutAsync(_apiUrls.FondoApiUrl + $"api/{_version}/{_entidad}", content);
        }
    }
}
