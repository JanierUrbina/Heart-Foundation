using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Heart_Foundation.Models.seguridad
{
    public class Claims : IdentityUserClaim<string>
    {
        public override int Id { get => base.Id; set => base.Id = value; }
        public override string UserId { get => base.UserId; set => base.UserId = value; }
        public override string ClaimType { get => base.ClaimType; set => base.ClaimType = value; }
        public override string ClaimValue { get => base.ClaimValue; set => base.ClaimValue = value; }
        public Usuarios Usuarios { get; set; }
    }
}
