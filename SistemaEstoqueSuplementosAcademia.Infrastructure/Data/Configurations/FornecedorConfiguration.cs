// Configurations/FornecedorConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;

namespace SistemaEstoqueSuplementosAcademia.Infrastructure.Data.Configurations
{
    public class FornecedorConfiguration : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.Property(f => f.Nome).IsRequired().HasMaxLength(150);
            builder.Property(f => f.Cnpj).IsRequired().HasMaxLength(18);
            builder.HasIndex(f => f.Cnpj).IsUnique();
        }
    }
}