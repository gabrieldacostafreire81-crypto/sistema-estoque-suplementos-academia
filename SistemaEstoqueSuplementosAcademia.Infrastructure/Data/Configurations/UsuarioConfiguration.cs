// Configurations/UsuarioConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Enums;

namespace SistemaEstoqueSuplementosAcademia.Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.Property(u => u.Nome).IsRequired().HasMaxLength(150);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasData(new Usuario
            {
                Id = 1,
                Nome = "Administrador",
                Email = "admin@sistemaestoque.com",
                SenhaHash = "$2b$11$QCfze8kaD6WvqkMZTsNyYexMDzdsrALOkdYWepCzu26ZnVLBTpcF.",
                Perfil = PerfilUsuario.Administrador,
                Ativo = true,
                DataCriacao = new DateTime(2026, 8, 22, 0, 0, 0, DateTimeKind.Utc)
            });
        }
    }
}