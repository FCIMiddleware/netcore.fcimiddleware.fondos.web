using netcore.fcimiddleware.fondos.web.Models.Shared;
using netcore.fcimiddleware.fondos.web.Models.V1.Monedas;

namespace netcore.fcimiddleware.fondos.web.Services.Monedas
{
    public interface IMonedaProxy : IActions<Moneda>
    {
        Task<HttpResponseMessage> List(PaginationQueryRequest request);
    }
}
