using Heart_Foundation.Models.seguridad;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Heart_Foundation.Models.seguridad
{
    public class Usuarios : IdentityUser
    {
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string NombreCompleto { get; set; }
        public int Version { get; set; }
        public bool Estado { get; set; }
        public ICollection<Claims> Claims { get; set; }
        public ICollection<Cuentas> Cuentas { get; set; }
        public ICollection<UsuarioRol> UsuarioRols { get; set; }
    }
}
