using Microsoft.AspNetCore.Mvc;
using Front_Auth1.DTOs;
using Front_Auth1.Services;
using Microsoft.AspNetCore.Http;
using Front_Auth1.DTOs.UsuarioDTOs;

namespace Front_Auth1.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        // Inyección del servicio
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // [GET] /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            // Si ya hay un token en sesión, redirigimos al inicio
            if (HttpContext.Session.GetString("JWToken") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new UsuarioLoginDTO()); // Muestra la vista del formulario
        }

        // [POST] /Auth/Login (Envío del formulario)
        [HttpPost]
        public async Task<IActionResult> Login(UsuarioLoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // 1. Llamar al servicio para comunicarse con la API
                var authResult = await _authService.LoginAsync(model);

                // 2. Almacenar el Token y el Rol en la Sesión (CLAVE)
                HttpContext.Session.SetString("JWToken", authResult.Token);
                HttpContext.Session.SetString("UserRole", authResult.Role);
                HttpContext.Session.SetInt32("UserId", authResult.UserId);

                // 3. Redirigir a la página principal
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // 4. Mostrar un mensaje de error al usuario
                ModelState.AddModelError(string.Empty, "Inicio de sesión fallido. Verifique las credenciales.");
                return View(model);
            }
        }

        // [GET] /Auth/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            // Limpia toda la sesión (elimina el token)
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}