using Microsoft.EntityFrameworkCore.Storage;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.ReRepository;

namespace WebAPI_GiftManagement_BaoTran.Repository
{
    public interface IRepositoryWrapper
    {
        IRepositoryBase<Gift> Gifts { get; }
        IRepositoryBase<Category> Categories { get; }
        IRepositoryBase<Promotion> Promotions { get; }
        IRepositoryBase<Cart> Carts { get; }
        IRepositoryBase<Transaction> Transactions { get; }
        IRepositoryBase<TransactionDetail> TransactionDetails { get; }
        IRepositoryBase<User> Users { get; }
        IRepositoryBase<RefreshToken> RefreshTokens { get; }
        IRepositoryBase<RoleUser> RoleUser { get; }
        IRepositoryBase<Role> Role { get; }
        IRepositoryBase<Permission> Permission { get; }
        IRepositoryBase<RolePermission> RolePermission { get; }
        IRepositoryBase<Ranking> Rankings { get; }
        IRepositoryBase<RankingUser> RankingUser { get; }
        Task SaveChangeAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }

    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly MyDbContext _db;

        public RepositoryWrapper(MyDbContext db)
        {
            _db = db;
        }

        private IRepositoryBase<Gift> GiftsRepositoryBase;
        public IRepositoryBase<Gift> Gifts => GiftsRepositoryBase ??= new RepositoryBase<Gift>(_db);


        private IRepositoryBase<Category> CategoriesRepositoryBase;
        public IRepositoryBase<Category> Categories => CategoriesRepositoryBase ??= new RepositoryBase<Category>(_db);


        private IRepositoryBase<Promotion> PromotionsRepositoryBase;
        public IRepositoryBase<Promotion> Promotions => PromotionsRepositoryBase ??= new RepositoryBase<Promotion>(_db);


        private IRepositoryBase<Cart> CartsRepositoryBase;
        public IRepositoryBase<Cart> Carts => CartsRepositoryBase ??= new RepositoryBase<Cart>(_db);


        private IRepositoryBase<Transaction> TransactionsRepositoryBase;
        public IRepositoryBase<Transaction> Transactions => TransactionsRepositoryBase ??= new RepositoryBase<Transaction>(_db);


        private IRepositoryBase<TransactionDetail> TransactionDetailsRepositoryBase;
        public IRepositoryBase<TransactionDetail> TransactionDetails => TransactionDetailsRepositoryBase ??= new RepositoryBase<TransactionDetail>(_db);


        private IRepositoryBase<User> UsersRepositoryBase;
        public IRepositoryBase<User> Users => UsersRepositoryBase ??= new RepositoryBase<User>(_db);


        private IRepositoryBase<RefreshToken> RefreshTokensRepositoryBase;
        public IRepositoryBase<RefreshToken> RefreshTokens => RefreshTokensRepositoryBase ??= new RepositoryBase<RefreshToken>(_db);


        private IRepositoryBase<RoleUser> RoleUserRepositoryBase;
        public IRepositoryBase<RoleUser> RoleUser => RoleUserRepositoryBase ??= new RepositoryBase<RoleUser>(_db);


        private IRepositoryBase<Role> RoleRepositoryBase;
        public IRepositoryBase<Role> Role => RoleRepositoryBase ??= new RepositoryBase<Role>(_db);


        private IRepositoryBase<RolePermission> RolePermissionrRepositoryBase;
        public IRepositoryBase<RolePermission> RolePermission => RolePermissionrRepositoryBase ??= new RepositoryBase<RolePermission>(_db);


        private IRepositoryBase<Permission> PermissionrRepositoryBase;
        public IRepositoryBase<Permission> Permission => PermissionrRepositoryBase ??= new RepositoryBase<Permission>(_db);



        private IRepositoryBase<Ranking> RankingsRepositoryBase;
        public IRepositoryBase<Ranking> Rankings => RankingsRepositoryBase ??= new RepositoryBase<Ranking>(_db);



        private IRepositoryBase<RankingUser> RankingUsersRepositoryBase;
        public IRepositoryBase<RankingUser> RankingUser => RankingUsersRepositoryBase ??= new RepositoryBase<RankingUser>(_db);



        public async Task SaveChangeAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _db.Database.BeginTransactionAsync();
        }
    }
}
