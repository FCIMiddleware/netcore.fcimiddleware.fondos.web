using netcore.fcimiddleware.fondos.web.Models.V1.Moneda;
using netcore.fcimiddleware.fondos.web.Models.V1.Shared;

namespace netcore.fcimiddleware.fondos.web.Services.Moneda
{
    public interface IMonedaProxy
    {
        Task<HttpResponseMessage> Create(CreateMonedaRequest request);
        Task<HttpResponseMessage> Update(UpdateMonedaRequest request);
        Task<HttpResponseMessage> Delete(DeleteMonedaRequest request);
        Task<HttpResponseMessage> Pagination(PaginationQueryRequest request);
        Task<HttpResponseMessage> GetById(GetByIdMonedaRequest request);
    }
}
