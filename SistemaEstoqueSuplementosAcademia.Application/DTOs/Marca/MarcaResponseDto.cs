// Application/DTOs/Marca/MarcaResponseDto.cs
namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Marca
{
    public class MarcaResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
