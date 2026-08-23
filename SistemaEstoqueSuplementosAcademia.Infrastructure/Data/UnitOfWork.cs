// Infrastructure/Data/UnitOfWork.cs
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task SalvarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}