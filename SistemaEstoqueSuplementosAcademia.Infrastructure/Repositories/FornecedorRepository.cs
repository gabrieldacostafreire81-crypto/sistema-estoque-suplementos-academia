// Infrastructure/Repositories/FornecedorRepository.cs
using Microsoft.EntityFrameworkCore;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;
using SistemaEstoqueSuplementosAcademia.Infrastructure.Data;

namespace SistemaEstoqueSuplementosAcademia.Infrastructure.Repositories
{
    public class FornecedorRepository : IFornecedorRepository
    {
        private readonly AppDbContext _context;

        public FornecedorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Fornecedor?> ObterPorIdAsync(int id)
        {
            return await _context.Fornecedores.FindAsync(id);
        }

        public async Task<(IEnumerable<Fornecedor> Itens, int TotalRegistros)> ObterPaginadoAsync(
            string? nome, bool? ativo, int pagina, int tamanhoPagina)
        {
            var query = _context.Fornecedores.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
                query = query.Where(f => f.Nome.Contains(nome));

            if (ativo.HasValue)
                query = query.Where(f => f.Ativo == ativo);

            var total = await query.CountAsync();

            var itens = await query
                .OrderBy(f => f.Nome)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return (itens, total);
        }

        public async Task<bool> ExisteComCnpjAsync(string cnpj, int? idParaIgnorar = null)
        {
            return await _context.Fornecedores.AnyAsync(f => f.Cnpj == cnpj && f.Id != idParaIgnorar);
        }

        public async Task AdicionarAsync(Fornecedor fornecedor)
        {
            await _context.Fornecedores.AddAsync(fornecedor);
        }

        public void Atualizar(Fornecedor fornecedor)
        {
            _context.Fornecedores.Update(fornecedor);
        }

        public async Task SalvarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
