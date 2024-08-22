using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Heart_Foundation.Models.Solicitudes
{
    [Table("representante", Schema = "solicitud")]
    public class Representante : EntidadBase
    {
        public string NombreCompleto { get; set; }
        public string Cargo { get; set; }
        public string UserName { get; set; }

        [Column("id")]
        public override long Id { get; set; }
        [ForeignKey("Id")] //Un representante, tiene una sola Institucion
        public Institucion Institucion { get; set; }

        public void AgregarInstitucion(Institucion NuevaInstitucion)
        {
            this.Institucion = NuevaInstitucion;
        }
    }
}
