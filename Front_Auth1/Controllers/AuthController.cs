using Microsoft.AspNetCore.Mvc;
using Front_Auth1.DTOs; // <- Aquí están tus DTOs (UsuarioLoginDTO, AuthResponseDTO, etc.)
using Front_Auth1.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks; // Necesario para Task
using System;
using Front_Auth1.DTOs.UsuarioDTOs; // Necesario para Exception

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

        // --- LOGIN ---

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
                // Puedes usar esto para depurar, pero no lo muestres al usuario final: 
                // Console.WriteLine(ex.Message); 
                return View(model);
            }
        }

        // --- REGISTRO ---

        // [GET] /Auth/Registrar
        [HttpGet]
        public IActionResult Registrar()
        {
            // Asegúrate de que no hay lógica compleja aquí. 
            // Si tienes alguna lógica que depende de servicios, coméntala temporalmente.

            // Devolvemos el DTO (Front_Auth1.DTOs.UsuarioDTOs.UsuarioRegistroDTO)
            return View(new UsuarioRegistroDTO());
        }

        // [POST] /Auth/Registrar
        [HttpPost]
        public async Task<IActionResult> Registrar(UsuarioRegistroDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Llama al servicio de registro (no necesitamos la respuesta)
                await _authService.RegisterAsync(model);

                // Redirige al login para que el nuevo usuario inicie sesión
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Error al registrar el usuario. El email o nombre de usuario ya existe.");
                return View(model);
            }
        }

        // --- LOGOUT ---

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
