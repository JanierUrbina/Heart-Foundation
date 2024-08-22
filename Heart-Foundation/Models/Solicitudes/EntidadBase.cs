namespace Heart_Foundation.Models.Solicitudes
{
    //virtual te permite que una propiedad y método pueda ser sobreescrita por una clase que herede de ella.
    public class EntidadBase
    {
        public virtual long Id { get; set; }
        public virtual DateTime FechaCreacion { get; set; }
        public virtual DateTime FechaModificacion { get; set; }
        public virtual bool Estado { get; set; }

        public virtual void SetGlobalValues()
        {
            this.FechaCreacion = DateTime.UtcNow;
            this.FechaModificacion = DateTime.UtcNow;
            this.Estado = true;
        }
    }
}
