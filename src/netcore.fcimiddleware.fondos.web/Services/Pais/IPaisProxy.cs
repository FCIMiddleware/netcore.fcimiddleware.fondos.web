using netcore.fcimiddleware.fondos.web.Models.V1.Pais;
using netcore.fcimiddleware.fondos.web.Models.V1.Shared;

namespace netcore.fcimiddleware.fondos.web.Services.Pais
{
    public interface IPaisProxy
    {
        Task<HttpResponseMessage> Create(CreatePaisRequest request);
        Task<HttpResponseMessage> Update(UpdatePaisRequest request);
        Task<HttpResponseMessage> Delete(DeletePaisRequest request);
        Task<HttpResponseMessage> Pagination(PaginationQueryRequest request);
        Task<HttpResponseMessage> GetById(GetByIdPaisRequest request);
    }
}
