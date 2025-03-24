using Microsoft.EntityFrameworkCore;

namespace WebAPI_GiftManagement_BaoTran.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }

        #region DbSet
        public DbSet<User> User { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<RoleUser> RoleUsers { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        public DbSet<Category> Categories { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Users
            modelBuilder.Entity<User>()
               .Property(g => g.Coin)
               .HasDefaultValue(0);


            //Gift
            modelBuilder.Entity<Gift>()
                .HasOne(g => g.Category)
                .WithMany(g => g.Gifts)
                .HasForeignKey(g => g.IdCategory)
                .OnDelete(DeleteBehavior.Restrict);

            //Cart
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Gift)
                .WithMany(c => c.Carts)
                .HasForeignKey(c => c.IdMainGift)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(c => c.Carts)
                .HasForeignKey(c => c.IdUser)
                .OnDelete(DeleteBehavior.Restrict);

            //Promotion
            modelBuilder.Entity<Promotion>()
                .HasOne(p => p.MainGift)
                .WithMany(p => p.MainGifts)
                .HasForeignKey(p => p.IdMainGift)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Promotion>()
                .HasOne(P => P.PromoGift)
                .WithMany(P => P.PromoGifts)
                .HasForeignKey(P => P.IdPromoGift)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Promotion>()
                .HasIndex(cg => new { cg.IdMainGift, cg.IdPromoGift }) //cặp duy nhất
                .IsUnique();

            //Transaction
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(t => t.Transactions)
                .HasForeignKey(t => t.IdUser)
                .OnDelete(DeleteBehavior.Restrict);

            //TransactionDetail
            modelBuilder.Entity<TransactionDetail>()
                .HasOne(t => t.Transaction)
                .WithMany(t => t.TransactionDetails)
                .HasForeignKey(t => t.IdTransaction)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionDetail>()
                .HasOne(t => t.Gift)
                .WithMany(t => t.TransactionDetails)
                .HasForeignKey(t => t.IdGift)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionDetail>()
                .HasIndex(cg => new { cg.IdTransaction, cg.IdGift, cg.MainGift }) //bộ ba duy nhất
                .IsUnique();


            // RefreshToken
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(r => r.RefreshTokens)
                .HasForeignKey(rt => rt.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

            // RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.IdRole, rp.IdPermission }); // Đặt khóa chính

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.IdRole);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.IdPermission);

            // RoleUser
            modelBuilder.Entity<RoleUser>()
                .HasKey(rp => new { rp.IdRole, rp.IdUser }); // Đặt khóa chính

            modelBuilder.Entity<RoleUser>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RoleUsers)
                .HasForeignKey(rp => rp.IdRole);

            modelBuilder.Entity<RoleUser>()
                .HasOne(rp => rp.User)
                .WithMany(u => u.RoleUsers)
                .HasForeignKey(rp => rp.IdUser);


            // RankingUser
            modelBuilder.Entity<RankingUser>()
                .HasKey(rp => new { rp.Id }); // Đặt khóa chính

            modelBuilder.Entity<RankingUser>()
                .HasOne(rp => rp.Ranking)
                .WithMany(r => r.RankingUsers)
                .HasForeignKey(rp => rp.IdRanking);

            modelBuilder.Entity<RankingUser>()
                .HasOne(rp => rp.User)
                .WithMany(u => u.RankingUsers)
                .HasForeignKey(rp => rp.IdUser);


            DbInitializer.Seed(modelBuilder);
        }




    }
}
