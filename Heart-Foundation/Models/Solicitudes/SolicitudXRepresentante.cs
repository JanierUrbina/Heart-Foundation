using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Heart_Foundation.Models.Solicitudes
{
    [Table("solicituxrepresentante", Schema = "solicitud")]
    public class SolicitudXRepresentante : EntidadBase
    {
        [ForeignKey("Solicitud")]
        public long IdSolicitud { get; set; }

        public Solicitud Solicitud { get; set; }
        [ForeignKey("Representante")]
        public long IdRepresentante { get; set; }

        public Representante Representante { get; set; }

    }
}
