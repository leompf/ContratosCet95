using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContratosCet95.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTipoContratoAddDurationMonths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationMonths",
                table: "TiposContratos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMonths",
                table: "TiposContratos");
        }
    }
}
