// Services/EstoqueServiceTests.cs
using Microsoft.EntityFrameworkCore;
using Moq;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Estoque;
using SistemaEstoqueSuplementosAcademia.Application.Services;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;
using SistemaEstoqueSuplementosAcademia.Infrastructure.Data;
using Xunit;

namespace SistemaEstoqueSuplementosAcademia.Tests.Services
{
    public class EstoqueServiceTests
    {
        private static AppDbContext CriarContextoEmMemoria()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task RegistrarSaida_ComQuantidadeMaiorQueEstoque_DeveLancarExcecaoENaoAlterarEstoque()
        {
            var produto = new Produto { Id = 1, Nome = "Whey", EstoqueAtual = 5, EstoqueMinimo = 2 };

            var produtoRepoMock = new Mock<IProdutoRepository>();
            produtoRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(produto);

            var movimentacaoRepoMock = new Mock<IMovimentacaoEstoqueRepository>();

            var context = CriarContextoEmMemoria();
            var unitOfWork = new UnitOfWork(context);
            var service = new EstoqueService(produtoRepoMock.Object, movimentacaoRepoMock.Object, unitOfWork);

            var dto = new MovimentacaoEntradaSaidaDto { ProdutoId = 1, UsuarioId = 1, Quantidade = 10 };

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.RegistrarSaidaAsync(dto));

            Assert.Equal(5, produto.EstoqueAtual);
            movimentacaoRepoMock.Verify(r => r.AdicionarAsync(It.IsAny<MovimentacaoEstoque>()), Times.Never);
        }

        [Fact]
        public async Task RegistrarSaida_ComQuantidadeIgualAoEstoque_DeveZerarEstoqueComSucesso()
        {
            var produto = new Produto { Id = 1, Nome = "Whey", EstoqueAtual = 5, EstoqueMinimo = 2 };

            var produtoRepoMock = new Mock<IProdutoRepository>();
            produtoRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(produto);

            var movimentacaoRepoMock = new Mock<IMovimentacaoEstoqueRepository>();

            var context = CriarContextoEmMemoria();
            var unitOfWork = new UnitOfWork(context);
            var service = new EstoqueService(produtoRepoMock.Object, movimentacaoRepoMock.Object, unitOfWork);

            var dto = new MovimentacaoEntradaSaidaDto { ProdutoId = 1, UsuarioId = 1, Quantidade = 5 };

            var resultado = await service.RegistrarSaidaAsync(dto);

            Assert.Equal(0, produto.EstoqueAtual);
            Assert.Equal(0, resultado.EstoqueResultante);
        }

        [Fact]
        public async Task RegistrarEntrada_DeveAumentarEstoqueCorretamente()
        {
            var produto = new Produto { Id = 1, Nome = "Creatina", EstoqueAtual = 10, EstoqueMinimo = 2 };

            var produtoRepoMock = new Mock<IProdutoRepository>();
            produtoRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(produto);

            var movimentacaoRepoMock = new Mock<IMovimentacaoEstoqueRepository>();

            var context = CriarContextoEmMemoria();
            var unitOfWork = new UnitOfWork(context);
            var service = new EstoqueService(produtoRepoMock.Object, movimentacaoRepoMock.Object, unitOfWork);

            var dto = new MovimentacaoEntradaSaidaDto { ProdutoId = 1, UsuarioId = 1, Quantidade = 20 };

            var resultado = await service.RegistrarEntradaAsync(dto);

            Assert.Equal(30, produto.EstoqueAtual);
            Assert.Equal(30, resultado.EstoqueResultante);
        }

        [Fact]
        public async Task RegistrarAjuste_DeveDefinirEstoqueExatoInformado()
        {
            var produto = new Produto { Id = 1, Nome = "Pré-treino", EstoqueAtual = 8, EstoqueMinimo = 2 };

            var produtoRepoMock = new Mock<IProdutoRepository>();
            produtoRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(produto);

            var movimentacaoRepoMock = new Mock<IMovimentacaoEstoqueRepository>();

            var context = CriarContextoEmMemoria();
            var unitOfWork = new UnitOfWork(context);
            var service = new EstoqueService(produtoRepoMock.Object, movimentacaoRepoMock.Object, unitOfWork);

            var dto = new MovimentacaoAjusteDto
            {
                ProdutoId = 1,
                UsuarioId = 1,
                NovaQuantidade = 3,
                Motivo = "Contagem física divergente do sistema"
            };

            var resultado = await service.RegistrarAjusteAsync(dto);

            Assert.Equal(3, produto.EstoqueAtual);
            Assert.Equal(5, resultado.Quantidade); // diferença absoluta: 8 -> 3 = 5
        }
    }
}
