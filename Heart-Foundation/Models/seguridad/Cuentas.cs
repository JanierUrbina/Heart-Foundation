using Microsoft.AspNetCore.Identity;

namespace Heart_Foundation.Models.seguridad
{
    public class Cuentas : IdentityUserLogin<string>
    {
        public override string UserId { get => base.UserId; set => base.UserId = value; }
        public override string LoginProvider { get => base.LoginProvider; set => base.LoginProvider = value; }
        public override string ProviderKey { get => base.ProviderKey; set => base.ProviderKey = value; }
        public Usuarios Usuarios { get; set; }
    }
}
