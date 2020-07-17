using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.API.Migrations
{
    public partial class AddLikeUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LikeUser",
                columns: table => new
                {
                    LikeOriginUserId = table.Column<int>(nullable: false),
                    LikeDestinyUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeUser", x => new { x.LikeOriginUserId, x.LikeDestinyUserId });
                    table.ForeignKey(
                        name: "FK_LikeUser_Users_LikeDestinyUserId",
                        column: x => x.LikeDestinyUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikeUser_Users_LikeOriginUserId",
                        column: x => x.LikeOriginUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LikeUser_LikeDestinyUserId",
                table: "LikeUser",
                column: "LikeDestinyUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikeUser");
        }
    }
}
