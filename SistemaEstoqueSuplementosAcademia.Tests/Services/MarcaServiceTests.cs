using Moq;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Marca;
using SistemaEstoqueSuplementosAcademia.Application.Services;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;
using Xunit;

namespace SistemaEstoqueSuplementosAcademia.Tests.Services
{
    public class MarcaServiceTests
    {
        private readonly Mock<IMarcaRepository> _repositoryMock;
        private readonly MarcaService _service;

        public MarcaServiceTests()
        {
            _repositoryMock = new Mock<IMarcaRepository>();
            _service = new MarcaService(_repositoryMock.Object);
        }

        [Fact]
        public async Task CriarAsync_ComNomeNovo_DeveCriarMarcaComSucesso()
        {
            var dto = new MarcaCreateDto { Nome = "Growth" };
            _repositoryMock.Setup(r => r.ExisteComNomeAsync(dto.Nome, null)).ReturnsAsync(false);

            var resultado = await _service.CriarAsync(dto);

            Assert.Equal("Growth", resultado.Nome);
            Assert.True(resultado.Ativo);
            _repositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Marca>()), Times.Once);
        }

        [Fact]
        public async Task CriarAsync_ComNomeJaExistente_DeveLancarExcecao()
        {
            var dto = new MarcaCreateDto { Nome = "Max Titanium" };
            _repositoryMock.Setup(r => r.ExisteComNomeAsync(dto.Nome, null)).ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarAsync(dto));
        }

        [Fact]
        public async Task InativarAsync_ComIdExistente_DeveMarcarComoInativo()
        {
            var marca = new Marca { Id = 1, Nome = "Integralmedica", Ativo = true };
            _repositoryMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(marca);

            await _service.InativarAsync(1);

            Assert.False(marca.Ativo);
        }
    }
}
