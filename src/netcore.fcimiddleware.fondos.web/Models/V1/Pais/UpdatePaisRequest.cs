using System.ComponentModel.DataAnnotations;

namespace netcore.fcimiddleware.fondos.web.Models.V1.Pais
{
    public class UpdatePaisRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La Descripcion es obligatoria")]
        [StringLength(250, ErrorMessage = "El {0} debe estar entre al menos {2} caracteres de longitud", MinimumLength = 1)]
        public string Descripcion { get; set; }
        public string? IdCAFCI { get; set; } = string.Empty;
    }
}
