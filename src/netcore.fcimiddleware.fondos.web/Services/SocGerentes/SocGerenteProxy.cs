using Microsoft.Extensions.Options;
using netcore.fcimiddleware.fondos.web.Models.Shared;
using netcore.fcimiddleware.fondos.web.Models.V1.SocGerentes;
using netcore.fcimiddleware.fondos.web.Services.Proxies;
using System.Text;
using System.Text.Json;

namespace netcore.fcimiddleware.fondos.web.Services.SocGerentes
{
    public class SocGerenteProxy : ISocGerenteProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;
        private readonly ILogger<SocGerenteProxy> _logger;
        private string _entidad = "SocGerente";
        private string _version = "v1";

        public SocGerenteProxy(
            IOptions<ApiUrls> apiUrls,
            HttpClient httpClient,
            ILogger<SocGerenteProxy> logger)
        {
            _apiUrls = apiUrls.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> Create(SocGerente request)
        {
            StringContent content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
                );

            return await _httpClient.PostAsync(_apiUrls.FondoApiUrl + $"api/{_version}/{_entidad}", content);
        }

        public async Task<HttpResponseMessage> Delete(SocGerente request)
        {
            return await _httpClient.DeleteAsync(_apiUrls.FondoApiUrl + $"api/{_version}/{_entidad}/{request.Id}");
        }

        public async Task<HttpResponseMessage> GetById(SocGerente request)
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

        public async Task<HttpResponseMessage> List(PaginationQueryRequest request)
        {
            return
                await _httpClient
                .GetAsync(
                    _apiUrls.FondoApiUrl + $"api/{_version}/{_entidad}/list?PageIndex={request.PageIndex}&PageSize={request.PageSize}&Search={request.Search}&Sort={request.Sort}");
        }

        public async Task<HttpResponseMessage> Update(SocGerente request)
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
