using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApiRelogios.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Relogios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Marca = table.Column<string>(type: "TEXT", nullable: false),
                    Modelo = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Preco = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relogios", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Relogios",
                columns: new[] { "Id", "Marca", "Modelo", "Preco", "Tipo" },
                values: new object[,]
                {
                    { 1, "Casio", "F-91W", 149.90000000000001, "Digital" },
                    { 2, "Seiko", "Presage SRPB43", 3499.0, "Automático" },
                    { 3, "Apple", "Watch Series 10", 4299.0, "Smartwatch" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Relogios");
        }
    }
}
