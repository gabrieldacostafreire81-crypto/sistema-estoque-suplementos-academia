using Moq;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Categoria;
using SistemaEstoqueSuplementosAcademia.Application.Services;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;
using Xunit;

namespace SistemaEstoqueSuplementosAcademia.Tests.Services
{
    public class CategoriaServiceTests
    {
        private readonly Mock<ICategoriaRepository> _repositoryMock;
        private readonly CategoriaService _service;

        public CategoriaServiceTests()
        {
            _repositoryMock = new Mock<ICategoriaRepository>();
            _service = new CategoriaService(_repositoryMock.Object);
        }

        [Fact]
        public async Task CriarAsync_ComNomeNovo_DeveCriarCategoriaComSucesso()
        {
            // Arrange
            var dto = new CategoriaCreateDto { Nome = "Creatina" };
            _repositoryMock
                .Setup(r => r.ExisteComNomeAsync(dto.Nome, null))
                .ReturnsAsync(false);

            // Act
            var resultado = await _service.CriarAsync(dto);

            // Assert
            Assert.Equal("Creatina", resultado.Nome);
            Assert.True(resultado.Ativo);
            _repositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Categoria>()), Times.Once);
            _repositoryMock.Verify(r => r.SalvarAsync(), Times.Once);
        }

        [Fact]
        public async Task CriarAsync_ComNomeJaExistente_DeveLancarExcecao()
        {
            // Arrange
            var dto = new CategoriaCreateDto { Nome = "Whey Protein" };
            _repositoryMock
                .Setup(r => r.ExisteComNomeAsync(dto.Nome, null))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.CriarAsync(dto));

            _repositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Categoria>()), Times.Never);
        }

        [Fact]
        public async Task AtualizarAsync_ComIdInexistente_DeveLancarExcecao()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.ObterPorIdAsync(999))
                .ReturnsAsync((Categoria?)null);

            var dto = new CategoriaUpdateDto { Nome = "Pré-treino" };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.AtualizarAsync(999, dto));
        }

        [Fact]
        public async Task InativarAsync_ComIdExistente_DeveMarcarComoInativo()
        {
            // Arrange
            var categoria = new Categoria { Id = 1, Nome = "Vitaminas", Ativo = true };
            _repositoryMock
                .Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync(categoria);

            // Act
            await _service.InativarAsync(1);

            // Assert
            Assert.False(categoria.Ativo);
            _repositoryMock.Verify(r => r.Atualizar(categoria), Times.Once);
            _repositoryMock.Verify(r => r.SalvarAsync(), Times.Once);
        }
    }
}