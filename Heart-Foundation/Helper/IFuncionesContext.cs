using Heart_Foundation.Models.Solicitudes;

namespace Heart_Foundation.Helper
{
    public interface IFuncionesContext
    {
        void Agregar<T>(T Entidad);
        void Modificar<T>(T Entidad);
        void DarBaja<T>(T Entidad) where T : EntidadBase;
    }
}
