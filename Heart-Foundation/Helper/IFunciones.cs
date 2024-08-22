using Heart_Foundation.DTO;
using Heart_Foundation.Models;
using Heart_Foundation.Models.seguridad;
using Heart_Foundation.Models.Solicitudes;
using Heart_Foundation.ViewModels;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Heart_Foundation.Helper
{
    public interface IFunciones
    {
        List<T> Listar<T>() where T : EntidadBase;
        List<T> Listar<T>(params Expression<Func<T, object>>[] includes) where T : EntidadBase;
        T ObtenerRegistro<T>(long id) where T : EntidadBase;
        Task<string> ObtenerIdUser(ClaimsPrincipal principal);
        Task<Usuarios> UserXId(string id);
        Solicitud AgregarSolicitud(Solicitud solicitud);
        Solicitud EditarSolicitud(Solicitud solicitud);
        void AgregarExpediente(Expediente expediente);
        List<Expediente> ObtenerRutaImagenes(long id);
        void AgregarNotaInformacion(DTONota Nota);
        List<Nota> VerNotas(long IdSol);
        SolicitudUsuario AgregarSolicitudUsuario(SolicitudUsuario solicitud);
        void RechazarSol(long id);
        Heart_Foundation.Models.Solicitudes.Representante AgregarRepresentante(RepresentanteViewModel model);
        void RegistarUsuarioXRepresentante(DTORepresentantes model);
        List<Solicitud> SolicitudesAsignadas(string username, int estado);
    }
}
