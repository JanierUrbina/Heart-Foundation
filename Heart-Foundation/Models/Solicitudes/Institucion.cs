using System.ComponentModel.DataAnnotations.Schema;

namespace Heart_Foundation.Models.Solicitudes
{
    [Table("institucion", Schema = "solicitud")]
    public class Institucion : EntidadBase
    {
        public enum Area { Gubernamental, Privada, Anonima }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string UbicacionFileLogo { get; set; }
        public Area AreaEmpresa { get; set; }
        public virtual Representante Representante { get; set; }
    }
}
