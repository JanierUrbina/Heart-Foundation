using Heart_Foundation.Models.Solicitudes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Heart_Foundation.ViewModels
{
    public class RepresentanteViewModel
    {
        public InstitucionVM Institucion { get; set; }
        public RepresentanteVM Representante { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errores = new List<ValidationResult>();

            // Validar Institucion
            if (Institucion != null)
            {
                if (string.IsNullOrWhiteSpace(Institucion.Nombre))
                {
                    errores.Add(new ValidationResult("El nombre de la institución no puede estar vacío", new[] { nameof(Institucion.Nombre) }));
                }
                if (string.IsNullOrWhiteSpace(Institucion.Descripcion))
                {
                    errores.Add(new ValidationResult("La descripción de la institución no puede estar vacía", new[] { nameof(Institucion.Descripcion) }));
                }
                if (Institucion.UbicacionFileLogo==null)
                {
                    errores.Add(new ValidationResult("La ubicación del archivo del logo no puede estar vacía", new[] { nameof(Institucion.UbicacionFileLogo) }));
                }
            }
            else
            {
                errores.Add(new ValidationResult("La institución no puede ser nula", new[] { nameof(Institucion) }));
            }

            // Validar Representante
            if (Representante != null)
            {
                if (string.IsNullOrWhiteSpace(Representante.NombreCompleto))
                {
                    errores.Add(new ValidationResult("El nombre completo del representante no puede estar vacío", new[] { nameof(Representante.NombreCompleto) }));
                }
                if (string.IsNullOrWhiteSpace(Representante.Cargo))
                {
                    errores.Add(new ValidationResult("El cargo del representante no puede estar vacío", new[] { nameof(Representante.Cargo) }));
                }
                if (string.IsNullOrWhiteSpace(Representante.UserName))
                {
                    errores.Add(new ValidationResult("El nombre de usuario del representante no puede estar vacío", new[] { nameof(Representante.UserName) }));
                }
            }
            else
            {
                errores.Add(new ValidationResult("El representante no puede ser nulo", new[] { nameof(Representante) }));
            }

            return errores;
        }
    }
    public class InstitucionVM
    {
        public long Id { get; set; }
        public enum Area { Gubernamental, Privada, Anonima }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public virtual IFormFile UbicacionFileLogo { get; set; }
        public int AreaEmpresa { get; set; }
    }

    public class RepresentanteVM
    {
        public long Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Cargo { get; set; }
        public string UserName { get; set; }
    }
}
