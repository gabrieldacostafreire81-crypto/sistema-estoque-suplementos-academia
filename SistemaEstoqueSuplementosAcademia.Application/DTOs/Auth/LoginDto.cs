// Application/DTOs/Auth/LoginDto.cs
using System.ComponentModel.DataAnnotations;

namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Auth
{
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;
    }
}