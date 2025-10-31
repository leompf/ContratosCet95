using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContratosCet95.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityTipoContrato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Conditions",
                table: "Contratos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Contratos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TiposContratos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposContratos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_TypeId",
                table: "Contratos",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contratos_TiposContratos_TypeId",
                table: "Contratos",
                column: "TypeId",
                principalTable: "TiposContratos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contratos_TiposContratos_TypeId",
                table: "Contratos");

            migrationBuilder.DropTable(
                name: "TiposContratos");

            migrationBuilder.DropIndex(
                name: "IX_Contratos_TypeId",
                table: "Contratos");

            migrationBuilder.DropColumn(
                name: "Conditions",
                table: "Contratos");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Contratos");
        }
    }
}
