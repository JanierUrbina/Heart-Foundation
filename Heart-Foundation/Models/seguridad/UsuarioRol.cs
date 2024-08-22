using Microsoft.AspNetCore.Identity;


namespace Heart_Foundation.Models.seguridad
{
    public class UsuarioRol : IdentityUserRole<string>
    {
        public override string UserId { get => base.UserId; set => base.UserId = value; }
        public override string RoleId { get => base.RoleId; set => base.RoleId = value; }
        public Usuarios Usuarios { get; set; }
        public Roles Roles { get; set; }

    } 
}
