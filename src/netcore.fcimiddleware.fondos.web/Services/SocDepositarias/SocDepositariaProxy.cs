using Microsoft.Extensions.Options;
using netcore.fcimiddleware.fondos.web.Models.Shared;
using netcore.fcimiddleware.fondos.web.Models.V1.SocDepositarias;
using netcore.fcimiddleware.fondos.web.Services.Proxies;
using System.Text;
using System.Text.Json;

namespace netcore.fcimiddleware.fondos.web.Services.SocDepositarias
{
    public class SocDepositariaProxy : ISocDepositariaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;
        private readonly ILogger<SocDepositariaProxy> _logger;
        private string _entidad = "SocDepositaria";
        private string _version = "v1";

        public SocDepositariaProxy(
            IOptions<ApiUrls> apiUrls,
            HttpClient httpClient,
            ILogger<SocDepositariaProxy> logger)
        {
            _apiUrls = apiUrls.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> Create(SocDepositaria request)
        {
            StringContent content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
                );

            return await _httpClient.PostAsync(_apiUrls.FondoApiUrl + $"api/{_version}/{_entidad}", content);            
        }

        public async Task<HttpResponseMessage> Delete(SocDepositaria request)
        {
            return await _httpClient.DeleteAsync(_apiUrls.FondoApiUrl + $"api/{_version}/{_entidad}/{request.Id}");
        }

        public async Task<HttpResponseMessage> GetById(SocDepositaria request)
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

        public async Task<HttpResponseMessage> Update(SocDepositaria request)
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
