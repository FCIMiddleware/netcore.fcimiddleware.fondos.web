using Microsoft.Extensions.Options;
using netcore.fcimiddleware.fondos.web.Models.Shared;
using netcore.fcimiddleware.fondos.web.Models.V1.Monedas;
using netcore.fcimiddleware.fondos.web.Services.Proxies;
using System.Text;
using System.Text.Json;

namespace netcore.fcimiddleware.fondos.web.Services.Monedas
{
    public class MonedaProxy : IMonedaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;
        private readonly ILogger<MonedaProxy> _logger;
        private string _entidad = "Pais";
        private string _version = "v1";

        public MonedaProxy(
            IOptions<ApiUrls> apiUrls, 
            HttpClient httpClient, 
            ILogger<MonedaProxy> logger)
        {
            _apiUrls = apiUrls.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> Create(Moneda request)
        {
            StringContent content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
                );

            return await _httpClient.PostAsync(_apiUrls.FondoApiUrl + $"api/{_version}/{_entidad}", content);
        }

        public async Task<HttpResponseMessage> Delete(Moneda request)
        {
            return await _httpClient.DeleteAsync(_apiUrls.FondoApiUrl + $"api/{_version}/{_entidad}/{request.Id}");
        }

        public async Task<HttpResponseMessage> GetById(Moneda request)
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

        public async Task<HttpResponseMessage> Update(Moneda request)
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
