namespace netcore.fcimiddleware.fondos.web.Models.V1.Shared
{
    public class PaginationQueryRequest
    {
        public string? Search { get; set; } = string.Empty;
        public string? Sort { get; set; } = string.Empty;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
