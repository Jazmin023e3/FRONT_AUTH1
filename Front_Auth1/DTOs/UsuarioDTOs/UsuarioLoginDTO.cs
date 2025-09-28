using System.ComponentModel.DataAnnotations;

namespace Front_Auth1.DTOs.UsuarioDTOs
{
    public class UsuarioLoginDTO
    {
        [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
