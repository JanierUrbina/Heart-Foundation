using Heart_Foundation.DTO;
using Heart_Foundation.Helper;
using Heart_Foundation.Models.seguridad;
using Heart_Foundation.Models.Solicitudes;
using Heart_Foundation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Heart_Foundation.Models.Solicitudes.Solicitud;

namespace Heart_Foundation.Controllers
{
    public class RepresentanteController : Controller
    {
        IFunciones _funciones;
        private readonly UserManager<Usuarios> _userManager;
        public RepresentanteController(IFunciones funciones, UserManager<Usuarios> userManager)
        {
            _funciones = funciones;
            _userManager = userManager;
        }
        public ActionResult Index()
        {
            return View();
        }
        [Route("Representante/IndexJSON")]
        public async Task<JsonResult> IndexJSON()
        {
            try
            {
                var rep = _funciones.Listar<Representante>().ToList();
                var inst = _funciones.Listar<Institucion>().ToList();
                var listado = (from r in rep join i in inst on r.Institucion.Id equals i.Id select new
                {
                    Id = r.Id,
                    Nombre = r.NombreCompleto,
                    Cargo = r.Cargo,
                    Institucion = i.Nombre                    
                }); 
                return Json(listado);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<Usuarios>> UserRoles()
        {
            var lu = _userManager.Users.ToListAsync();
            var listaUsuarios = lu.Result.Where(x => x.Estado).ToList();
            var usersInRole = new List<Usuarios>();

            //Representantes en su tabla
            var representantes = _funciones.Listar<Heart_Foundation.Models.Solicitudes.Representante>();

            foreach (var user in listaUsuarios)
            {
                if(!representantes.Any(X=>X.UserName==user.UserName)) //Solo Usuarios no agregados a la tabla representante
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Representante"))
                    {
                        usersInRole.Add(user);
                    }
                }
               
            }
            return usersInRole.ToList();
        }
        [HttpGet, Authorize]
        public async Task<ActionResult> Crear() 
        {
            try
            {
                var usersInRole = await UserRoles();
                ViewBag.Representantes = new SelectList(usersInRole, "UserName", "UserName");
                return View(new RepresentanteViewModel()
                {
                    Institucion = new InstitucionVM(),
                    Representante = new RepresentanteVM()
                });
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        [HttpPost, Authorize]
        public async Task<ActionResult> Crear([FromForm] RepresentanteViewModel model)
        {
            if(ModelState.IsValid) 
            {
                _funciones.AgregarRepresentante(model);
                return RedirectToAction("Index");
            }
            var usersInRole = await UserRoles();
            ViewBag.Representantes = new SelectList(usersInRole, "UserName", "UserName");
            return View(model);
        }

        [HttpPost]
        public JsonResult CargarLogo(long id)
        {
            try
            {
                var Institucion = _funciones.ObtenerRegistro<Institucion>(id);

                using (var fileStream = new FileStream(Institucion.UbicacionFileLogo, FileMode.Open, FileAccess.Read))
                {
                    //genera bytes apartir de ruta
                    var bytes = System.IO.File.ReadAllBytes(Institucion.UbicacionFileLogo);
                    string archivoruta = Convert.ToBase64String(bytes);
                    
                    return Json(new { exito = true, imagen = archivoruta });
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al cargar imagen, vuelva a intentarlo más tarde." });
            }
        }

        [Route("Representante/CargarRepresentantes"), HttpGet]
        public ActionResult CargarRepresentantes(long idsol)
        {
            try
            {
                var listado = _funciones.Listar<Representante>().ToList();
                ViewBag.Representantes = new SelectList(listado, "Id", "NombreCompleto");
                return PartialView("CargarRepresentantes", new DTORepresentantes()
                {
                    idSolicitud = idsol
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult CargarRepresentantes(DTORepresentantes model)
        {
            try
            {
                _funciones.RegistarUsuarioXRepresentante(model);
                return RedirectToAction("AsignarSolicitudes","Solicitudes");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult MisSolicitudes()
        {
            return View();
        }
        [Route("Representante/MisSolicitudesJSON")]
        public async Task<JsonResult> MisSolicitudesJSON()
        {
            try
            {
                string username = _userManager.GetUserAsync(User).Result.UserName;
                int estado = 2;
                var listado = _funciones.SolicitudesAsignadas(username, estado);
                return Json(listado);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
