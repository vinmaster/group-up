using Microsoft.EntityFrameworkCore.Migrations;

namespace GroupUp.Data.Migrations
{
    public partial class ChangeModifiedAtName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "AspNetUsers",
                newName: "UpdatedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "AspNetUsers",
                newName: "ModifiedAt");
        }
    }
}
