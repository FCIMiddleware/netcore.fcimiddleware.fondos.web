using System.ComponentModel.DataAnnotations;

namespace netcore.fcimiddleware.fondos.web.Models.V1.Fondos
{
    public class FondoCreateRequest
    {
        [Required(ErrorMessage = "La Descripcion es obligatorio")]
        public string? Descripcion { get; set; }

        public string? IdCAFCI { get; set; }

        [Required(ErrorMessage = "La Moneda es obligatorio")]
        public int? MonedaId { get; set; }

        [Required(ErrorMessage = "La Sociedad Gerente es obligatorio")]
        public int? SocGerenteId { get; set; }

        [Required(ErrorMessage = "El Pais es obligatorio")]
        public int? PaisId { get; set; }

        [Required(ErrorMessage = "La Sociedad Depositaria es obligatorio")]
        public int? SocDepositariaId { get; set; }

        public bool IsSincronized { get; set; } = false;
    }
}
