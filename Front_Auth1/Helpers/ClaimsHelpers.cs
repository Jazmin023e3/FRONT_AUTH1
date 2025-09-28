using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Front_Auth1.Helpers
{
    // Helper para simplificar la extracción de claims o datos del usuario si se usa
    // la autenticación basada en ClaimsPrincipal (útil para Identity)
    public static class ClaimsHelpers
    {
        /// <summary>
        /// Verifica si el usuario actual está autenticado.
        /// </summary>
        public static bool IsAuthenticated(this HttpContext httpContext)
        {
            // Usamos el token de sesión como indicador primario de autenticación en este ejemplo.
            return !string.IsNullOrEmpty(httpContext.Session.GetToken());
        }

        /// <summary>
        /// Obtiene el ID del usuario de la sesión.
        /// </summary>
        public static int? GetUserId(this HttpContext httpContext)
        {
            return httpContext.Session.GetInt32("UserId");
        }

        /// <summary>
        /// Obtiene el rol del usuario de la sesión.
        /// </summary>
        public static string GetUserRole(this HttpContext httpContext)
        {
            return httpContext.Session.GetRole();
        }
    }
}
