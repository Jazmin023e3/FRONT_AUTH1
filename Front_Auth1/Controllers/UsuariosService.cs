using Front_Auth1.DTOs;
using Front_Auth1.DTOs.UsuarioDTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Front_Auth1.Controllers // El namespace coincide con la ubicación del archivo
{
    // Puedes inyectar este servicio en cualquier otro controlador (ej. UsuarioController)
    public class UsuariosService
    {
        private readonly HttpClient _httpClient;

        public UsuariosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        /// <summary>
        /// Obtiene la lista de usuarios, enviando el token JWT en el encabezado.
        /// </summary>
        /// <param name="jwtToken">El token JWT obtenido de la sesión.</param>
        public async Task<List<UsuarioDTO>> GetUsuariosAsync(string jwtToken)
        {
            // 1. Limpiar encabezados anteriores por si acaso
            _httpClient.DefaultRequestHeaders.Authorization = null;

            // 2. Adjuntar el token en formato "Bearer" (CLAVE)
            if (!string.IsNullOrEmpty(jwtToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", jwtToken);
            }
            else
            {
                throw new UnauthorizedAccessException("No se encontró el token de autenticación.");
            }

            // 3. Llamar al endpoint protegido de la API: /api/Usuarios
            var response = await _httpClient.GetAsync("api/Usuarios");

            if (response.IsSuccessStatusCode)
            {
                var usuarios = await response.Content.ReadFromJsonAsync<List<UsuarioDTO>>();
                return usuarios ?? new List<UsuarioDTO>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                     response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                // Manejo de token inválido o rol incorrecto
                throw new UnauthorizedAccessException("Acceso denegado. El token expiró o no tiene el rol requerido.");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener usuarios: {response.StatusCode}. Detalle: {errorContent}");
            }
        }
    }
}