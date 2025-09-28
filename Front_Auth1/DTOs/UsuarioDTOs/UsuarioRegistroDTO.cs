using System.ComponentModel.DataAnnotations;

namespace Front_Auth1.DTOs
{
    public class UsuarioRegistroDTO
    {
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Campo para verificar que la contraseña se escribió correctamente
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        // Opcional: Si deseas que el Front-end envíe el rol predeterminado
        // De lo contrario, puedes manejar esto en la lógica de tu API.
        public string Role { get; set; } = "Operador";
    }
}

