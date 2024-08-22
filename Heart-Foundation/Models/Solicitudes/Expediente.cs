using System.ComponentModel.DataAnnotations.Schema;

namespace Heart_Foundation.Models.Solicitudes
{
    [Table("expediente", Schema = "solicitud")]
    public class Expediente : EntidadBase
    {
        public string Nota { get; set; }
        public string UbicacionFile { get; set; }
        public string ExtencionnFile { get; set; }

        [Column("idsolicitud")]
        public long IdSolicitud { get; set; }
        [ForeignKey("IdSolicitud")]
        public Solicitud Solicitud { get; set; }
    }
}
