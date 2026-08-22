// Application/DTOs/Categoria/CategoriaCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Categoria
{
    public class CategoriaCreateDto
    {
        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [MaxLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;
    }
}