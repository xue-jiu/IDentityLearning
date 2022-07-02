using Microsoft.EntityFrameworkCore.Migrations;

namespace IDentityLearning.Migrations
{
    public partial class AddPreference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MyPreference",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyPreference",
                table: "AspNetUsers");
        }
    }
}
