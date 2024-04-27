using netcore.fcimiddleware.fondos.web.Models.Shared;

namespace netcore.fcimiddleware.fondos.web.Services
{
    public interface IActions<T> where T : class
    {
        Task<HttpResponseMessage> Create(T request);
        Task<HttpResponseMessage> Update(T request);
        Task<HttpResponseMessage> Delete(T request);
        Task<HttpResponseMessage> Pagination(PaginationQueryRequest request);        
        Task<HttpResponseMessage> GetById(T request);
    }
}
