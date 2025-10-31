using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContratosCet95.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixContratoEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contratos_TiposContratos_TypeId",
                table: "Contratos");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Contratos");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Contratos");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Contratos",
                newName: "TipoContratoId");

            migrationBuilder.RenameIndex(
                name: "IX_Contratos_TypeId",
                table: "Contratos",
                newName: "IX_Contratos_TipoContratoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contratos_TiposContratos_TipoContratoId",
                table: "Contratos",
                column: "TipoContratoId",
                principalTable: "TiposContratos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contratos_TiposContratos_TipoContratoId",
                table: "Contratos");

            migrationBuilder.RenameColumn(
                name: "TipoContratoId",
                table: "Contratos",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Contratos_TipoContratoId",
                table: "Contratos",
                newName: "IX_Contratos_TypeId");

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "Contratos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Contratos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Contratos_TiposContratos_TypeId",
                table: "Contratos",
                column: "TypeId",
                principalTable: "TiposContratos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
