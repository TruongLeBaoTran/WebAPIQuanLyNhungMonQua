using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_GiftManagement_BaoTran.Migrations
{
    public partial class DbUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ranking",
                columns: table => new
                {
                    IdRanking = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranking", x => x.IdRanking);
                });

            migrationBuilder.CreateTable(
                name: "RankingUser",
                columns: table => new
                {
                    IdRanking = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Point = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankingUser", x => new { x.IdRanking, x.IdUser });
                    table.ForeignKey(
                        name: "FK_RankingUser_Ranking_IdRanking",
                        column: x => x.IdRanking,
                        principalTable: "Ranking",
                        principalColumn: "IdRanking",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RankingUser_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$NcRaPQH2ae6bF6rLbX8oUesLl4QyGgVtGWRXOTygr5Yy0WXZ.mPgy");

            migrationBuilder.CreateIndex(
                name: "IX_RankingUser_IdUser",
                table: "RankingUser",
                column: "IdUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RankingUser");

            migrationBuilder.DropTable(
                name: "Ranking");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$vkniw5Onv0KwhObkOMyF5uyzl.B1UXNtUMfaRdq8Zvi1CSF83ajRO");
        }
    }
}
