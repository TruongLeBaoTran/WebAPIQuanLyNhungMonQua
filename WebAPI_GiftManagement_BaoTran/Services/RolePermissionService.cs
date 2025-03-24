using AutoMapper;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;
using WebAPI_GiftManagement_BaoTran.Validators;

namespace WebAPI_GiftManagement_BaoTran.Services
{
    public interface IRolePermissionService
    {
        Task<IEnumerable<RolePermissionResponse>> GetPermissionInRole(int idRole);
        Task<(bool Success, string ErrorMessage)> AddListPermissionToRole(int IdRole, List<int> listIdPermissions);
    }
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly RolePermissionValidator _validatorRolePermission;

        public RolePermissionService(IRepositoryWrapper repository, IMapper mapper, RolePermissionValidator validatorRolePermission)
        {
            _repository = repository;
            _mapper = mapper;
            _validatorRolePermission = validatorRolePermission;

        }

        public async Task<IEnumerable<RolePermissionResponse>> GetPermissionInRole(int idRole)
        {
            IEnumerable<RolePermission> rolePermissions = await _repository.RolePermission.GetAllAsync(g => g.IdRole == idRole);
            if (!rolePermissions.Any())
            {
                return null;
            }
            return _mapper.Map<IEnumerable<RolePermissionResponse>>(rolePermissions);
        }

        public async Task<bool> AddPermissionInRole(int idRole, List<int> listIdPermissions, List<int> listPermissionInRoleIds)
        {
            List<int> permisionsToAdd = listIdPermissions.Except(listPermissionInRoleIds).ToList(); //Những permission trong listIdPermission nhưng ko có trong listPermissionInRoleIds

            foreach (int idPermission in permisionsToAdd)
            {
                bool result2 = await _repository.Permission.AnyAsync(x => x.IdPermission == idPermission);
                if (!result2) return false;

                if (await _validatorRolePermission.IsRowNotExist(idRole, idPermission))
                {
                    RolePermission rolePermission = new()
                    {
                        IdRole = idRole,
                        IdPermission = idPermission,
                    };
                    _repository.RolePermission.Create(rolePermission);
                }

            }
            return true;
        }


        public async Task<bool> DeletePermissionInRole(IEnumerable<RolePermission> listPermissionInRole, List<int> listIdPermissions, List<int> listPermissionInRoleIds)
        {
            List<int> permissionsToRemove = listPermissionInRoleIds.Except(listIdPermissions).ToList();  // Những permission trong listPermissionInRole nhưng không có trong listIdPermission

            foreach (int idPermission in permissionsToRemove)
            {
                bool result2 = await _repository.Permission.AnyAsync(x => x.IdPermission == idPermission);
                if (!result2) return false;

                RolePermission? rolePermission = listPermissionInRole.FirstOrDefault(r => r.IdPermission == idPermission);
                if (rolePermission != null)
                {
                    _repository.RolePermission.Delete(rolePermission);
                }
            }
            return true;
        }

        //Thêm/update nhóm Permission vào 1 role
        public async Task<(bool Success, string ErrorMessage)> AddListPermissionToRole(int idRole, List<int> listIdPermissions)
        {
            bool result = await _repository.Role.AnyAsync(x => x.IdRole == idRole);
            if (!result) return (false, "IdRole not found");

            IEnumerable<RolePermission> listPermissionInRole = await _repository.RolePermission.GetAllAsync(x => x.IdRole == idRole);
            if (listPermissionInRole.Any())
            {
                List<int> listPermissionInRoleIds = listPermissionInRole.Select(r => r.IdPermission).ToList();

                //Những Permission trong listIdPermission nhưng ko có trong listPermissionInRole --> thêm dòng (idrole, idPermission) vào bảng RolePermission
                bool result2 = await AddPermissionInRole(idRole, listIdPermissions, listPermissionInRoleIds);
                if (!result2) return (false, "IdPermission not found");

                // Những Permission trong listPermissionInRole nhưng không có trong listIdPermission--> xóa dòng (idrole, idPermission) ra khỏi bảng RolePermission
                bool result3 = await DeletePermissionInRole(listPermissionInRole, listIdPermissions, listPermissionInRoleIds);
                if (!result3) return (false, "IdPermission not found");

            }
            else
            {
                //idrole chua ton tai --> them moi dong
                foreach (int idPermission in listIdPermissions)
                {
                    bool result2 = await _repository.Permission.AnyAsync(x => x.IdPermission == idPermission);
                    if (!result2) return (false, "IdPermission not found");

                    if (await _validatorRolePermission.IsRowNotExist(idRole, idPermission))
                    {
                        RolePermission rolePermission = new()
                        {
                            IdRole = idRole,
                            IdPermission = idPermission,
                        };
                        _repository.RolePermission.Create(rolePermission);
                    }

                }
            }

            await _repository.SaveChangeAsync();
            return (true, null);
        }

    }
}
