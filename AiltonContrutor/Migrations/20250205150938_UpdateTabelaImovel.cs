using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CasaFacilEPS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTabelaImovel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "Imovel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MetragemImovel",
                table: "Imovel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "Imovel");

            migrationBuilder.DropColumn(
                name: "MetragemImovel",
                table: "Imovel");
        }
    }
}
