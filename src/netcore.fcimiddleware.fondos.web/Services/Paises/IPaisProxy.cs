using netcore.fcimiddleware.fondos.web.Models.Shared;
using netcore.fcimiddleware.fondos.web.Models.V1.Paises;

namespace netcore.fcimiddleware.fondos.web.Services.Paises
{
    public interface IPaisProxy : IActions<Pais>
    {
        Task<HttpResponseMessage> List(PaginationQueryRequest request);
    }
}
