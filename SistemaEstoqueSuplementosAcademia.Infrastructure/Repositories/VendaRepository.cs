// Infrastructure/Repositories/VendaRepository.cs
using Microsoft.EntityFrameworkCore;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;
using SistemaEstoqueSuplementosAcademia.Domain.Models;
using SistemaEstoqueSuplementosAcademia.Infrastructure.Data;

namespace SistemaEstoqueSuplementosAcademia.Infrastructure.Repositories
{
    public class VendaRepository : IVendaRepository
    {
        private readonly AppDbContext _context;

        public VendaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Venda venda)
        {
            await _context.Vendas.AddAsync(venda);
        }

        public async Task<Venda?> ObterPorIdAsync(int id)
        {
            return await _context.Vendas
                .Include(v => v.Usuario)
                .Include(v => v.Itens)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Venda>> ObterTodasAsync()
        {
            return await _context.Vendas
                .Include(v => v.Usuario)
                .Include(v => v.Itens)
                    .ThenInclude(i => i.Produto)
                .OrderByDescending(v => v.DataHora)
                .ToListAsync();
        }

        public async Task<IEnumerable<Venda>> ObterPorPeriodoAsync(DateTime? dataInicial, DateTime? dataFinal)
        {
            var query = _context.Vendas
                .Include(v => v.Usuario)
                .Include(v => v.Itens)
                    .ThenInclude(i => i.Produto)
                .AsQueryable();

            if (dataInicial.HasValue)
                query = query.Where(v => v.DataHora >= dataInicial);

            if (dataFinal.HasValue)
                query = query.Where(v => v.DataHora <= dataFinal);

            return await query
                .OrderByDescending(v => v.DataHora)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProdutoMaisVendido>> ObterMaisVendidosAsync(
            int topN, DateTime? dataInicial, DateTime? dataFinal)
        {
            var query = _context.ItensVenda
                .Include(i => i.Venda)
                .Include(i => i.Produto)
                .AsQueryable();

            if (dataInicial.HasValue)
                query = query.Where(i => i.Venda.DataHora >= dataInicial);

            if (dataFinal.HasValue)
                query = query.Where(i => i.Venda.DataHora <= dataFinal);

            return await query
                .GroupBy(i => new { i.ProdutoId, i.Produto.Nome })
                .Select(g => new ProdutoMaisVendido
                {
                    ProdutoId = g.Key.ProdutoId,
                    ProdutoNome = g.Key.Nome,
                    QuantidadeTotalVendida = g.Sum(i => i.Quantidade),
                    FaturamentoTotal = g.Sum(i => i.Subtotal)
                })
                .OrderByDescending(p => p.QuantidadeTotalVendida)
                .Take(topN)
                .ToListAsync();
        }
    }
}