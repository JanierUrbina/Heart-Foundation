using Heart_Foundation.DTO;
using Heart_Foundation.Models.seguridad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;

namespace Heart_Foundation.Controllers
{
    public class SessionController : Controller
    {
        private readonly UserManager<Usuarios> _userManager;
        private readonly SignInManager<Usuarios> _signInManager;

        private readonly RoleManager<Roles> _roleManager;
        public SessionController(UserManager<Usuarios> userManager, SignInManager<Usuarios> signInManager, RoleManager<Roles> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> MostrarVista(IList<string> rol)
        {
            string controllerName = "", actionName = "";

            if (rol != null)
            {
                if(rol.Count==0) 
                {
                    var user = await _userManager.GetUserAsync(User);
                    rol = await _userManager.GetRolesAsync(user); //Si solo accedió, se carga de nuevo los roles
                }
                if (rol.Contains("Admin"))
                {
                    controllerName = "Home"; actionName = "Admin";
                }
                else if (rol.Contains("Natural"))
                {
                    controllerName = "Solicitudes"; actionName = "MisSolicitudes";
                }
                else if (rol.Contains("Representante"))
                {
                    controllerName = "Representante"; actionName = "MisSolicitudes";
                }
            }

            return RedirectToAction(actionName, controllerName);
        }

        //Inicio
        [AllowAnonymous, HttpGet]
        public IActionResult Inicio()
        {
            return View();
        }
        [AllowAnonymous, HttpPost]
        public async Task<IActionResult> Inicio(DTOLogin login)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return View("Index", "Home");
                }
                if (ModelState.IsValid)
                {
                    var existe = _userManager.FindByEmailAsync(login.Email).Result; //Buscar el usuario por correo
                    if (existe is null || !await _userManager.CheckPasswordAsync(existe, login.Password)) // si no trae resultado o no tiene credenciales correctas
                    {
                        ModelState.AddModelError("", "Intento de inicio de sesión no válido.");
                        return View(login);
                    }
                    else
                    {
                        var result = await _signInManager.PasswordSignInAsync(existe.UserName, login.Password, false, lockoutOnFailure: false); //atento a iniciar sesión
                        if (result.Succeeded)
                        {
                            var rol = await _userManager.GetRolesAsync(existe);

                            return await MostrarVista(rol);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Intento de inicio de sesión no válido.");
                            return View(login);
                        }
                    }

                }
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        //Cerrar sesion
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); //cierra sesión del usuario actual
            return RedirectToAction("Index", "Home");
        }
    }
}
