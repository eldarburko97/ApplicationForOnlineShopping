using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppBoutiqueKids.Migrations
{
    public partial class ModelsCartDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Details_Orders_OrderId",
                table: "Order_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Details_ProductSizes_ProductSizeId",
                table: "Order_Details");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order_Details",
                table: "Order_Details");

            migrationBuilder.DropColumn(
                name: "ProductImage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Order_Details");

            migrationBuilder.DropColumn(
                name: "DiscountPrice",
                table: "Order_Details");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Order_Details");

            migrationBuilder.RenameTable(
                name: "Order_Details",
                newName: "OrderDetails");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Details_ProductSizeId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Details_OrderId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                column: "Id");

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

            migrationBuilder.CreateTable(
                name: "CartDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CartId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ProductSizeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartDetails_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartDetails_ProductSizes_ProductSizeId",
                        column: x => x.ProductSizeId,
                        principalTable: "ProductSizes",
                        principalColumn: "ProductSizeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_CartId",
                table: "CartDetails",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_ProductSizeId",
                table: "CartDetails",
                column: "ProductSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ProductSizes_ProductSizeId",
                table: "OrderDetails",
                column: "ProductSizeId",
                principalTable: "ProductSizes",
                principalColumn: "ProductSizeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ProductSizes_ProductSizeId",
                table: "OrderDetails");

            migrationBuilder.DropTable(
                name: "CartDetails");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.RenameTable(
                name: "OrderDetails",
                newName: "Order_Details");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductSizeId",
                table: "Order_Details",
                newName: "IX_Order_Details_ProductSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_OrderId",
                table: "Order_Details",
                newName: "IX_Order_Details_OrderId");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProductImage",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubTotal",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "Order_Details",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DiscountPrice",
                table: "Order_Details",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Order_Details",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order_Details",
                table: "Order_Details",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Details_Orders_OrderId",
                table: "Order_Details",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Details_ProductSizes_ProductSizeId",
                table: "Order_Details",
                column: "ProductSizeId",
                principalTable: "ProductSizes",
                principalColumn: "ProductSizeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
