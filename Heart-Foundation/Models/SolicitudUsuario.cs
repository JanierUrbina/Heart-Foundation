using Heart_Foundation.Models.Solicitudes;

namespace Heart_Foundation.Models
{
    public class SolicitudUsuario: EntidadBase
    {
        public enum TipoUsuarios { Representante, Usuarios}
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string NumeroTelefonico { get; set; }
        public string Mensaje { get; set; }
        public TipoUsuarios TipoUsuario { get; set; }
    }
}
