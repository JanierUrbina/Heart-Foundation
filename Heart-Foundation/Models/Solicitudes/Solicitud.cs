using System.ComponentModel.DataAnnotations.Schema;

namespace Heart_Foundation.Models.Solicitudes
{
    [Table("solicitud", Schema = "solicitud")]
    public class Solicitud : EntidadBase
    {
        public Solicitud()
        {
            this.Etapas = new List<Etapa>();
            this.Expedientes = new List<Expediente>();
        }
        public enum EstadosSolicitudes { Solicitado, Rechazado, Proceso, Finalizado }
        public EstadosSolicitudes EstadoSolicitud { get; set; }
        public string NombreCompleto { get; set; }
        public string Direccion { get; set; }
        public string Ubicacion { get; set; }
        public string Motivo { get; set; }
        public string NumeroSolicitud { get; set; }
        public string idpersona { get; set; }

        [NotMapped]
        public virtual int Indice { get { return 0; } }
        public List<Etapa> Etapas { get; set; }
        public List<Expediente> Expedientes { get; set; }

        public virtual void InicioSolicitud()
        {
            this.Etapas.Add(new Etapa()
            {
                IdentificadorEtapa = 0, //Inicio
                FechaCreacion = DateTime.UtcNow,
                FechaModificacion = DateTime.UtcNow,
                FechaInicio = DateTime.UtcNow,
                FechaFinal = DateTime.UtcNow,
                EstadoEtapa = Etapa.EstEtapa.Finalizado,
                Estado = true
            });
        }

        public Nota AgregarNotaInformacion(string Nota, string Usuario)
        {
            var EtapaAc = this.Etapas.FirstOrDefault(x => x.IdentificadorEtapa == this.Indice);
            var NotaCreada = EtapaAc.AgregarNotaInfo(Nota, Usuario);
            return NotaCreada;
        }

        public void RechazarSolicitud()
        {
            this.EstadoSolicitud = EstadosSolicitudes.Rechazado;
            var etapa = this.Etapas.FirstOrDefault(x => x.IdentificadorEtapa == Indice);
            etapa.IdentificadorEtapa = 2; //Rechazado
            etapa.FechaFinal = DateTime.UtcNow;
        }

        public void InicioProceso()
        {
            this.EstadoSolicitud = EstadosSolicitudes.Proceso;
            this.Etapas.Add(new Etapa()
            {
                IdentificadorEtapa = 1, //Proceso
                FechaCreacion = DateTime.UtcNow,
                FechaModificacion = DateTime.UtcNow,
                FechaInicio = DateTime.UtcNow,
                FechaFinal = DateTime.UtcNow,
                EstadoEtapa = Etapa.EstEtapa.Finalizado,
                Estado = true
            });
        }
        public void FinalizarSolicitud()
        {
            this.EstadoSolicitud = EstadosSolicitudes.Finalizado;
            this.Etapas.Add(new Etapa()
            {
                IdentificadorEtapa = 3, //Fin Solicitud
                FechaCreacion = DateTime.UtcNow,
                FechaModificacion = DateTime.UtcNow,
                FechaInicio = DateTime.UtcNow,
                FechaFinal = DateTime.UtcNow,
                EstadoEtapa = Etapa.EstEtapa.Finalizado,
                Estado = true
            });
           
        }
        public void NotaDenegacion(string Comentario, string Usuario)
        {
            this.Etapas.FirstOrDefault().AgregarNotaDenegacion(Comentario, Usuario);
        }
    }
}
