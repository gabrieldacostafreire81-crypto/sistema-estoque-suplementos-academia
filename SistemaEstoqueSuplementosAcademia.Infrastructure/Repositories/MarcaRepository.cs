// Infrastructure/Repositories/MarcaRepository.cs
using Microsoft.EntityFrameworkCore;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;
using SistemaEstoqueSuplementosAcademia.Infrastructure.Data;

namespace SistemaEstoqueSuplementosAcademia.Infrastructure.Repositories
{
    public class MarcaRepository : IMarcaRepository
    {
        private readonly AppDbContext _context;

        public MarcaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Marca?> ObterPorIdAsync(int id)
        {
            return await _context.Marcas.FindAsync(id);
        }

        public async Task<IEnumerable<Marca>> ObterTodasAsync()
        {
            return await _context.Marcas.OrderBy(m => m.Nome).ToListAsync();
        }

        public async Task<bool> ExisteComNomeAsync(string nome, int? idParaIgnorar = null)
        {
            return await _context.Marcas.AnyAsync(m => m.Nome == nome && m.Id != idParaIgnorar);
        }

        public async Task AdicionarAsync(Marca marca)
        {
            await _context.Marcas.AddAsync(marca);
        }

        public void Atualizar(Marca marca)
        {
            _context.Marcas.Update(marca);
        }

        public async Task SalvarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}