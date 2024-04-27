using netcore.fcimiddleware.fondos.web.Models.Shared;
using netcore.fcimiddleware.fondos.web.Models.V1.SocDepositarias;

namespace netcore.fcimiddleware.fondos.web.Services.SocDepositarias
{
    public interface ISocDepositariaProxy : IActions<SocDepositaria>
    {
        Task<HttpResponseMessage> List(PaginationQueryRequest request);
    }
}
