using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaApi.Migrations
{
    /// <inheritdoc />
    public partial class TablaLibrosCorrigeCAmpos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libros_Autores_AutorId",
                table: "Libros");

            migrationBuilder.RenameColumn(
                name: "Titulo",
                table: "Libros",
                newName: "titulo");

            migrationBuilder.RenameColumn(
                name: "AutorId",
                table: "Libros",
                newName: "autorId");

            migrationBuilder.RenameIndex(
                name: "IX_Libros_AutorId",
                table: "Libros",
                newName: "IX_Libros_autorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Libros_Autores_autorId",
                table: "Libros",
                column: "autorId",
                principalTable: "Autores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libros_Autores_autorId",
                table: "Libros");

            migrationBuilder.RenameColumn(
                name: "titulo",
                table: "Libros",
                newName: "Titulo");

            migrationBuilder.RenameColumn(
                name: "autorId",
                table: "Libros",
                newName: "AutorId");

            migrationBuilder.RenameIndex(
                name: "IX_Libros_autorId",
                table: "Libros",
                newName: "IX_Libros_AutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Libros_Autores_AutorId",
                table: "Libros",
                column: "AutorId",
                principalTable: "Autores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
