using netcore.fcimiddleware.fondos.web.Models.V1.Monedas;
using netcore.fcimiddleware.fondos.web.Models.V1.Paises;
using netcore.fcimiddleware.fondos.web.Models.V1.SocDepositarias;
using netcore.fcimiddleware.fondos.web.Models.V1.SocGerentes;
using netcore.fcimiddleware.fondos.web.Models.V1.TpValorCptFondos;

namespace netcore.fcimiddleware.fondos.web.Models.V1.Fondos
{
    public class Fondo
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string? IdCAFCI { get; set; }
        public int MonedaId { get; set; }
        public Moneda Monedas { get; set; }
        public int SocGerenteId { get; set; }
        public SocGerente SocGerentes { get; set; }
        public int PaisId { get; set; }
        public Pais Paises { get; set; }
        public int SocDepositariaId { get; set; }
        public SocDepositaria SocDepositarias { get; set; }
        public ICollection<TpValorCptFondo> TpValorCptFondos { get; set; }        
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public bool IsSincronized { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
