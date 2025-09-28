using Microsoft.AspNetCore.Http;

namespace Front_Auth1.Helpers
{
    // Clase estática para manejar la lectura y escritura del Token JWT en la sesión
    public static class AuthHelpers
    {
        private const string TokenKey = "JWToken";
        private const string RoleKey = "UserRole";
        private const string UserIdKey = "UserId";

        /// <summary>
        /// Almacena el token JWT, el rol y el ID del usuario en la sesión.
        /// </summary>
        public static void SetAuthData(this ISession session, string token, string role, int userId)
        {
            session.SetString(TokenKey, token);
            session.SetString(RoleKey, role);
            session.SetInt32(UserIdKey, userId);
        }

        /// <summary>
        /// Obtiene el token JWT de la sesión.
        /// </summary>
        public static string GetToken(this ISession session)
        {
            return session.GetString(TokenKey);
        }

        /// <summary>
        /// Obtiene el rol del usuario de la sesión.
        /// </summary>
        public static string GetRole(this ISession session)
        {
            return session.GetString(RoleKey);
        }

        /// <summary>
        /// Limpia todos los datos de autenticación de la sesión.
        /// </summary>
        public static void ClearAuthData(this ISession session)
        {
            session.Remove(TokenKey);
            session.Remove(RoleKey);
            session.Remove(UserIdKey);
        }
    }
}
