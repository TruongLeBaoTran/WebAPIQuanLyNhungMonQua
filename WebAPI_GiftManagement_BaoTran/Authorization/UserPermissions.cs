using Microsoft.EntityFrameworkCore;
using WebAPI_GiftManagement_BaoTran.Data;

namespace WebAPI_GiftManagement_BaoTran.Authorization
{
    public interface IUserPermissions
    {
        Task<List<Permission>> GetUserPermissionsAsync(int userId);
    }

    public class UserPermissions : IUserPermissions
    {
        private readonly MyDbContext _context;

        public UserPermissions(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Permission>> GetUserPermissionsAsync(int userId)
        {
            List<RoleUser> userRoles = await _context.RoleUsers
                .Where(ur => ur.IdUser == userId)
                .Include(ur => ur.Role)
                .ToListAsync();

            List<int> roleIds = userRoles.Select(ur => ur.Role.IdRole).ToList();

            List<Permission> permissions = await _context.RolePermissions
                .Where(rp => roleIds.Contains(rp.IdRole))
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission)
                .ToListAsync();

            return permissions;
        }

    }



}
