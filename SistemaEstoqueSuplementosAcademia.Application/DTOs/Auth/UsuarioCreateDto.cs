// Application/DTOs/Auth/UsuarioCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Auth
{
    public class UsuarioCreateDto
    {
        [Required, MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        [Required]
        public string Perfil { get; set; } = string.Empty;
    }
}
