namespace Front_Auth1.DTOs
{
    // DTO que mapea las propiedades de un usuario para el Front-end
    public class UsuarioDTO
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        // Asegúrate de que los nombres de estas propiedades coincidan 
        // exactamente con el JSON que devuelve tu API.
    }
}
