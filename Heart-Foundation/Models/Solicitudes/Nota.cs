using System.ComponentModel.DataAnnotations.Schema;

namespace Heart_Foundation.Models.Solicitudes
{
    [Table("nota", Schema = "solicitud")]
    public class Nota : EntidadBase
    {
        public enum ClasificacionNota { Informacion, Denegacion }
        [Column("idetapa")]
        public long IdEtapa { get; set; }
        public ClasificacionNota TipoNota { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaHoraMensje { get; set; }
        public string Usuario { get; set; }
        [ForeignKey("IdEtapa")]
        public Etapa Etapa { get; set; }
    }
}
