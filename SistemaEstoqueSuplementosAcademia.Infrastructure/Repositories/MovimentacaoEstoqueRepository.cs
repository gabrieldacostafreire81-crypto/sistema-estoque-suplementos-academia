// Infrastructure/Repositories/MovimentacaoEstoqueRepository.cs
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

        public async Task<IEnumerable<MovimentacaoEstoque>> ObterPorProdutoAsync(int produtoId)
        {
            return await _context.MovimentacoesEstoque
                .Include(m => m.Produto)
                .Include(m => m.Usuario)
                .Where(m => m.ProdutoId == produtoId)
                .OrderByDescending(m => m.DataHora)
                .ToListAsync();
        }
    }
}
