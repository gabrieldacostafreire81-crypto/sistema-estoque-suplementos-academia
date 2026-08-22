// Application/DTOs/Categoria/CategoriaResponseDto.cs
namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Categoria
{
    public class CategoriaResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}