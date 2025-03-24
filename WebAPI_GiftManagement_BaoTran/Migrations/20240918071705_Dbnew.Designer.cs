﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebAPI_GiftManagement_BaoTran.Data;

#nullable disable

namespace WebAPI_GiftManagement_BaoTran.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20240918071705_Dbnew")]
    partial class Dbnew
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Cart", b =>
                {
                    b.Property<int>("IdCart")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCart"), 1L, 1);

                    b.Property<int>("IdMainGift")
                        .HasColumnType("int");

                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("TotalCoin")
                        .HasColumnType("int");

                    b.HasKey("IdCart");

                    b.HasIndex("IdMainGift");

                    b.HasIndex("IdUser");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Category", b =>
                {
                    b.Property<int>("IdCategory")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCategory"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdCategory");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Gift", b =>
                {
                    b.Property<int>("IdGift")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdGift"), 1L, 1);

                    b.Property<int?>("AccumulatedPoints")
                        .HasColumnType("int");

                    b.Property<int?>("Coin")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdCategory")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RemainingQuantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdGift");

                    b.HasIndex("IdCategory");

                    b.ToTable("Gifts");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Permission", b =>
                {
                    b.Property<int>("IdPermission")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPermission"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdPermission");

                    b.ToTable("Permissions");

                    b.HasData(
                        new
                        {
                            IdPermission = 1,
                            Code = "CreateAdmin",
                            Name = "CreateAdmin"
                        },
                        new
                        {
                            IdPermission = 2,
                            Code = "ViewCategory",
                            Name = "ViewCategory"
                        },
                        new
                        {
                            IdPermission = 3,
                            Code = "ViewOneCategory",
                            Name = "ViewOneCategory"
                        },
                        new
                        {
                            IdPermission = 4,
                            Code = "CreateCategory",
                            Name = "CreateCategory"
                        },
                        new
                        {
                            IdPermission = 5,
                            Code = "DeleteCategory",
                            Name = "DeleteCategory"
                        },
                        new
                        {
                            IdPermission = 6,
                            Code = "UpdateCategory",
                            Name = "UpdateCategory"
                        },
                        new
                        {
                            IdPermission = 7,
                            Code = "ViewGift",
                            Name = "ViewGift"
                        },
                        new
                        {
                            IdPermission = 8,
                            Code = "ViewOneGift",
                            Name = "ViewOneGift"
                        },
                        new
                        {
                            IdPermission = 9,
                            Code = "CreateGift",
                            Name = "CreateGift"
                        },
                        new
                        {
                            IdPermission = 10,
                            Code = "DeleteGift",
                            Name = "DeleteGift"
                        },
                        new
                        {
                            IdPermission = 11,
                            Code = "UpdateGift",
                            Name = "UpdateGift"
                        },
                        new
                        {
                            IdPermission = 12,
                            Code = "AddPromoGiftToGift",
                            Name = "AddPromoGiftToGift"
                        },
                        new
                        {
                            IdPermission = 13,
                            Code = "ViewPromoGiftToGift",
                            Name = "ViewPromoGiftToGift"
                        },
                        new
                        {
                            IdPermission = 14,
                            Code = "UpdateQuantityPromoGiftInGift",
                            Name = "UpdateQuantityPromoGiftInGift"
                        },
                        new
                        {
                            IdPermission = 15,
                            Code = "DeletePromoGiftInGift",
                            Name = "DeletePromoGiftInGift"
                        },
                        new
                        {
                            IdPermission = 16,
                            Code = "GivenGift",
                            Name = "GivenGift"
                        },
                        new
                        {
                            IdPermission = 17,
                            Code = "CancelScheduledGiftDistribution",
                            Name = "CancelScheduledGiftDistribution"
                        },
                        new
                        {
                            IdPermission = 18,
                            Code = "GiveCoins",
                            Name = "GiveCoins"
                        },
                        new
                        {
                            IdPermission = 19,
                            Code = "ViewUser",
                            Name = "ViewUser"
                        },
                        new
                        {
                            IdPermission = 20,
                            Code = "ViewRole",
                            Name = "ViewRole"
                        },
                        new
                        {
                            IdPermission = 21,
                            Code = "ViewOneRole",
                            Name = "ViewOneRole"
                        },
                        new
                        {
                            IdPermission = 22,
                            Code = "CreateRole",
                            Name = "CreateRole"
                        },
                        new
                        {
                            IdPermission = 23,
                            Code = "DeleteRole",
                            Name = "DeleteRole"
                        },
                        new
                        {
                            IdPermission = 24,
                            Code = "UpdateRole",
                            Name = "UpdateRole"
                        },
                        new
                        {
                            IdPermission = 25,
                            Code = "AddListPermissionToRole",
                            Name = "AddListPermissionToRole"
                        },
                        new
                        {
                            IdPermission = 26,
                            Code = "GetPermissionInRole",
                            Name = "GetPermissionInRole"
                        },
                        new
                        {
                            IdPermission = 27,
                            Code = "AddListUserToRole",
                            Name = "AddListUserToRole"
                        },
                        new
                        {
                            IdPermission = 28,
                            Code = "ViewUserInRole",
                            Name = "ViewUserInRole"
                        },
                        new
                        {
                            IdPermission = 29,
                            Code = "ViewAllPermissions",
                            Name = "ViewAllPermissions"
                        },
                        new
                        {
                            IdPermission = 30,
                            Code = "ViewOnePermission",
                            Name = "ViewOnePermission"
                        },
                        new
                        {
                            IdPermission = 31,
                            Code = "CreatePermission",
                            Name = "CreatePermission"
                        },
                        new
                        {
                            IdPermission = 32,
                            Code = "DeletePermission",
                            Name = "DeletePermission"
                        },
                        new
                        {
                            IdPermission = 33,
                            Code = "UpdatePermission",
                            Name = "UpdatePermission"
                        });
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Promotion", b =>
                {
                    b.Property<int>("IdPromotion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPromotion"), 1L, 1);

                    b.Property<int>("IdMainGift")
                        .HasColumnType("int");

                    b.Property<int>("IdPromoGift")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("IdPromotion");

                    b.HasIndex("IdPromoGift");

                    b.HasIndex("IdMainGift", "IdPromoGift")
                        .IsUnique();

                    b.ToTable("Promotions");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Ranking", b =>
                {
                    b.Property<int>("IdRanking")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRanking"), 1L, 1);

                    b.Property<DateTime?>("ApprovedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("IdRanking");

                    b.ToTable("Ranking");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.RankingUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("IdRanking")
                        .HasColumnType("int");

                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdRanking");

                    b.HasIndex("IdUser");

                    b.ToTable("RankingUser");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdUser");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Role", b =>
                {
                    b.Property<int>("IdRole")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRole"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdRole");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            IdRole = 1,
                            Code = "sa",
                            Name = "Admin"
                        });
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.RolePermission", b =>
                {
                    b.Property<int>("IdRole")
                        .HasColumnType("int");

                    b.Property<int>("IdPermission")
                        .HasColumnType("int");

                    b.HasKey("IdRole", "IdPermission");

                    b.HasIndex("IdPermission");

                    b.ToTable("RolePermissions");

                    b.HasData(
                        new
                        {
                            IdRole = 1,
                            IdPermission = 1
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 2
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 3
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 4
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 5
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 6
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 7
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 8
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 9
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 10
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 11
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 12
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 13
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 14
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 15
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 16
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 17
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 18
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 19
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 20
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 21
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 22
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 23
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 24
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 25
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 26
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 27
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 28
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 29
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 30
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 31
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 32
                        },
                        new
                        {
                            IdRole = 1,
                            IdPermission = 33
                        });
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.RoleUser", b =>
                {
                    b.Property<int>("IdRole")
                        .HasColumnType("int");

                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.HasKey("IdRole", "IdUser");

                    b.HasIndex("IdUser");

                    b.ToTable("RoleUsers");

                    b.HasData(
                        new
                        {
                            IdRole = 1,
                            IdUser = 1
                        });
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Transaction", b =>
                {
                    b.Property<int>("IdTransaction")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdTransaction"), 1L, 1);

                    b.Property<int?>("AccumulatedPoints")
                        .HasColumnType("int");

                    b.Property<int>("CoinTotal")
                        .HasColumnType("int");

                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionTime")
                        .HasColumnType("datetime2");

                    b.HasKey("IdTransaction");

                    b.HasIndex("IdUser");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.TransactionDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("IdGift")
                        .HasColumnType("int");

                    b.Property<int>("IdTransaction")
                        .HasColumnType("int");

                    b.Property<int?>("MainGift")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdGift");

                    b.HasIndex("IdTransaction", "IdGift", "MainGift")
                        .IsUnique()
                        .HasFilter("[MainGift] IS NOT NULL");

                    b.ToTable("TransactionDetails");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Coin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "Admin123@gmail.com",
                            Password = "$2a$11$vMcHO1CGNi8ugjlU44nzl.W0sX3MMeN3dNw1sS3tu5k0zpFUoMusC",
                            Phone = "0123456789",
                            Username = "Admin123@"
                        });
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Cart", b =>
                {
                    b.HasOne("WebAPI_GiftManagement_BaoTran.Data.Gift", "Gift")
                        .WithMany("Carts")
                        .HasForeignKey("IdMainGift")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebAPI_GiftManagement_BaoTran.Data.User", "User")
                        .WithMany("Carts")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Gift");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Gift", b =>
                {
                    b.HasOne("WebAPI_GiftManagement_BaoTran.Data.Category", "Category")
                        .WithMany("Gifts")
                        .HasForeignKey("IdCategory")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Promotion", b =>
                {
                    b.HasOne("WebAPI_GiftManagement_BaoTran.Data.Gift", "MainGift")
                        .WithMany("MainGifts")
                        .HasForeignKey("IdMainGift")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebAPI_GiftManagement_BaoTran.Data.Gift", "PromoGift")
                        .WithMany("PromoGifts")
                        .HasForeignKey("IdPromoGift")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("MainGift");

                    b.Navigation("PromoGift");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.RankingUser", b =>
                {
                    b.HasOne("WebAPI_GiftManagement_BaoTran.Data.Ranking", "Ranking")
                        .WithMany("RankingUsers")
                        .HasForeignKey("IdRanking")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPI_GiftManagement_BaoTran.Data.User", "User")
                        .WithMany("RankingUsers")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ranking");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.RefreshToken", b =>
                {
                    b.HasOne("WebAPI_GiftManagement_BaoTran.Data.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.RolePermission", b =>
                {
                    b.HasOne("WebAPI_GiftManagement_BaoTran.Data.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("IdPermission")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPI_GiftManagement_BaoTran.Data.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("IdRole")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.RoleUser", b =>
                {
                    b.HasOne("WebAPI_GiftManagement_BaoTran.Data.Role", "Role")
                        .WithMany("RoleUsers")
                        .HasForeignKey("IdRole")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPI_GiftManagement_BaoTran.Data.User", "User")
                        .WithMany("RoleUsers")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Transaction", b =>
                {
                    b.HasOne("WebAPI_GiftManagement_BaoTran.Data.User", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.TransactionDetail", b =>
                {
                    b.HasOne("WebAPI_GiftManagement_BaoTran.Data.Gift", "Gift")
                        .WithMany("TransactionDetails")
                        .HasForeignKey("IdGift")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebAPI_GiftManagement_BaoTran.Data.Transaction", "Transaction")
                        .WithMany("TransactionDetails")
                        .HasForeignKey("IdTransaction")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Gift");

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Category", b =>
                {
                    b.Navigation("Gifts");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Gift", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("MainGifts");

                    b.Navigation("PromoGifts");

                    b.Navigation("TransactionDetails");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Permission", b =>
                {
                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Ranking", b =>
                {
                    b.Navigation("RankingUsers");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Role", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("RoleUsers");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.Transaction", b =>
                {
                    b.Navigation("TransactionDetails");
                });

            modelBuilder.Entity("WebAPI_GiftManagement_BaoTran.Data.User", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("RankingUsers");

                    b.Navigation("RefreshTokens");

                    b.Navigation("RoleUsers");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
