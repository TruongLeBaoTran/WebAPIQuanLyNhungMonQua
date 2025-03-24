using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_GiftManagement_BaoTran.Migrations
{
    public partial class DbU : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    IdCategory = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.IdCategory);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    IdPermission = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.IdPermission);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRole = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRole);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Coin = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gifts",
                columns: table => new
                {
                    IdGift = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Coin = table.Column<int>(type: "int", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RemainingQuantity = table.Column<int>(type: "int", nullable: false),
                    IdCategory = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gifts", x => x.IdGift);
                    table.ForeignKey(
                        name: "FK_Gifts_Categories_IdCategory",
                        column: x => x.IdCategory,
                        principalTable: "Categories",
                        principalColumn: "IdCategory",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    IdRole = table.Column<int>(type: "int", nullable: false),
                    IdPermission = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.IdRole, x.IdPermission });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_IdPermission",
                        column: x => x.IdPermission,
                        principalTable: "Permissions",
                        principalColumn: "IdPermission",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_IdRole",
                        column: x => x.IdRole,
                        principalTable: "Roles",
                        principalColumn: "IdRole",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUsers",
                columns: table => new
                {
                    IdRole = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUsers", x => new { x.IdRole, x.IdUser });
                    table.ForeignKey(
                        name: "FK_RoleUsers_Roles_IdRole",
                        column: x => x.IdRole,
                        principalTable: "Roles",
                        principalColumn: "IdRole",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUsers_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    IdTransaction = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    TransactionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CoinTotal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.IdTransaction);
                    table.ForeignKey(
                        name: "FK_Transactions_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    IdCart = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMainGift = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    TotalCoin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.IdCart);
                    table.ForeignKey(
                        name: "FK_Carts_Gifts_IdMainGift",
                        column: x => x.IdMainGift,
                        principalTable: "Gifts",
                        principalColumn: "IdGift",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Carts_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    IdPromotion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMainGift = table.Column<int>(type: "int", nullable: false),
                    IdPromoGift = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.IdPromotion);
                    table.ForeignKey(
                        name: "FK_Promotions_Gifts_IdMainGift",
                        column: x => x.IdMainGift,
                        principalTable: "Gifts",
                        principalColumn: "IdGift",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Promotions_Gifts_IdPromoGift",
                        column: x => x.IdPromoGift,
                        principalTable: "Gifts",
                        principalColumn: "IdGift",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTransaction = table.Column<int>(type: "int", nullable: false),
                    IdGift = table.Column<int>(type: "int", nullable: false),
                    MainGift = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionDetails_Gifts_IdGift",
                        column: x => x.IdGift,
                        principalTable: "Gifts",
                        principalColumn: "IdGift",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionDetails_Transactions_IdTransaction",
                        column: x => x.IdTransaction,
                        principalTable: "Transactions",
                        principalColumn: "IdTransaction",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "IdPermission", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "CreateAdmin", "CreateAdmin" },
                    { 2, "ViewCategory", "ViewCategory" },
                    { 3, "ViewOneCategory", "ViewOneCategory" },
                    { 4, "CreateCategory", "CreateCategory" },
                    { 5, "DeleteCategory", "DeleteCategory" },
                    { 6, "UpdateCategory", "UpdateCategory" },
                    { 7, "ViewGift", "ViewGift" },
                    { 8, "ViewOneGift", "ViewOneGift" },
                    { 9, "CreateGift", "CreateGift" },
                    { 10, "DeleteGift", "DeleteGift" },
                    { 11, "UpdateGift", "UpdateGift" },
                    { 12, "AddPromoGiftToGift", "AddPromoGiftToGift" },
                    { 13, "ViewPromoGiftToGift", "ViewPromoGiftToGift" },
                    { 14, "UpdateQuantityPromoGiftInGift", "UpdateQuantityPromoGiftInGift" },
                    { 15, "DeletePromoGiftInGift", "DeletePromoGiftInGift" },
                    { 16, "GivenGift", "GivenGift" },
                    { 17, "CancelScheduledGiftDistribution", "CancelScheduledGiftDistribution" },
                    { 18, "GiveCoins", "GiveCoins" },
                    { 19, "ViewUser", "ViewUser" },
                    { 20, "ViewRole", "ViewRole" },
                    { 21, "ViewOneRole", "ViewOneRole" },
                    { 22, "CreateRole", "CreateRole" },
                    { 23, "DeleteRole", "DeleteRole" },
                    { 24, "UpdateRole", "UpdateRole" },
                    { 25, "AddListPermissionToRole", "AddListPermissionToRole" },
                    { 26, "GetPermissionInRole", "GetPermissionInRole" },
                    { 27, "AddListUserToRole", "AddListUserToRole" },
                    { 28, "ViewUserInRole", "ViewUserInRole" },
                    { 29, "ViewAllPermissions", "ViewAllPermissions" },
                    { 30, "ViewOnePermission", "ViewOnePermission" },
                    { 31, "CreatePermission", "CreatePermission" },
                    { 32, "DeletePermission", "DeletePermission" },
                    { 33, "UpdatePermission", "UpdatePermission" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "IdRole", "Code", "Name" },
                values: new object[] { 1, "sa", "Admin" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Birthday", "Email", "Image", "Password", "Phone", "Username" },
                values: new object[] { 1, null, "Admin123@gmail.com", null, "$2a$11$vkniw5Onv0KwhObkOMyF5uyzl.B1UXNtUMfaRdq8Zvi1CSF83ajRO", "0123456789", "Admin123@" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "IdPermission", "IdRole" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 6, 1 },
                    { 7, 1 },
                    { 8, 1 },
                    { 9, 1 },
                    { 10, 1 },
                    { 11, 1 },
                    { 12, 1 },
                    { 13, 1 },
                    { 14, 1 },
                    { 15, 1 },
                    { 16, 1 },
                    { 17, 1 },
                    { 18, 1 },
                    { 19, 1 },
                    { 20, 1 },
                    { 21, 1 },
                    { 22, 1 },
                    { 23, 1 },
                    { 24, 1 },
                    { 25, 1 },
                    { 26, 1 },
                    { 27, 1 },
                    { 28, 1 },
                    { 29, 1 },
                    { 30, 1 },
                    { 31, 1 },
                    { 32, 1 },
                    { 33, 1 }
                });

            migrationBuilder.InsertData(
                table: "RoleUsers",
                columns: new[] { "IdRole", "IdUser" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_IdMainGift",
                table: "Carts",
                column: "IdMainGift");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_IdUser",
                table: "Carts",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_IdCategory",
                table: "Gifts",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_IdMainGift_IdPromoGift",
                table: "Promotions",
                columns: new[] { "IdMainGift", "IdPromoGift" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_IdPromoGift",
                table: "Promotions",
                column: "IdPromoGift");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_IdUser",
                table: "RefreshTokens",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_IdPermission",
                table: "RolePermissions",
                column: "IdPermission");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUsers_IdUser",
                table: "RoleUsers",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_IdGift",
                table: "TransactionDetails",
                column: "IdGift");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_IdTransaction_IdGift_MainGift",
                table: "TransactionDetails",
                columns: new[] { "IdTransaction", "IdGift", "MainGift" },
                unique: true,
                filter: "[MainGift] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_IdUser",
                table: "Transactions",
                column: "IdUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "RoleUsers");

            migrationBuilder.DropTable(
                name: "TransactionDetails");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Gifts");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
