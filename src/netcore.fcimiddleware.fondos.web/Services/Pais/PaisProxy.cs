using Microsoft.Extensions.Options;
using netcore.fcimiddleware.fondos.web.Models.V1.Pais;
using netcore.fcimiddleware.fondos.web.Models.V1.Shared;
using netcore.fcimiddleware.fondos.web.Services.Proxies;
using System.Text;
using System.Text.Json;

namespace netcore.fcimiddleware.fondos.web.Services.Pais
{
    public class PaisProxy : IPaisProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;
        private readonly ILogger<PaisProxy> _logger;

        public PaisProxy(
            IOptions<ApiUrls> apiUrls,
            HttpClient httpClient, 
            ILogger<PaisProxy> logger)
        {
            _apiUrls = apiUrls.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> Create(CreatePaisRequest request)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
                );

            return await _httpClient.PostAsync(_apiUrls.FondoApiUrl + "api/v1/Pais", content);
        }

        public async Task<HttpResponseMessage> Delete(DeletePaisRequest request)
        {
            return await _httpClient.DeleteAsync(
                string.Format(_apiUrls.FondoApiUrl + "api/v1/Pais/{0}",
                request.Id)
                );
        }

        public async Task<HttpResponseMessage> GetById(GetByIdPaisRequest request)
        {
            return await _httpClient.GetAsync(
                string.Format(_apiUrls.FondoApiUrl + "api/v1/Pais/id?Id={0}",
                request.Id)
                );
        }

        public async Task<HttpResponseMessage> Pagination(PaginationQueryRequest request)
        {
            return await _httpClient.GetAsync(
                string.Format(_apiUrls.FondoApiUrl + "api/v1/Pais/pagination?PageIndex={0}&PageSize={1}&Search={2}&Sort={3}",
                request.PageIndex,
                request.PageSize,
                request.Search,
                request.Sort)
                );
        }

        public async Task<HttpResponseMessage> Update(UpdatePaisRequest request)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
                );

            return await _httpClient.PutAsync(_apiUrls.FondoApiUrl + "api/v1/Pais", content);
        }
    }
}
