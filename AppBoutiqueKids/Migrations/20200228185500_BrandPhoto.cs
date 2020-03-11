using Microsoft.EntityFrameworkCore.Migrations;

namespace AppBoutiqueKids.Migrations
{
    public partial class BrandPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Brands",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Brands");
        }
    }
}
