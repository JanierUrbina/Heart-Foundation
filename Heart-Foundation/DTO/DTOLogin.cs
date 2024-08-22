using System.ComponentModel.DataAnnotations;

namespace Heart_Foundation.DTO
{
    public class DTOLogin
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
