namespace netcore.fcimiddleware.fondos.web.Models.V1.Moneda
{
    public class GetByIdMonedaResponse
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string? IdCAFCI { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public bool IsSincronized { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
