using netcore.fcimiddleware.fondos.web.Models.Shared;
using netcore.fcimiddleware.fondos.web.Models.V1.SocGerentes;

namespace netcore.fcimiddleware.fondos.web.Services.SocGerentes
{
    public interface ISocGerenteProxy : IActions<SocGerente>
    {
        Task<HttpResponseMessage> List(PaginationQueryRequest request);
    }
}
