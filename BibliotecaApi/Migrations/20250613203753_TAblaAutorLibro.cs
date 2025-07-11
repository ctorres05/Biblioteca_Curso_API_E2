﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaApi.Migrations
{
    /// <inheritdoc />
    public partial class TAblaAutorLibro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libros_Autores_autorId",
                table: "Libros");

            migrationBuilder.DropIndex(
                name: "IX_Libros_autorId",
                table: "Libros");

            migrationBuilder.DropColumn(
                name: "autorId",
                table: "Libros");

            migrationBuilder.CreateTable(
                name: "AutoresLibros",
                columns: table => new
                {
                    AutorId = table.Column<int>(type: "int", nullable: false),
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoresLibros", x => new { x.AutorId, x.LibroId });
                    table.ForeignKey(
                        name: "FK_AutoresLibros_Autores_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Autores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutoresLibros_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutoresLibros_LibroId",
                table: "AutoresLibros",
                column: "LibroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoresLibros");

            migrationBuilder.AddColumn<int>(
                name: "autorId",
                table: "Libros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Libros_autorId",
                table: "Libros",
                column: "autorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Libros_Autores_autorId",
                table: "Libros",
                column: "autorId",
                principalTable: "Autores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
