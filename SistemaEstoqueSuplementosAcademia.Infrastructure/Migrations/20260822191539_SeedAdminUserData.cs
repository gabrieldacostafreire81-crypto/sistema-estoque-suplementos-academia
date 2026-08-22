using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEstoqueSuplementosAcademia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUserData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Ativo", "DataCriacao", "Email", "Nome", "Perfil", "SenhaHash" },
                values: new object[] { 1, true, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), "admin@sistemaestoque.com", "Administrador", 1, "$2b$11$QCfze8kaD6WvqkMZTsNyYexMDzdsrALOkdYWepCzu26ZnVLBTpcF." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
