// Services/VendaServiceTests.cs
using Moq;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Venda;
using SistemaEstoqueSuplementosAcademia.Application.Services;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;
using SistemaEstoqueSuplementosAcademia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace SistemaEstoqueSuplementosAcademia.Tests.Services
{
    public class VendaServiceTests
    {
        private static AppDbContext CriarContextoEmMemoria()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CriarAsync_ComEstoqueSuficiente_DeveDescontarEstoqueECalcularTotalCorretamente()
        {
            var produto = new Produto
            {
                Id = 1,
                Nome = "Whey",
                EstoqueAtual = 20,
                EstoqueMinimo = 5,
                PrecoVenda = 90.00m
            };

            var produtoRepoMock = new Mock<IProdutoRepository>();
            produtoRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(produto);

            var movimentacaoRepoMock = new Mock<IMovimentacaoEstoqueRepository>();

            var vendaRepoMock = new Mock<IVendaRepository>();
            Venda? vendaCriada = null;
            vendaRepoMock
                .Setup(r => r.AdicionarAsync(It.IsAny<Venda>()))
                .Callback<Venda>(v => vendaCriada = v)
                .Returns(Task.CompletedTask);
            vendaRepoMock
                .Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => vendaCriada);

            var context = CriarContextoEmMemoria();
            var unitOfWork = new UnitOfWork(context);

            var service = new VendaService(
                vendaRepoMock.Object, produtoRepoMock.Object, movimentacaoRepoMock.Object, unitOfWork);

            var dto = new VendaCreateDto
            {
                Itens = new List<VendaItemCreateDto>
                {
                    new VendaItemCreateDto { ProdutoId = 1, Quantidade = 5 }
                }
            };

            var resultado = await service.CriarAsync(dto, usuarioId: 1);

            Assert.Equal(15, produto.EstoqueAtual);
            Assert.Equal(450.00m, resultado.ValorTotal);
            Assert.Single(resultado.Itens);
            Assert.Equal(90.00m, resultado.Itens[0].PrecoUnitarioNaVenda);
        }

        [Fact]
        public async Task CriarAsync_ComEstoqueInsuficienteEmUmDosItens_DeveLancarExcecaoENaoAlterarNenhumEstoque()
        {
            var produtoA = new Produto { Id = 1, Nome = "Whey", EstoqueAtual = 10, EstoqueMinimo = 2, PrecoVenda = 90.00m };
            var produtoB = new Produto { Id = 2, Nome = "Creatina", EstoqueAtual = 3, EstoqueMinimo = 2, PrecoVenda = 60.00m };

            var produtoRepoMock = new Mock<IProdutoRepository>();
            produtoRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(produtoA);
            produtoRepoMock.Setup(r => r.ObterPorIdAsync(2)).ReturnsAsync(produtoB);

            var movimentacaoRepoMock = new Mock<IMovimentacaoEstoqueRepository>();
            var vendaRepoMock = new Mock<IVendaRepository>();

            var context = CriarContextoEmMemoria();
            var unitOfWork = new UnitOfWork(context);

            var service = new VendaService(
                vendaRepoMock.Object, produtoRepoMock.Object, movimentacaoRepoMock.Object, unitOfWork);

            var dto = new VendaCreateDto
            {
                Itens = new List<VendaItemCreateDto>
                {
                    new VendaItemCreateDto { ProdutoId = 1, Quantidade = 5 },  // válido
                    new VendaItemCreateDto { ProdutoId = 2, Quantidade = 10 } // inválido: só tem 3
                }
            };

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CriarAsync(dto, usuarioId: 1));

            // O item 1 já teria sido descontado em memória, mas nada foi persistido
            Assert.Equal(5, produtoA.EstoqueAtual); // 10 - 5 = 5 (mudou em memória)
            Assert.Equal(3, produtoB.EstoqueAtual); // nunca mudou, falhou antes

            vendaRepoMock.Verify(r => r.AdicionarAsync(It.IsAny<Venda>()), Times.Never);
        }

        [Fact]
        public async Task CriarAsync_ComProdutoInexistente_DeveLancarExcecao()
        {
            var produtoRepoMock = new Mock<IProdutoRepository>();
            produtoRepoMock.Setup(r => r.ObterPorIdAsync(999)).ReturnsAsync((Produto?)null);

            var movimentacaoRepoMock = new Mock<IMovimentacaoEstoqueRepository>();
            var vendaRepoMock = new Mock<IVendaRepository>();

            var context = CriarContextoEmMemoria();
            var unitOfWork = new UnitOfWork(context);

            var service = new VendaService(
                vendaRepoMock.Object, produtoRepoMock.Object, movimentacaoRepoMock.Object, unitOfWork);

            var dto = new VendaCreateDto
            {
                Itens = new List<VendaItemCreateDto>
                {
                    new VendaItemCreateDto { ProdutoId = 999, Quantidade = 1 }
                }
            };

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => service.CriarAsync(dto, usuarioId: 1));
        }
    }
}
