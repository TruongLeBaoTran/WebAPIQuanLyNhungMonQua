using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_GiftManagement_BaoTran.Migrations
{
    public partial class DbUpdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RankingUser",
                table: "RankingUser");

            migrationBuilder.AddColumn<int>(
                name: "AccumulatedPoints",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RankingUser",
                table: "RankingUser",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$fE02LSrEhVuR8lXQYNqx8euzKymPfOfGX4Mu6NwmKARJaARDvYLNu");

            migrationBuilder.CreateIndex(
                name: "IX_RankingUser_IdRanking",
                table: "RankingUser",
                column: "IdRanking");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RankingUser",
                table: "RankingUser");

            migrationBuilder.DropIndex(
                name: "IX_RankingUser_IdRanking",
                table: "RankingUser");

            migrationBuilder.DropColumn(
                name: "AccumulatedPoints",
                table: "Transactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RankingUser",
                table: "RankingUser",
                columns: new[] { "IdRanking", "IdUser" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$NcRaPQH2ae6bF6rLbX8oUesLl4QyGgVtGWRXOTygr5Yy0WXZ.mPgy");
        }
    }
}
