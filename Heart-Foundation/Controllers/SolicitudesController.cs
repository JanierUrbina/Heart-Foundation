using Heart_Foundation.Helper;
using Heart_Foundation.Models.Solicitudes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using Heart_Foundation.DTO;
using Heart_Foundation.Models;
using Heart_Foundation.Helper.Archivos;
using Heart_Foundation.Models.seguridad;

namespace Heart_Foundation.Controllers
{
    public class SolicitudesController : Controller
    {
        IFunciones _funciones;
        IArchivo _archivo;
        public SolicitudesController(IFunciones funciones, IArchivo _archiv)
        {
            _funciones = funciones;
            _archivo = _archiv;
        }
        // GET: SolicitudesController
        [Authorize]
        public ActionResult MisSolicitudes()
        {
            return View();
        }

        [Route("Solicitudes/IndexJSON")]
        public async Task<JsonResult> IndexJSON()
        {
            try
            {
                var id = await _funciones.ObtenerIdUser(User);
                var listado = _funciones.Listar<Solicitud>().Where(x => x.idpersona == id).ToList();
                return Json(listado);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize, HttpGet]
        public ActionResult Solicitud(long id)
        {
            Solicitud solicitud = new Solicitud();
            if (id!=0)
            {
                var registro = _funciones.ObtenerRegistro<Solicitud>(id) as Solicitud;
                return View(registro);
            }            
            return View(solicitud);
        }

        [Authorize, HttpPost]
        public async Task<ActionResult> Solicitud(Solicitud solicitud)
        {
           
                if(solicitud.Id!=0)
                {
                    var s = _funciones.EditarSolicitud(solicitud);
                }
                else
                {
                    solicitud.idpersona = await _funciones.ObtenerIdUser(User);
                    var s = _funciones.AgregarSolicitud(solicitud);
                }
                
            return RedirectToAction("MisSolicitudes");
        }

        [HttpPost]
        public JsonResult GuardarAdjuntos()
        {
            try
            {
                var files = Request.Form.Files;
                var notas = Request.Form["notas"]; // Obtener los detalles como una cadena
                var det = JsonConvert.DeserializeObject<List<Expediente>>(notas);
               
                int cont = 0;
               
                foreach (var file in files)
                {

                    string ruta = _archivo.GuardarArchivo(file, @"C:\\Imagenes");
                    var expe = new Expediente()
                    {
                        Nota = det[cont].Nota,
                        UbicacionFile = ruta,
                        ExtencionnFile = Path.GetExtension(file.FileName),
                        IdSolicitud = det[cont].IdSolicitud
                    };

                    _funciones.AgregarExpediente(expe);
                    cont++;
                }
               
                return Json(new { exito = true, message = "Creado con éxito" });
            }
            catch (Exception ex)
            {
               
                return Json(new { exito = false, message = ex.Message }); 
            }
        }


        [HttpPost]
        public JsonResult CargarImagenes(long id)
        {
            try
            {
                var listado = _funciones.ObtenerRutaImagenes(id);
                List<object> imagenesBase64 = new List<object>();

                foreach (var expediente in listado)
                {
                    using (var fileStream = new FileStream(expediente.UbicacionFile, FileMode.Open, FileAccess.Read))
                    {
                        var bytes = System.IO.File.ReadAllBytes(expediente.UbicacionFile);
                        string base64String = Convert.ToBase64String(bytes);
                        imagenesBase64.Add(new
                        {
                            Nota = expediente.Nota,
                            ImagenBase64 = base64String
                        });

                    }
                }
                if(imagenesBase64.Count == 0)
                {
                    return Json(new { exito = false, message = "Esta solicitud aún no cuenta con expedientes." });
                }
                return Json(new { exito = true, imagenes = imagenesBase64 });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        [Authorize]
        public ActionResult AsignarSolicitudes()
        {
            return View();
        }

        [Route("Solicitudes/AsignarSolicitudesJSON")]
        public JsonResult AsignarSolicitudesJSON()
        {
            try
            {              
                var listadoSolicitados = _funciones.Listar<Solicitud>().Where(x => x.EstadoSolicitud == Models.Solicitudes.Solicitud.EstadosSolicitudes.Solicitado).ToList();
                return Json(listadoSolicitados);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("Solicitudes/SolicitudesAsignadasJSON")]
        public JsonResult SolicitudesAsignadasJSON()
        {
            try
            {
                var Solicitados = _funciones.Listar<Solicitud>().Where(x => x.EstadoSolicitud == Models.Solicitudes.Solicitud.EstadosSolicitudes.Proceso).ToList();
                var sxr = _funciones.Listar<SolicitudXRepresentante>().ToList();
                var representantes = _funciones.Listar<Representante>().ToList();
                var listado = (from s in Solicitados
                               join sr in sxr on s.Id equals sr.IdSolicitud
                               join r in representantes on sr.IdRepresentante equals r.Id
                               where sxr.Count > 0
                               select new
                               {
                                   NombreCompleto = s.NombreCompleto,
                                   EstadoSolicitud = s.EstadoSolicitud,
                                   FechaCreacion = s.FechaCreacion,
                                   Representante = r.NombreCompleto
                               }).ToList();
                return Json(listado);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet, Authorize]
        public ActionResult AgregarNota(long IdSolicitud)
        {
            var Model = new DTONota()
            {
                Usuario = User.Identity.Name,
                IdSolicitud = IdSolicitud
            };
            return PartialView("AgregarNota", Model);
        }

        [HttpPost, Authorize]
        public ActionResult AgregarNota(DTONota Model)
        {
            if (ModelState.IsValid)
            {
                _funciones.AgregarNotaInformacion(Model);
                return Json(new { success = true, message = "La nota se ha guardado correctamente." });
            }
            else
            {
                return Json(new { success = false, message = "Ha ocurrido un error al guardar la nota." });
            }
        }


        [HttpGet, Authorize]
        public ActionResult VerNotas(long IdSolicitud)
        {
            var Model = _funciones.VerNotas(IdSolicitud);
            return PartialView("VerNotas", Model);
        }

        [HttpPost, Authorize]
        public ActionResult Rechazar(long id)
        {
            try
            {
                _funciones.RechazarSol(id);
                return Json(new { exito = true, message = "Se rechazó esta solicitud" });
            }
            catch (Exception)
            {
                return Json(new { exito = false, message = "No se pudo rechazar esta solicitud" });
            }

        }

        [HttpGet]
        public ActionResult ModalSolicitud(long IdSolicitud)
        {
            var model = _funciones.ObtenerRegistro<Solicitud>(IdSolicitud);
            var user = _funciones.UserXId(model.idpersona).Result;
            var newsp = new SolicitudPersona()
            {
                Id = model.Id,
                NumeroSolicitud = model.NumeroSolicitud,
                Motivo = model.Motivo,
                Direccion = model.Direccion,
                IdPersona = model.idpersona,
                NumeroTelefonico = "",
                UserName = user.UserName,
                Correo = user.Email
            };

            return PartialView("ModalSolicitud", newsp);
        }

        [HttpPost]
        public JsonResult ValidarSolicitud(long idsol, string Nota, bool Estado)
        {
            var sol = _funciones.ObtenerRegistro<Solicitud>(idsol);
            if(Estado)
            {
               sol.FinalizarSolicitud();
            }
            else
            {
                sol.EstadoSolicitud = Models.Solicitudes.Solicitud.EstadosSolicitudes.Rechazado;
                _funciones.RechazarSol(idsol);
                sol.NotaDenegacion(Nota, User.Identity.Name);
            }
            _funciones.EditarSolicitud(sol);
            return Json(new { bien = true });
        }


    }
}
