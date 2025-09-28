using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Front_Auth1.Controllers; // Usamos el namespace donde definimos UsuariosService
using Front_Auth1.DTOs; // Necesario para usar UsuarioDTO

namespace Front_Auth1.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuariosService _usuariosService;

        // Inyección de Dependencias: El servicio se inyecta automáticamente
        public UsuarioController(UsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        // [GET] /Usuario/Index - Acción principal para ver la lista de usuarios
        public async Task<IActionResult> Index()
        {
            // 1. Obtener el token JWT de la sesión
            var token = HttpContext.Session.GetString("JWToken");

            // 2. Si no hay token, el usuario no está logueado, lo redirigimos al login
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                // 3. Llamar al servicio, el cual adjuntará el token a la petición HTTP
                var usuarios = await _usuariosService.GetUsuariosAsync(token);

                // 4. Mostrar la vista con la lista de usuarios
                return View(usuarios);
            }
            catch (UnauthorizedAccessException)
            {
                // Este error ocurre si la API devuelve 401 o 403 (token expirado, inválido o rol incorrecto)
                // Limpiamos la sesión para forzar un nuevo login.
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                // 5. Manejo de otros errores (conexión, serialización, etc.)
                ViewBag.ErrorMessage = "Error al cargar los datos de la API: " + ex.Message;
                // Devolvemos una lista vacía para no romper la vista
                return View(new List<UsuarioDTO>());
            }
        }
    }
}
