using Microsoft.EntityFrameworkCore;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;
using SistemaEstoqueSuplementosAcademia.Infrastructure.Data;

namespace SistemaEstoqueSuplementosAcademia.Infrastructure.Repositories
{
    public class MovimentacaoEstoqueRepository : IMovimentacaoEstoqueRepository
    {
        private readonly AppDbContext _context;

        public MovimentacaoEstoqueRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(MovimentacaoEstoque movimentacao)
        {
            await _context.MovimentacoesEstoque.AddAsync(movimentacao);
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> ObterPorFiltroAsync(
            int? produtoId, DateTime? dataInicial, DateTime? dataFinal)
        {
            var query = _context.MovimentacoesEstoque
                .Include(m => m.Produto)
                .Include(m => m.Usuario)
                .AsQueryable();

            if (produtoId.HasValue)
                query = query.Where(m => m.ProdutoId == produtoId);

            if (dataInicial.HasValue)
                query = query.Where(m => m.DataHora >= dataInicial);

            if (dataFinal.HasValue)
                query = query.Where(m => m.DataHora <= dataFinal);

            return await query
                .OrderByDescending(m => m.DataHora)
                .ToListAsync();
        }
    }
}