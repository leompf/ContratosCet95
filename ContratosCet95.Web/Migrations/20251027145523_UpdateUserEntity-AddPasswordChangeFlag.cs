using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContratosCet95.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserEntityAddPasswordChangeFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChangePassword",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChangePassword",
                table: "AspNetUsers");
        }
    }
}
