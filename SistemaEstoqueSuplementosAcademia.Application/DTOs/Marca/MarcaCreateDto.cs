// Application/DTOs/Marca/MarcaCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Marca
{
    public class MarcaCreateDto
    {
        [Required(ErrorMessage = "O nome da marca é obrigatório.")]
        [MaxLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;
    }
}
