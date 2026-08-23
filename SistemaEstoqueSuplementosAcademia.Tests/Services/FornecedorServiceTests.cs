using Moq;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Fornecedor;
using SistemaEstoqueSuplementosAcademia.Application.Services;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;
using Xunit;

namespace SistemaEstoqueSuplementosAcademia.Tests.Services
{
    public class FornecedorServiceTests
    {
        private readonly Mock<IFornecedorRepository> _repositoryMock;
        private readonly FornecedorService _service;

        public FornecedorServiceTests()
        {
            _repositoryMock = new Mock<IFornecedorRepository>();
            _service = new FornecedorService(_repositoryMock.Object);
        }

        [Fact]
        public async Task CriarAsync_ComCnpjNovo_DeveCriarFornecedorComSucesso()
        {
            var dto = new FornecedorCreateDto { Nome = "Growth Suplementos", Cnpj = "12.345.678/0001-99" };
            _repositoryMock.Setup(r => r.ExisteComCnpjAsync(dto.Cnpj, null)).ReturnsAsync(false);

            var resultado = await _service.CriarAsync(dto);

            Assert.Equal("Growth Suplementos", resultado.Nome);
            Assert.True(resultado.Ativo);
            _repositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Fornecedor>()), Times.Once);
        }

        [Fact]
        public async Task CriarAsync_ComCnpjJaExistente_DeveLancarExcecao()
        {
            var dto = new FornecedorCreateDto { Nome = "Outro Fornecedor", Cnpj = "12.345.678/0001-99" };
            _repositoryMock.Setup(r => r.ExisteComCnpjAsync(dto.Cnpj, null)).ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarAsync(dto));

            _repositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Fornecedor>()), Times.Never);
        }

        [Fact]
        public async Task InativarAsync_ComIdExistente_DeveMarcarComoInativo()
        {
            var fornecedor = new Fornecedor { Id = 1, Nome = "Fornecedor X", Cnpj = "11.111.111/0001-11", Ativo = true };
            _repositoryMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(fornecedor);

            await _service.InativarAsync(1);

            Assert.False(fornecedor.Ativo);
        }
    }
}
