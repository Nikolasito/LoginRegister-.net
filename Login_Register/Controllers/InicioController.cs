using Microsoft.AspNetCore.Mvc;

using Login_Register.Models;
using Login_Register.Recursos;
using Login_Register.Servicios.Contrato;

using System.Security.Claims; 
using Microsoft.AspNetCore.Authentication.Cookies;      //Autorizacion por cokies
using Microsoft.AspNetCore.Authentication;



namespace Login_Register.Controllers
{
    public class InicioController : Controller

    {
        private IUsuarioService _usuarioService;

        //Constructor
        public InicioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }


        //Registrarse
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario modelo)
        {
            modelo.Clave = Utilidades.EncriptarClave(modelo.Clave); //Encriptar clave SHA256

            Usuario usuario_creado = await _usuarioService.SaveUsuarios(modelo);

            if(usuario_creado.IdUsuario > 0)
            {
                return RedirectToAction("IniciarSesion", "Inicio");
            }
            else
            {
                ViewData["Mensaje"] = "No se pudo crear usuario";
                return View();
            } 
        }

        //INICIAR SESION
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion (string correo, string clave)
        {
            Usuario usuario_encontrado = await _usuarioService.GetUsuarios(correo, Utilidades.EncriptarClave(clave));

            if(usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario_encontrado.NombreUsuario)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties() //Propiedades de la Autentificacion
            {
                AllowRefresh = true    //Permiter que se refresque la autentificacion
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );
            return RedirectToAction("Index", "Home");
        }

    }
}
