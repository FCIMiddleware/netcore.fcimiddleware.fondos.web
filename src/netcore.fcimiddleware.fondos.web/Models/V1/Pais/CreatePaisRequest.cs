using System.ComponentModel.DataAnnotations;

namespace netcore.fcimiddleware.fondos.web.Models.V1.Pais
{
    public class CreatePaisRequest
    {
        [Required(ErrorMessage = "El Descripcion es obligatorio")]
        [StringLength(250, ErrorMessage = "El {0} debe estar entre al menos {2} caracteres de longitud", MinimumLength = 5)]
        public string Descripcion { get; set; }
        public string? IdCAFCI { get; set; } = string.Empty;
        public bool IsSincronized { get; set; } = false;
    }
}
