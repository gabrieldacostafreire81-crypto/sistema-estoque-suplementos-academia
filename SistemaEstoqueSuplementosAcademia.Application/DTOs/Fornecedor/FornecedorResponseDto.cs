// Application/DTOs/Fornecedor/FornecedorResponseDto.cs
namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Fornecedor
{
    public class FornecedorResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public bool Ativo { get; set; }
    }
}
