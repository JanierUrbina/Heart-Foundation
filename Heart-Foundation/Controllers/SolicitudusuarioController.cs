using Heart_Foundation.DTO;
using Heart_Foundation.Helper;
using Heart_Foundation.Models;
using Heart_Foundation.Models.Solicitudes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Heart_Foundation.Controllers
{
    public class SolicitudusuarioController : Controller
    {
        IFunciones _funciones;
        IFuncionesContext _funcionesC;
        public SolicitudusuarioController(IFunciones funciones, IFuncionesContext funcionesContext)
        {
            _funciones = funciones;
            _funcionesC = funcionesContext;
        }

        [HttpPost, AllowAnonymous]
        public IActionResult Solicitud()
        {
            try
            {
                SolicitudUsuario model = new SolicitudUsuario();
                var stream = new MemoryStream();
                Request.Body.CopyTo(stream);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    string requestbody = reader.ReadToEnd();
                    if (requestbody.Length > 0)
                    {
                        model = JsonConvert.DeserializeObject<SolicitudUsuario>(requestbody);
                    }
                }
                _funciones.AgregarSolicitudUsuario(model);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }

        [Route("Solicitudusuario/IndexJSON")]
        public async Task<JsonResult> IndexJSON()
        {
            try
            {
                var id = await _funciones.ObtenerIdUser(User);
                var listado = _funciones.Listar<SolicitudUsuario>().ToList();
                return Json(listado);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult Rechazar(long id)
        {
            try
            {
                var ent = _funciones.ObtenerRegistro<SolicitudUsuario>(id);
                _funcionesC.DarBaja<SolicitudUsuario>(ent);
                return Json(new { exito = true, message = "Se rechazó esta solicitud" });
            }
            catch (Exception)
            {
                return Json(new { exito = false, message = "No se pudo rechazar esta solicitud" });
            }

        }

    }
}
