using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlueSun.Data.Migrations
{
    public partial class UserHasNFTs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "NFTs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_NFTs_OwnerId",
                table: "NFTs",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_NFTs_AspNetUsers_OwnerId",
                table: "NFTs",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NFTs_AspNetUsers_OwnerId",
                table: "NFTs");

            migrationBuilder.DropIndex(
                name: "IX_NFTs_OwnerId",
                table: "NFTs");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "NFTs");
        }
    }
}
