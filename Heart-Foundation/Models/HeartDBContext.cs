using Heart_Foundation.Models.seguridad;
using Heart_Foundation.Models.Solicitudes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Heart_Foundation.Models
{
    public class HeartDBContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public HeartDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("MyConnection"));
        }
        //Hecho a mano para obtener una seguridad personalizada.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de claves primarias
            modelBuilder.Entity<Usuarios>(x => {
                x.HasMany(e => e.Claims).WithOne(e => e.Usuarios).HasForeignKey(uc => uc.UserId).IsRequired();
                x.ToTable("usuarios", "seguridad");
                x.HasMany(e => e.Cuentas).WithOne(e => e.Usuarios).HasForeignKey(ul => ul.UserId).IsRequired();
                x.HasMany(e => e.UsuarioRols).WithOne(e => e.Usuarios).HasForeignKey(ul => ul.UserId).IsRequired();
            });

            modelBuilder.Entity<Roles>(x => {
                x.ToTable("roles", "seguridad");
                x.HasMany(e => e.UsuarioRols).WithOne(e => e.Roles).HasForeignKey(uc => uc.RoleId).IsRequired();
            });

            modelBuilder.Entity<Cuentas>(x => {
                x.HasOne(e => e.Usuarios).WithMany(e => e.Cuentas).HasForeignKey(ul => ul.LoginProvider).IsRequired();
                x.HasKey(l => new { l.LoginProvider, l.ProviderKey });
                x.Property(l => l.LoginProvider).HasMaxLength(128);
                x.Property(l => l.ProviderKey).HasMaxLength(128);
                x.ToTable("cuentas", "seguridad");
            });


            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("claims", "seguridad").HasKey(r => r.Id);
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("usuario_roles", "seguridad").HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("roleclaims", "seguridad").HasKey(rc => new { rc.Id});
            // Agrega otras configuraciones necesarias para entidades de Identity


            // Llama a la configuración de OnModelCreating de IdentityDbContext
            base.OnModelCreating(modelBuilder);
        }


        // Define DbSet para las entidades
        public virtual DbSet<Etapa> Etapas { get; set; }
        public virtual DbSet<Expediente> Expedientes { get; set; }
        public virtual DbSet<Institucion> Institucion { get; set; }
        public virtual DbSet<Nota> Notas { get; set; }
        public virtual DbSet<Representante> Representantes { get; set; }
        public virtual DbSet<Solicitud> Solicitud { get; set; }
        public virtual DbSet<SolicitudXRepresentante> SolicitudXRepresentante { get; set; }
        public virtual DbSet<SolicitudUsuario> SolicitudUsuario { get; set; }
    }
}
