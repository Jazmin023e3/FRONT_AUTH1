using Front_Auth1.DTOs;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Front_Auth1.DTOs.UsuarioDTOs; // <-- Asegúrate de tener este using

namespace Front_Auth1.Services
{
    // Asegúrate de que AuthResponseDTO y UsuarioLoginDTO estén disponibles
    public class AuthService : ApiService
    {
        // El constructor debe coincidir con el constructor de ApiService
        public AuthService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
            : base(httpClient, httpContextAccessor)
        {
        }

        // Método asíncrono para iniciar sesión
        public async Task<AuthResponseDTO> LoginAsync(UsuarioLoginDTO model)
        {
            // 1. Adjuntar el token de autorización (si existe)
            // Aunque para el Login no es necesario, es buena práctica si reusas el cliente
            // AddAuthorizationHeader(); 

            // Si el AddAuthorizationHeader() es llamado aquí, puede dar un problema circular. 
            // Es mejor evitarlo para Login y Registro, ya que son públicos.

            // 2. Convierte el modelo de login a JSON
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 3. Llama al endpoint de la API
            // Usamos el cliente HTTP base (_httpClient)
            var response = await _httpClient.PostAsync("api/Auth/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponseDTO>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (authResponse == null)
                {
                    throw new Exception("Respuesta de autenticación inválida.");
                }

                return authResponse;
            }
            else
            {
                // Leer el mensaje de error de la API para mejor diagnóstico
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Credenciales inválidas o error de servidor: {errorContent}");
            }
        }

        // --- Método de Registro ---
        public async Task RegisterAsync(UsuarioRegistroDTO model)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Auth/Registrar", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"El registro falló: {errorContent}");
            }
        }
    }
}
