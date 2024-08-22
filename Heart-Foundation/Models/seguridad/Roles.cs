using Microsoft.AspNetCore.Identity;

namespace Heart_Foundation.Models.seguridad
{
    public class Roles : IdentityRole<string>
    {
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public ICollection<UsuarioRol> UsuarioRols { get; set; }
    }

}
