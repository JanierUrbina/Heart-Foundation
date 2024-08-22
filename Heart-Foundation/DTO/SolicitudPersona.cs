namespace Heart_Foundation.DTO
{
    public class SolicitudPersona
    {
        #region Solicitud
        public long Id { get; set; }
        public string NumeroSolicitud { get; set; }
        public string Motivo { get; set; }
        public string Direccion { get; set; }
        #endregion

        #region Persona
        public string IdPersona { get; set; }
        public string NumeroTelefonico { get; set; }
        public string UserName { get; set; }
        public string Correo { get; set; }
        #endregion
    }
}
