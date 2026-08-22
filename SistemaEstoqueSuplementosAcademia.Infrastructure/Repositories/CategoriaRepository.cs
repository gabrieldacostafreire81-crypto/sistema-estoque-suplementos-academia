using Microsoft.EntityFrameworkCore;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;
using SistemaEstoqueSuplementosAcademia.Infrastructure.Data;

namespace SistemaEstoqueSuplementosAcademia.Infrastructure.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Categoria?> ObterPorIdAsync(int id)
        {
            return await _context.Categorias.FindAsync(id);
        }

        public async Task<IEnumerable<Categoria>> ObterTodasAsync()
        {
            return await _context.Categorias
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<bool> ExisteComNomeAsync(string nome, int? idParaIgnorar = null)
        {
            return await _context.Categorias
                .AnyAsync(c => c.Nome == nome && c.Id != idParaIgnorar);
        }

        public async Task AdicionarAsync(Categoria categoria)
        {
            await _context.Categorias.AddAsync(categoria);
        }

        public void Atualizar(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
        }

        public async Task SalvarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}