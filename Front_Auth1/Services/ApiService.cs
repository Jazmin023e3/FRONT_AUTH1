using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Front_Auth1.Helpers;

namespace Front_Auth1.Services
{
    // Clase abstracta base que centraliza la lógica común de acceso a la API,
    // especialmente la inyección del token JWT en los encabezados.
    public abstract class ApiService
    {
        // Cliente HTTP que se inyecta con la URL base de la API.
        protected readonly HttpClient _httpClient;

        // Permite acceder a la sesión para leer el token JWT.
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Agrega el token JWT guardado en la sesión como encabezado de Autorización (Bearer).
        /// Todos los métodos protegidos de los servicios hijos deben llamar a este método primero.
        /// </summary>
        protected void AddAuthorizationHeader()
        {
            // Intentar obtener el token de la sesión usando el Helper
            var token = _httpContextAccessor.HttpContext?.Session.GetToken();

            // Limpiar cualquier encabezado previo
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(token))
            {
                // Establecer el encabezado Bearer Token
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}

