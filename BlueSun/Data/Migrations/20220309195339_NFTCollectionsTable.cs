using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlueSun.Data.Migrations
{
    public partial class NFTCollectionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NFTCollectionId",
                table: "NFTs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "NFTCollections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFTCollections", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NFTs_NFTCollectionId",
                table: "NFTs",
                column: "NFTCollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_NFTs_NFTCollections_NFTCollectionId",
                table: "NFTs",
                column: "NFTCollectionId",
                principalTable: "NFTCollections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NFTs_NFTCollections_NFTCollectionId",
                table: "NFTs");

            migrationBuilder.DropTable(
                name: "NFTCollections");

            migrationBuilder.DropIndex(
                name: "IX_NFTs_NFTCollectionId",
                table: "NFTs");

            migrationBuilder.DropColumn(
                name: "NFTCollectionId",
                table: "NFTs");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
