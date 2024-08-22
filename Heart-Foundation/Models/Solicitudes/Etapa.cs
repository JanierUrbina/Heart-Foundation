using System.ComponentModel.DataAnnotations.Schema;

namespace Heart_Foundation.Models.Solicitudes
{
    [Table("etapa", Schema = "solicitud")]
    public class Etapa : EntidadBase
    {
        public Etapa()
        {
            Notas = new List<Nota>();
        }
        public enum EstEtapa { Inicio, Finalizado, Pendiente }
        public EstEtapa EstadoEtapa { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public int IdentificadorEtapa { get; set; }

        [Column("idsolicitud")]
        public long IdSolicitud { get; set; }
        [ForeignKey("IdSolicitud")]
        public Solicitud Solicitud { get; set; }

        public List<Nota> Notas { get; set; }

        public Nota AgregarNotaInfo(string Comentario, string Usuario)
        {
            var NewNota = new Nota()
            {
                TipoNota = Nota.ClasificacionNota.Informacion,
                FechaHoraMensje = DateTime.UtcNow,
                Mensaje = Comentario,
                Usuario = Usuario
            };
            NewNota.SetGlobalValues();
            this.Notas.Add(NewNota);
            return NewNota;
        }

        public Nota AgregarNotaDenegacion(string Comentario, string Usuario)
        {
            var NewNota = new Nota()
            {
                TipoNota = Nota.ClasificacionNota.Denegacion,
                FechaHoraMensje = DateTime.UtcNow,
                Mensaje = Comentario,
                Usuario = Usuario
            };
            NewNota.SetGlobalValues();
            this.Notas.Add(NewNota);
            return NewNota;
        }
    }
}
