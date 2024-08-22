using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Heart_Foundation.ViewModels
{
    public class UsuarioViewModel : IValidatableObject
    {
      
            [Display(Name = "Id")]
            public string? Id { get; set; }

            [Required]
            [Display(Name = "Usuario")]
            public string NomUsuario { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [Display(Name = "nombrerol")]
            public List<string> nombrerol { get; set; }

            //Correo

            [Display(Name = "correo")]
            public string correo { get; set; }

            [Display(Name = "NombreCompleto")]
            public string NombreCompleto { get; set; }
            
            [Display(Name = "Telefono")]
            public string Telefono { get; set; }

            public long? IdSU { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
           {
               var errores = new List<ValidationResult>();

               if (correo != null)
               {
                   var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                   if (!regex.IsMatch(correo))
                   {
                       errores.Add(new ValidationResult("El correo electrónico es inválido"));
                   }
               }
               if (Password.Length < 6)
               {
                   errores.Add(new ValidationResult("La contraseña debe tener al menos 6 caractares"));
               }
               return errores;
           }
            
        }
    }
