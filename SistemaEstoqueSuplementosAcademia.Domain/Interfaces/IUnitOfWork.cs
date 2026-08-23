// Domain/Interfaces/IUnitOfWork.cs
namespace SistemaEstoqueSuplementosAcademia.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task SalvarAsync();
    }
}
