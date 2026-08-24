// Services/VendaRepositoryReportsTests.cs
using Microsoft.EntityFrameworkCore;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Infrastructure.Data;
using SistemaEstoqueSuplementosAcademia.Infrastructure.Repositories;
using Xunit;

namespace SistemaEstoqueSuplementosAcademia.Tests.Services
{
    public class VendaRepositoryReportsTests
    {
        private static AppDbContext CriarContextoComDados()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            var categoria = new Categoria { Id = 1, Nome = "Suplementos", Ativo = true };
            var marca = new Marca { Id = 1, Nome = "MarcaX", Ativo = true };
            var fornecedor = new Fornecedor { Id = 1, Nome = "FornX", Cnpj = "11.111.111/0001-11", Ativo = true };

            var produtoA = new Produto
            {
                Id = 1,
                Nome = "Whey",
                CategoriaId = 1,
                MarcaId = 1,
                FornecedorId = 1,
                PrecoCompra = 50,
                PrecoVenda = 90,
                EstoqueAtual = 100,
                EstoqueMinimo = 5,
                Ativo = true
            };
            var produtoB = new Produto
            {
                Id = 2,
                Nome = "Creatina",
                CategoriaId = 1,
                MarcaId = 1,
                FornecedorId = 1,
                PrecoCompra = 30,
                PrecoVenda = 60,
                EstoqueAtual = 100,
                EstoqueMinimo = 5,
                Ativo = true
            };

            var usuario = new Usuario
            {
                Id = 1,
                Nome = "Admin",
                Email = "admin@teste.com",
                SenhaHash = "hash",
                Perfil = Domain.Enums.PerfilUsuario.Administrador,
                Ativo = true
            };

            context.AddRange(categoria, marca, fornecedor, produtoA, produtoB, usuario);

            // Venda 1: 3x Whey + 2x Creatina
            var venda1 = new Venda { Id = 1, UsuarioId = 1, DataHora = new DateTime(2026, 1, 10), ValorTotal = 390, Status = "Concluida" };
            context.Add(venda1);
            context.Add(new ItemVenda { VendaId = 1, ProdutoId = 1, Quantidade = 3, PrecoUnitarioNaVenda = 90, Subtotal = 270 });
            context.Add(new ItemVenda { VendaId = 1, ProdutoId = 2, Quantidade = 2, PrecoUnitarioNaVenda = 60, Subtotal = 120 });

            // Venda 2: 5x Whey (fora do período que vamos testar)
            var venda2 = new Venda { Id = 2, UsuarioId = 1, DataHora = new DateTime(2026, 3, 15), ValorTotal = 450, Status = "Concluida" };
            context.Add(venda2);
            context.Add(new ItemVenda { VendaId = 2, ProdutoId = 1, Quantidade = 5, PrecoUnitarioNaVenda = 90, Subtotal = 450 });

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task ObterMaisVendidosAsync_SemFiltroDeData_DeveSomarTodasAsVendas()
        {
            var context = CriarContextoComDados();
            var repository = new VendaRepository(context);

            var resultado = (await repository.ObterMaisVendidosAsync(topN: 10, null, null)).ToList();

            Assert.Equal(2, resultado.Count);

            var whey = resultado.First(p => p.ProdutoNome == "Whey");
            Assert.Equal(8, whey.QuantidadeTotalVendida); // 3 + 5
            Assert.Equal(720, whey.FaturamentoTotal); // 270 + 450

            var creatina = resultado.First(p => p.ProdutoNome == "Creatina");
            Assert.Equal(2, creatina.QuantidadeTotalVendida);
        }

        [Fact]
        public async Task ObterMaisVendidosAsync_ComFiltroDePeriodo_DeveConsiderarSoAsVendasDoIntervalo()
        {
            var context = CriarContextoComDados();
            var repository = new VendaRepository(context);

            // Filtra só janeiro de 2026 — deve pegar só a Venda 1
            var resultado = (await repository.ObterMaisVendidosAsync(
                topN: 10, new DateTime(2026, 1, 1), new DateTime(2026, 1, 31))).ToList();

            var whey = resultado.First(p => p.ProdutoNome == "Whey");
            Assert.Equal(3, whey.QuantidadeTotalVendida); // só a Venda 1, não a 2
        }

        [Fact]
        public async Task ObterMaisVendidosAsync_DeveOrdenarPorQuantidadeDecrescente()
        {
            var context = CriarContextoComDados();
            var repository = new VendaRepository(context);

            var resultado = (await repository.ObterMaisVendidosAsync(topN: 10, null, null)).ToList();

            Assert.Equal("Whey", resultado[0].ProdutoNome); // 8 unidades, o mais vendido
            Assert.Equal("Creatina", resultado[1].ProdutoNome); // 2 unidades
        }

        [Fact]
        public async Task ObterMaisVendidosAsync_ComTopNMenorQueQuantidadeDeProdutos_DeveLimitarResultado()
        {
            var context = CriarContextoComDados();
            var repository = new VendaRepository(context);

            var resultado = (await repository.ObterMaisVendidosAsync(topN: 1, null, null)).ToList();

            Assert.Single(resultado);
            Assert.Equal("Whey", resultado[0].ProdutoNome);
        }
    }
}