using Heart_Foundation.DTO;
using Heart_Foundation.Helper.Archivos;
using Heart_Foundation.Models;
using Heart_Foundation.Models.seguridad;
using Heart_Foundation.Models.Solicitudes;
using Heart_Foundation.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Transactions;
using static Heart_Foundation.Models.Solicitudes.Solicitud;

namespace Heart_Foundation.Helper
{
    public class Funciones : IFunciones
    {
        HeartDBContext _context;
        IFuncionesContext _Fcontext;
        IArchivo _archivo;
        private readonly UserManager<Usuarios> _userManager;
        public Funciones(HeartDBContext heartDBContext, UserManager<Usuarios> user, IFuncionesContext funcionesContext, IArchivo _archiv)
        {
            _context = heartDBContext;
            _Fcontext = funcionesContext;
            _userManager = user;
            _archivo = _archiv;
        }
        public List<T> Listar<T>() where T : EntidadBase
        {
            try
            {
                var dbSet = _context.Set<T>();  // Obtiene el DbSet correspondiente al tipo T
                var listaEntidades = dbSet.Where(e => e.Estado).ToList(); // Filtra y convierte a lista
                return listaEntidades;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar entidades", ex);
            }
        }

        public List<T> Listar<T>(params Expression<Func<T, object>>[] includes) where T : EntidadBase //Lista con sus includes
        {
            try
            {
                var dbSet = _context.Set<T>();  // Obtiene el DbSet correspondiente al tipo T

                // Aplica las inclusiones especificadas en el parámetro includes
                IQueryable<T> query = dbSet;
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                var listaEntidades = query.Where(e => e.Estado).ToList(); // Filtra y convierte a lista
                return listaEntidades;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar entidades", ex);
            }
        }


        public T ObtenerRegistro<T>(long id) where T : EntidadBase
        {
            try {
                var dbSet = _context.Set<T>();  // Obtiene el DbSet correspondiente al tipo T
                var entidad = dbSet.FirstOrDefault(e => e.Estado && e.Id == id); // Filtra y convierte a entidad
                return entidad;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar entidades", ex);
            }
        }

        public async Task<string> ObtenerIdUser(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            return user.Id;
        }
        public async Task<Usuarios> UserXId(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }
        public Solicitud AgregarSolicitud(Solicitud solicitud)
        {
            solicitud.EstadoSolicitud = Solicitud.EstadosSolicitudes.Solicitado;
            solicitud.SetGlobalValues();
            solicitud.NumeroSolicitud = GenerarNumeroSolicitud();
            solicitud.InicioSolicitud();
            _Fcontext.Agregar(solicitud);
            return solicitud;
           
        }
        public Solicitud EditarSolicitud(Solicitud solicitud)
        {
            var sol = this.ObtenerRegistro<Solicitud>(solicitud.Id);
            sol.Ubicacion = solicitud.Ubicacion;
            sol.Direccion = solicitud.Direccion;
            sol.Motivo = solicitud.Motivo;
            sol.NombreCompleto = solicitud.NombreCompleto;
            _Fcontext.Modificar(sol);
            return solicitud;

        }

        public string GenerarNumeroSolicitud()
        {
            int n;
            string NumeroSolicitud;
            n = ObtenerConsecutivo();           
            NumeroSolicitud = "S" + DateTime.Now.ToString("yyMMdd") + String.Format("{0:0000}", n);

            return NumeroSolicitud;
        }

        public int ObtenerConsecutivo()
        {
            int n;
            n = Listar<Solicitud>().Where(x => x.FechaCreacion >= DateTime.Now.Date).Count()+1;
            return n;
        }

        public void AgregarExpediente(Expediente expediente)
        {
            expediente.SetGlobalValues();
            _Fcontext.Agregar(expediente);
        }

        public List<Expediente> ObtenerRutaImagenes(long id)
        {
            var lista = new List<string>();
            var listado = Listar<Expediente>().Where(x => x.IdSolicitud == id).ToList();
            return listado;
        }

        public void AgregarNotaInformacion(DTONota Nota)
        {
            try
            {
                var ObjSol = this.ObtenerRegistro<Solicitud>(Nota.IdSolicitud);
                _context.Entry(ObjSol).Collection(s => s.Etapas).Load(); //join con etapa
               
                ObjSol.AgregarNotaInformacion(Nota.Mensaje, Nota.Usuario);
                _Fcontext.Modificar(ObjSol);
            }
            catch (Exception e)
            {
               
            }
        }

        public List<Nota> VerNotas(long IdSol)
        {
            var Solicitud = this._context.Solicitud.Include(x => x.Etapas).ThenInclude(n => n.Notas).FirstOrDefault(f => f.Id == IdSol);
            return Solicitud.Etapas.SelectMany(x => x.Notas).ToList();
        }

        public SolicitudUsuario AgregarSolicitudUsuario(SolicitudUsuario solicitud)
        {
            solicitud.SetGlobalValues();
            _Fcontext.Agregar(solicitud);
            return solicitud;
        }

        public void RechazarSol(long id)
        {
            try
            {
                var ObjSol = this.ObtenerRegistro<Solicitud>(id);                
                _context.Entry(ObjSol).Collection(s => s.Etapas).Load(); //join con etapa
                ObjSol.RechazarSolicitud();
                _Fcontext.Modificar(ObjSol);
            }
            catch (Exception e)
            {

            }
        }

        #region Institucion
        public Heart_Foundation.Models.Solicitudes.Representante AgregarRepresentante(RepresentanteViewModel model)
        {

           var representante = new Heart_Foundation.Models.Solicitudes.Representante()
           {
               NombreCompleto = model.Representante.NombreCompleto,
               Cargo = model.Representante.Cargo,
               UserName = model.Representante.UserName
           };
            var inst = new Heart_Foundation.Models.Solicitudes.Institucion()
            {
                Nombre = model.Institucion.Nombre,
                Descripcion = model.Institucion.Descripcion,
                UbicacionFileLogo = _archivo.GuardarArchivo(model.Institucion.UbicacionFileLogo, @"C:\\Imagenes\Institucion\Logos"),
                AreaEmpresa = (Heart_Foundation.Models.Solicitudes.Institucion.Area)model.Institucion.AreaEmpresa
            };
            representante.AgregarInstitucion(inst);
            representante.SetGlobalValues();
            inst.SetGlobalValues();
            _Fcontext.Agregar(representante);
            return representante;

        }

        public void RegistarUsuarioXRepresentante(DTORepresentantes model)
        {
            var sxr = new SolicitudXRepresentante()
            {
                IdRepresentante = model.idRepresentante,
                IdSolicitud = model.idSolicitud
            };
            sxr.SetGlobalValues();

            var sol = ObtenerRegistro<Solicitud>(model.idSolicitud);
            _context.Entry(sol).Collection(s => s.Etapas).Load(); //join con etapa

            sol.InicioProceso();
            _Fcontext.Agregar(sxr);
            _Fcontext.Modificar(sol);
        }

        public List<Solicitud> SolicitudesAsignadas(string username, int estado)
        {
            // Llama a Listar incluyendo la propiedad de navegación 'Solicitud'
            EstadosSolicitudes estadosSolicitudes = (EstadosSolicitudes)estado;
            var sxr = Listar<SolicitudXRepresentante>(x => x.Solicitud, x=>x.Representante).ToList().Where(x=>x.Representante.UserName==username && x.Solicitud.EstadoSolicitud == estadosSolicitudes);
            return sxr.Select(x => x.Solicitud).ToList();
        }

        //public Institucion EditarInstitucion(Institucion model)
        //{
        //    var inst = this.ObtenerRegistro<Institucion>(model.Id);
        //    inst.Nombre = model.Nombre;
        //    inst.Descripcion = model.Descripcion;
        //    inst.AreaEmpresa = model.AreaEmpresa;
        //    _Fcontext.Modificar(inst);
        //    return inst;

        //}
        #endregion

    }
}
