using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlueSun.Data.Migrations
{
    public partial class AddedCategoryToNFT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "NFTs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NFTs_CategoryId",
                table: "NFTs",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_NFTs_Categories_CategoryId",
                table: "NFTs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NFTs_Categories_CategoryId",
                table: "NFTs");

            migrationBuilder.DropIndex(
                name: "IX_NFTs_CategoryId",
                table: "NFTs");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "NFTs");
        }
    }
}
