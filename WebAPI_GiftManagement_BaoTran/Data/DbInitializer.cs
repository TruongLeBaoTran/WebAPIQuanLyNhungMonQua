using Microsoft.EntityFrameworkCore;

namespace WebAPI_GiftManagement_BaoTran.Data
{
    public class DbInitializer
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Khởi tạo dữ liệu cho Admin
            User adminUser = new()
            {
                Id = 1,
                Username = "Admin123@",
                Email = "Admin123@gmail.com",
                Phone = "0123456789",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin123@"),
                Image = null,
                Birthday = null,
                Coin = null,
            };

            modelBuilder.Entity<User>().HasData(adminUser);

            //Khởi tạo dữ liệu cho Role
            modelBuilder.Entity<Role>().HasData(
                new Role { IdRole = 1, Name = "Admin", Code = "sa" }
            );

            // Khởi tạo dữ liệu cho RoleUser
            modelBuilder.Entity<RoleUser>().HasData(
                new RoleUser { IdUser = 1, IdRole = 1 }
            );

            //Khởi tạo dữ liệu cho Permission
            modelBuilder.Entity<Permission>().HasData(
                new Permission { IdPermission = 1, Name = "CreateAdmin", Code = "CreateAdmin" },
                new Permission { IdPermission = 2, Name = "ViewCategory", Code = "ViewCategory" },
                new Permission { IdPermission = 3, Name = "ViewOneCategory", Code = "ViewOneCategory" },
                new Permission { IdPermission = 4, Name = "CreateCategory", Code = "CreateCategory" },
                new Permission { IdPermission = 5, Name = "DeleteCategory", Code = "DeleteCategory" },
                new Permission { IdPermission = 6, Name = "UpdateCategory", Code = "UpdateCategory" },
                new Permission { IdPermission = 7, Name = "ViewGift", Code = "ViewGift" },
                new Permission { IdPermission = 8, Name = "ViewOneGift", Code = "ViewOneGift" },
                new Permission { IdPermission = 9, Name = "CreateGift", Code = "CreateGift" },
                new Permission { IdPermission = 10, Name = "DeleteGift", Code = "DeleteGift" },
                new Permission { IdPermission = 11, Name = "UpdateGift", Code = "UpdateGift" },
                new Permission { IdPermission = 12, Name = "AddPromoGiftToGift", Code = "AddPromoGiftToGift" },
                new Permission { IdPermission = 13, Name = "ViewPromoGiftToGift", Code = "ViewPromoGiftToGift" },
                new Permission { IdPermission = 14, Name = "UpdateQuantityPromoGiftInGift", Code = "UpdateQuantityPromoGiftInGift" },
                new Permission { IdPermission = 15, Name = "DeletePromoGiftInGift", Code = "DeletePromoGiftInGift" },
                new Permission { IdPermission = 16, Name = "GivenGift", Code = "GivenGift" },
                new Permission { IdPermission = 17, Name = "CancelScheduledGiftDistribution", Code = "CancelScheduledGiftDistribution" },
                new Permission { IdPermission = 18, Name = "GiveCoins", Code = "GiveCoins" },
                new Permission { IdPermission = 19, Name = "ViewUser", Code = "ViewUser" },
                new Permission { IdPermission = 20, Name = "ViewRole", Code = "ViewRole" },
                new Permission { IdPermission = 21, Name = "ViewOneRole", Code = "ViewOneRole" },
                new Permission { IdPermission = 22, Name = "CreateRole", Code = "CreateRole" },
                new Permission { IdPermission = 23, Name = "DeleteRole", Code = "DeleteRole" },
                new Permission { IdPermission = 24, Name = "UpdateRole", Code = "UpdateRole" },
                new Permission { IdPermission = 25, Name = "AddListPermissionToRole", Code = "AddListPermissionToRole" },
                new Permission { IdPermission = 26, Name = "GetPermissionInRole", Code = "GetPermissionInRole" },
                new Permission { IdPermission = 27, Name = "AddListUserToRole", Code = "AddListUserToRole" },
                new Permission { IdPermission = 28, Name = "ViewUserInRole", Code = "ViewUserInRole" },
                new Permission { IdPermission = 29, Name = "ViewAllPermissions", Code = "ViewAllPermissions" },
                new Permission { IdPermission = 30, Name = "ViewOnePermission", Code = "ViewOnePermission" },
                new Permission { IdPermission = 31, Name = "CreatePermission", Code = "CreatePermission" },
                new Permission { IdPermission = 32, Name = "DeletePermission", Code = "DeletePermission" },
                new Permission { IdPermission = 33, Name = "UpdatePermission", Code = "UpdatePermission" }
            );





            //    //Khởi tạo dữ liệu cho RolePermission
            modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { IdPermission = 1, IdRole = 1 },
            new RolePermission { IdPermission = 2, IdRole = 1 },
            new RolePermission { IdPermission = 3, IdRole = 1 },
            new RolePermission { IdPermission = 4, IdRole = 1 },
            new RolePermission { IdPermission = 5, IdRole = 1 },
            new RolePermission { IdPermission = 6, IdRole = 1 },
            new RolePermission { IdPermission = 7, IdRole = 1 },
            new RolePermission { IdPermission = 8, IdRole = 1 },
            new RolePermission { IdPermission = 9, IdRole = 1 },
            new RolePermission { IdPermission = 10, IdRole = 1 },
            new RolePermission { IdPermission = 11, IdRole = 1 },
            new RolePermission { IdPermission = 12, IdRole = 1 },
            new RolePermission { IdPermission = 13, IdRole = 1 },
            new RolePermission { IdPermission = 14, IdRole = 1 },
            new RolePermission { IdPermission = 15, IdRole = 1 },
            new RolePermission { IdPermission = 16, IdRole = 1 },
            new RolePermission { IdPermission = 17, IdRole = 1 },
            new RolePermission { IdPermission = 18, IdRole = 1 },
            new RolePermission { IdPermission = 19, IdRole = 1 },
            new RolePermission { IdPermission = 20, IdRole = 1 },
            new RolePermission { IdPermission = 21, IdRole = 1 },
            new RolePermission { IdPermission = 22, IdRole = 1 },
            new RolePermission { IdPermission = 23, IdRole = 1 },
            new RolePermission { IdPermission = 24, IdRole = 1 },
            new RolePermission { IdPermission = 25, IdRole = 1 },
            new RolePermission { IdPermission = 26, IdRole = 1 },
            new RolePermission { IdPermission = 27, IdRole = 1 },
            new RolePermission { IdPermission = 28, IdRole = 1 },
            new RolePermission { IdPermission = 29, IdRole = 1 },
            new RolePermission { IdPermission = 30, IdRole = 1 },
            new RolePermission { IdPermission = 31, IdRole = 1 },
            new RolePermission { IdPermission = 32, IdRole = 1 },
            new RolePermission { IdPermission = 33, IdRole = 1 }
        );


        }
    }
}
