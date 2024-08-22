using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Heart_Foundation.Models;
using Microsoft.AspNetCore.Identity;
using Heart_Foundation.Models.Solicitudes;

namespace Heart_Foundation.Helper
{
    public class FuncionesContext: IFuncionesContext
    {
        HeartDBContext _context;
        public FuncionesContext(HeartDBContext heartDBContext)
        {
            _context = heartDBContext;         
        }

        public void Agregar<T>(T Entidad)
        {
            if (_context.Entry(Entidad).State == EntityState.Detached)
            {
                this._context.Add(Entidad);
                _context.Entry(Entidad).State = EntityState.Added;
                _context.SaveChanges();
            }
           
        }

        public void Modificar<T>(T Entidad)
        {
            if (_context.Entry(Entidad).State == EntityState.Detached)
            {
                this._context.Attach(Entidad);
            }
            _context.Entry(Entidad).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DarBaja<T>(T Entidad) where T : EntidadBase
        {
            Entidad.Estado = false;
            this._context.Attach(Entidad);
            _context.Entry(Entidad).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
