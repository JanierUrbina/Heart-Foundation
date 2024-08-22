using Heart_Foundation.DTO;
using Heart_Foundation.Helper;
using Heart_Foundation.Models;
using Heart_Foundation.Models.seguridad;
using Heart_Foundation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace Heart_Foundation.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly UserManager<Usuarios> _userManager;
        private readonly SignInManager<Usuarios> _signInManager;
        private readonly RoleManager<Roles> _roleManager;
        IFuncionesContext _funcionesC; IFunciones _funciones;
        private IPasswordHasher<Usuarios> passwordHasher;
        public UsuariosController(UserManager<Usuarios> userManager, SignInManager<Usuarios> signInManager, RoleManager<Roles> roleManager
            , IPasswordHasher<Usuarios> passwordHash, IFuncionesContext funcionesContext, IFunciones funciones)
        {//Patrón Facade
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _funcionesC = funcionesContext; _funciones = funciones;
            passwordHasher = passwordHash;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [Route("Usuarios/IndexJSON")]
        public JsonResult IndexJSON()
        {
            try
            {
                var lu =  _userManager.Users;
                var listaUsuarios = from p in lu where p.Estado select p; //Solo tomar los estados falsos (Habilitado)
                return Json(listaUsuarios);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult CargarRepresentantes(DTORepresentantes model)
        {
            return RedirectToAction("Solicitudes/AsignarSolicitudes");
        }

        public async Task<ActionResult> Registro(string correo = null, string nombre = null, string numero=null, long? id = null) //Null por default, en caso que traiga valor, se toma el valor
        {
            if (User.IsInRole("Admin")) // Verifica si es admin para agregar usuarios
            {
                var roles = await _roleManager.Roles.ToListAsync();
                ViewBag.roles = new SelectList(roles, "Name");
                TempData["mensaje"] = null;

                var model = new UsuarioViewModel
                {
                    correo = correo,
                    NombreCompleto = nombre,
                    Telefono = numero,
                    IdSU = id
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("UserLogin", "Login");
            }
        }


        // POST: UsuariosController/Create
        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<ActionResult> Registro(UsuarioViewModel model)
        {
            var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            //ViewData["Username"] = username;
            try
            {
               
                bool esEdicion = model.Id==null? false:true;
               
                if (ModelState.IsValid)
                {
                    if(esEdicion) // Es edicion
                    {
                        var existeUser = await _userManager.FindByIdAsync(model.Id); //Obtiene el usuario existente desde la base de datos
                        if (existeUser != null)
                        {
                            // Actualiza las propiedades del usuario
                            existeUser.UserName = model.NomUsuario;
                            existeUser.Email = model.correo;

                            // Obtiene los roles actuales del usuario
                            var existingRoles = await _userManager.GetRolesAsync(existeUser);

                            // Obtiene los roles que se agregaron y eliminaron
                            var rolesToAdd = model.nombrerol.Except(existingRoles);
                            var rolesToRemove = existingRoles.Except(model.nombrerol);

                            // Agrega los nuevos roles
                            foreach (var roleName in rolesToAdd)
                            {
                                await _userManager.AddToRoleAsync(existeUser, roleName);
                            }

                            // Elimina los roles que se quitaron
                            foreach (var roleName in rolesToRemove)
                            {
                                await _userManager.RemoveFromRoleAsync(existeUser, roleName);
                            }

                            // Actualiza el usuario en la base de datos
                            await _userManager.UpdateAsync(existeUser);

                            // Manda mensaje
                            TempData["mensaje"] = "Actualizado con éxito";

                            return RedirectToAction("Index");
                        }
                        else
                        {
                            // Maneja el caso donde el usuario no existe
                            ModelState.AddModelError("", "El usuario no existe.");
                            return View(model);
                        }
                    }
                    else
                    {
                        var existeNombre = (await _userManager.FindByNameAsync(model.NomUsuario))!=null?true:false;
                        var existecorreo = (await _userManager.FindByEmailAsync(model.correo))!=null?true:false;
                        if (existeNombre)
                        {
                            ModelState.AddModelError("", "El nombre de usuario ya existe");//si el nombre de usuaro ya existe mada mensaje de error
                        }
                        else if (existecorreo)
                        {
                            ModelState.AddModelError("", "El correo ingresado ya existe");//si el correo ya existe mada mensaje de error
                        }
                        else

                        {
                            // Create user
                            var user = new Usuarios { UserName = model.NomUsuario, Email = model.correo, Estado = true, FechaCreacion = DateTime.UtcNow, FechaModificacion = DateTime.UtcNow, NombreCompleto = model.NombreCompleto, PhoneNumber = model.Telefono };
                            var result = await _userManager.CreateAsync(user, model.Password);

                            if (result.Succeeded)
                            {
                                //Agrega el rol al usuario
                                foreach (var roleName in model.nombrerol)
                                {
                                    await _userManager.AddToRoleAsync(user, roleName);
                                }
                                //Agrega los claims al usuario
                                await _userManager.AddClaimAsync(user, new Claim("Id", user.Id));
                                await _userManager.AddClaimAsync(user, new Claim("Nombre", model.NomUsuario));

                                //manda mensaje
                                TempData["mensaje"] = "Creado con éxito";//mensaje de éxito
                                if(model.IdSU!=null)
                                {
                                    long id = (long)model.IdSU;
                                    var ent = _funciones.ObtenerRegistro<SolicitudUsuario>(id);
                                    _funcionesC.DarBaja<SolicitudUsuario>(ent);
                                }
                                return RedirectToAction("Index");//redireccionamos

                            }
                            ModelState.AddModelError("", "La contraseña debe llevar a menos una Mayúscula, un número y un caracter alfanumérico.");
                        }
                    }
                    
                }
                var roles = await _roleManager.Roles.ToListAsync();
                ViewBag.roles = new SelectList(roles, "Name");
                return View(new UsuarioViewModel());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Metodos de seguridad
        public IActionResult CreateRol() => View();

        [HttpPost]
        public async Task<IActionResult> CreateRol(string Rol)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (_roleManager.Roles.Any(u => u.Name == Rol))
                    {
                        ModelState.AddModelError("", "El rol ingresado ya existe");//si el nombre de usuaro ya existe mada mensaje de error
                    }
                    else
                    {
                        var rol = new Roles
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "Admin",
                            FechaCreacion = DateTime.UtcNow,
                            FechaModificacion = DateTime.UtcNow
                        };
                        IdentityResult result = await _roleManager.CreateAsync(rol); // Esperar la finalización de la creación del rol

                        if (result.Succeeded)
                            return RedirectToAction("Index", "Usuarios");
                    }
                }
                return View(Rol);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public async Task<JsonResult> ActualizarContrasena(string id, string Contraseña)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                    user.PasswordHash = passwordHasher.HashPassword(user, Contraseña);
                    var cambiado = await _userManager.UpdateAsync(user);
                    if (cambiado.Succeeded)
                    {
                        return Json(new { success = true, message = "Actualizado Correctamente" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error al actualizar" });
                    }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al actualizar" });
            }
        }
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
