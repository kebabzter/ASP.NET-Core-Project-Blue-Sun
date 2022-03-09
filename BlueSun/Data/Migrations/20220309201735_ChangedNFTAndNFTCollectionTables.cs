using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlueSun.Data.Migrations
{
    public partial class ChangedNFTAndNFTCollectionTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "NFTCollections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NFTCollections_CategoryId",
                table: "NFTCollections",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_NFTCollections_Categories_CategoryId",
                table: "NFTCollections",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NFTCollections_Categories_CategoryId",
                table: "NFTCollections");

            migrationBuilder.DropIndex(
                name: "IX_NFTCollections_CategoryId",
                table: "NFTCollections");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "NFTCollections");

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
    }
}
