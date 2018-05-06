using Microsoft.EntityFrameworkCore.Migrations;

namespace ManipulaImagem.Migrations
{
    public partial class CriacaoBanco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Manipulacao",
                columns: table => new
                {
                    ManipulacaoId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manipulacao", x => x.ManipulacaoId);
                });

            migrationBuilder.CreateTable(
                name: "Acao",
                columns: table => new
                {
                    ManipulacaoId = table.Column<int>(nullable: false),
                    Ordem = table.Column<int>(nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    Percentagem = table.Column<int>(nullable: true),
                    Angulo = table.Column<int>(nullable: true),
                    X = table.Column<int>(nullable: true),
                    Y = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acao", x => new { x.ManipulacaoId, x.Ordem });
                    table.ForeignKey(
                        name: "FK_Acao_Manipulacao_ManipulacaoId",
                        column: x => x.ManipulacaoId,
                        principalTable: "Manipulacao",
                        principalColumn: "ManipulacaoId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Acao");

            migrationBuilder.DropTable(
                name: "Manipulacao");
        }
    }
}
