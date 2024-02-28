using Microsoft.Extensions.Options;
using netcore.fcimiddleware.fondos.web.Models.V1.Moneda;
using netcore.fcimiddleware.fondos.web.Models.V1.Shared;
using netcore.fcimiddleware.fondos.web.Services.Proxies;
using System.Text;
using System.Text.Json;

namespace netcore.fcimiddleware.fondos.web.Services.Moneda
{
    public class MonedaProxy : IMonedaProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;
        private readonly ILogger<MonedaProxy> _logger;

        public MonedaProxy(
            IOptions<ApiUrls> apiUrls, 
            HttpClient httpClient, 
            ILogger<MonedaProxy> logger)
        {
            _apiUrls = apiUrls.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> CreateMoneda(CreateMonedaRequest request)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
                );

            return await _httpClient.PostAsync(_apiUrls.FondoApiUrl + "api/v1/Moneda", content);

            //if (result.StatusCode != System.Net.HttpStatusCode.OK)
            //{
            //    return 0;
            //}

            //var dateResponse = JsonSerializer.Deserialize<int>(
            //        await result.Content.ReadAsStringAsync(),
            //        new JsonSerializerOptions
            //        {
            //            PropertyNameCaseInsensitive = true
            //        }
            //    );

            //return dateResponse;
        }

        public async Task<HttpResponseMessage> DeleteMoneda(DeleteMonedaRequest request)
        {
            return await _httpClient.DeleteAsync(
                string.Format(_apiUrls.FondoApiUrl + "api/v1/Moneda/{0}",
                request.Id)
                );

            //if ((result.StatusCode != System.Net.HttpStatusCode.OK) && (result.StatusCode != System.Net.HttpStatusCode.NoContent))
            //{
            //    return false;
            //}

            //return true;
        }

        public async Task<HttpResponseMessage> GetByIdMoneda(GetByIdMonedaRequest request)
        {
            return await _httpClient.GetAsync(
                string.Format(_apiUrls.FondoApiUrl + "api/v1/Moneda/id?Id={0}",
                request.Id)
                );

            //if (result.StatusCode != System.Net.HttpStatusCode.OK)
            //{
            //    return null;
            //}

            //var dateResponse = JsonSerializer.Deserialize<GetByIdMonedaResponse>(
            //        await result.Content.ReadAsStringAsync(),
            //        new JsonSerializerOptions
            //        {
            //            PropertyNameCaseInsensitive = true
            //        }
            //    );

            //return dateResponse;
        }

        public async Task<HttpResponseMessage> PaginationMoneda(PaginationQueryRequest request)
        {

            return await _httpClient.GetAsync(
                string.Format(_apiUrls.FondoApiUrl + "api/v1/Moneda/pagination?PageIndex={0}&PageSize={1}&Search={2}&Sort={3}", 
                request.PageIndex,
                request.PageSize, 
                request.Search, 
                request.Sort)
                );

            //if (result.StatusCode != System.Net.HttpStatusCode.OK)
            //{
            //    return null;
            //}

            //var dateResponse = JsonSerializer.Deserialize<PaginationQueryResponse<GetByIdMonedaResponse>>(
            //        await result.Content.ReadAsStringAsync(),
            //        new JsonSerializerOptions
            //        {
            //            PropertyNameCaseInsensitive = true
            //        }
            //    );

            //return dateResponse;
        }

        public async Task<HttpResponseMessage> UpdateMoneda(UpdateMonedaRequest request)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
                );

            return await _httpClient.PutAsync(_apiUrls.FondoApiUrl + "api/v1/Moneda", content);

            //if ((result.StatusCode != System.Net.HttpStatusCode.OK) && (result.StatusCode != System.Net.HttpStatusCode.NoContent))
            //{
            //    return false;
            //}

            //return true;
        }
    }
}
