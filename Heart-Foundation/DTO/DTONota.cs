using System.ComponentModel.DataAnnotations;

namespace Heart_Foundation.DTO
{
    public class DTONota
    {
        [Required]
        public long IdSolicitud { get; set; }

        [Required, MaxLength(512)]
        public string Mensaje { get; set; }

        public string Usuario { get; set; }
    }
}
