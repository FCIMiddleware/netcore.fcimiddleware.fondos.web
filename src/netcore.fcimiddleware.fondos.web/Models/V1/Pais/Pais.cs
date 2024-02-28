using System.ComponentModel.DataAnnotations;

namespace netcore.fcimiddleware.fondos.web.Models.V1.Pais
{
    public class Pais
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(250, ErrorMessage = "El {0} debe estar entre al menos {2} caracteres de longitud", MinimumLength = 5)]
        public string Descripcion { get; set; } = string.Empty;
        public string? IdCAFCI { get; set; } = string.Empty;
    }
}
