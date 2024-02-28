namespace netcore.fcimiddleware.fondos.web.Models.V1.Shared
{
    public class BadRequest
    {
        public string? Details { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
