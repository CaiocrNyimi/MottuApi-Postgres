using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MottuApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgresMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "SEQ_MOTO");

            migrationBuilder.CreateSequence<int>(
                name: "SEQ_MOVIMENTACAO");

            migrationBuilder.CreateSequence<int>(
                name: "SEQ_PATIO");

            migrationBuilder.CreateSequence<int>(
                name: "SEQ_USUARIO");

            migrationBuilder.CreateTable(
                name: "Patios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"SEQ_PATIO\"')"),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Localizacao = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"SEQ_USUARIO\"')"),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SenhaHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"SEQ_MOTO\"')"),
                    Placa = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    Modelo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PatioId = table.Column<int>(type: "integer", nullable: true),
                    DataEntrada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Motos_Patios_PatioId",
                        column: x => x.PatioId,
                        principalTable: "Patios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Movimentacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"SEQ_MOVIMENTACAO\"')"),
                    MotoId = table.Column<int>(type: "integer", nullable: false),
                    PatioId = table.Column<int>(type: "integer", nullable: false),
                    DataEntrada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataSaida = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimentacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movimentacoes_Motos_MotoId",
                        column: x => x.MotoId,
                        principalTable: "Motos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movimentacoes_Patios_PatioId",
                        column: x => x.PatioId,
                        principalTable: "Patios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Motos_PatioId",
                table: "Motos",
                column: "PatioId");

            migrationBuilder.CreateIndex(
                name: "IX_Motos_Placa",
                table: "Motos",
                column: "Placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_MotoId",
                table: "Movimentacoes",
                column: "MotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_PatioId",
                table: "Movimentacoes",
                column: "PatioId");

            migrationBuilder.CreateIndex(
                name: "IX_Patios_Localizacao",
                table: "Patios",
                column: "Localizacao",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patios_Nome",
                table: "Patios",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Username",
                table: "Usuarios",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movimentacoes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Motos");

            migrationBuilder.DropTable(
                name: "Patios");

            migrationBuilder.DropSequence(
                name: "SEQ_MOTO");

            migrationBuilder.DropSequence(
                name: "SEQ_MOVIMENTACAO");

            migrationBuilder.DropSequence(
                name: "SEQ_PATIO");

            migrationBuilder.DropSequence(
                name: "SEQ_USUARIO");
        }
    }
}
