using netcore.fcimiddleware.fondos.web.Models.V1.Moneda;
using netcore.fcimiddleware.fondos.web.Models.V1.Shared;

namespace netcore.fcimiddleware.fondos.web.Services.Moneda
{
    public interface IMonedaProxy
    {
        Task<HttpResponseMessage> CreateMoneda(CreateMonedaRequest request);
        Task<HttpResponseMessage> UpdateMoneda(UpdateMonedaRequest request);
        Task<HttpResponseMessage> DeleteMoneda(DeleteMonedaRequest request);
        Task<HttpResponseMessage> PaginationMoneda(PaginationQueryRequest request);
        Task<HttpResponseMessage> GetByIdMoneda(GetByIdMonedaRequest request);
    }
}
