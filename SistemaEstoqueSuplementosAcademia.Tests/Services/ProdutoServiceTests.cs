// Services/ProdutoServiceTests.cs
using Moq;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Produto;
using SistemaEstoqueSuplementosAcademia.Application.Services;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;
using Xunit;

namespace SistemaEstoqueSuplementosAcademia.Tests.Services
{
    public class ProdutoServiceTests
    {
        private readonly Mock<IProdutoRepository> _produtoRepoMock;
        private readonly Mock<ICategoriaRepository> _categoriaRepoMock;
        private readonly Mock<IMarcaRepository> _marcaRepoMock;
        private readonly Mock<IFornecedorRepository> _fornecedorRepoMock;
        private readonly ProdutoService _service;

        public ProdutoServiceTests()
        {
            _produtoRepoMock = new Mock<IProdutoRepository>();
            _categoriaRepoMock = new Mock<ICategoriaRepository>();
            _marcaRepoMock = new Mock<IMarcaRepository>();
            _fornecedorRepoMock = new Mock<IFornecedorRepository>();

            _service = new ProdutoService(
                _produtoRepoMock.Object,
                _categoriaRepoMock.Object,
                _marcaRepoMock.Object,
                _fornecedorRepoMock.Object);
        }

        [Fact]
        public async Task CriarAsync_ComCategoriaInexistente_DeveLancarExcecao()
        {
            var dto = new ProdutoCreateDto
            {
                Nome = "Whey 900g",
                CategoriaId = 99,
                MarcaId = 1,
                FornecedorId = 1,
                PrecoCompra = 50,
                PrecoVenda = 90,
                EstoqueMinimo = 5
            };

            _categoriaRepoMock.Setup(r => r.ObterPorIdAsync(99)).ReturnsAsync((Categoria?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarAsync(dto));

            _produtoRepoMock.Verify(r => r.AdicionarAsync(It.IsAny<Produto>()), Times.Never);
        }

        [Fact]
        public async Task CriarAsync_ComReferenciasValidas_DeveIniciarEstoqueZerado()
        {
            var dto = new ProdutoCreateDto
            {
                Nome = "Creatina 300g",
                CategoriaId = 1,
                MarcaId = 1,
                FornecedorId = 1,
                PrecoCompra = 30,
                PrecoVenda = 60,
                EstoqueMinimo = 10
            };

            _categoriaRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(new Categoria { Id = 1, Nome = "Creatina" });
            _marcaRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(new Marca { Id = 1, Nome = "Growth" });
            _fornecedorRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(new Fornecedor { Id = 1, Nome = "Fornecedor X", Cnpj = "11.111.111/0001-11" });

            Produto? produtoCriado = null;
            _produtoRepoMock
                .Setup(r => r.AdicionarAsync(It.IsAny<Produto>()))
                .Callback<Produto>(p => produtoCriado = p)
                .Returns(Task.CompletedTask);

            _produtoRepoMock
                .Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => produtoCriado);

            var resultado = await _service.CriarAsync(dto);

            Assert.Equal(0, resultado.EstoqueAtual);
        }
    }
}
