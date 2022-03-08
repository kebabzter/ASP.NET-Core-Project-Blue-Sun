using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlueSun.Data.Migrations
{
    public partial class OnlyCategoryAndNFTTablesLeft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NFTs_NFTCollections_NFTCollectionId",
                table: "NFTs");

            migrationBuilder.DropTable(
                name: "NFTCollections");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropIndex(
                name: "IX_NFTs_NFTCollectionId",
                table: "NFTs");

            migrationBuilder.DropColumn(
                name: "NFTCollectionId",
                table: "NFTs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NFTCollectionId",
                table: "NFTs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFTCollections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtistId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFTCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NFTCollections_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NFTs_NFTCollectionId",
                table: "NFTs",
                column: "NFTCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_NFTCollections_ArtistId",
                table: "NFTCollections",
                column: "ArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_NFTs_NFTCollections_NFTCollectionId",
                table: "NFTs",
                column: "NFTCollectionId",
                principalTable: "NFTCollections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
