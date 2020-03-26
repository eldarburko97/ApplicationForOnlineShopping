using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppBoutiqueKids.Migrations
{
    public partial class RemoveCartTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_Carts_CartId",
                table: "CartDetails");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "CartDetails",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CartDetails_CartId",
                table: "CartDetails",
                newName: "IX_CartDetails_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_AspNetUsers_UserId",
                table: "CartDetails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_AspNetUsers_UserId",
                table: "CartDetails");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CartDetails",
                newName: "CartId");

            migrationBuilder.RenameIndex(
                name: "IX_CartDetails_UserId",
                table: "CartDetails",
                newName: "IX_CartDetails_CartId");

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_Carts_CartId",
                table: "CartDetails",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
