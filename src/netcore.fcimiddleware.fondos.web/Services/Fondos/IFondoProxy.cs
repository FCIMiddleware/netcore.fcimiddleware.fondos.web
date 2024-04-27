using netcore.fcimiddleware.fondos.web.Models.V1.Fondos;

namespace netcore.fcimiddleware.fondos.web.Services.Fondos
{
    public interface IFondoProxy : IActions<Fondo>
    {
        Task<HttpResponseMessage> Create(FondoCreateRequest request);
    }
}
