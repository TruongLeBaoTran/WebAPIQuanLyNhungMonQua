using AutoMapper;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;
using WebAPI_GiftManagement_BaoTran.Validators;

namespace WebAPI_GiftManagement_BaoTran.Services
{
    public interface IRoleUserService
    {
        Task<IEnumerable<RoleUserResponse>> GetUserInRole(int idRole);
        Task<(bool Success, string ErrorMessage)> AddListUserToRole(int IdRole, List<int> listIdUsers);
    }

    public class RoleUserService : IRoleUserService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly RoleUserValidator _validationRoleUser;
        public RoleUserService(IRepositoryWrapper repository, IMapper mapper, RoleUserValidator validation)
        {
            _repository = repository;
            _mapper = mapper;
            _validationRoleUser = validation;
        }

        public async Task<IEnumerable<RoleUserResponse>> GetUserInRole(int idRole)
        {
            IEnumerable<RoleUser> roleUsers = await _repository.RoleUser.GetAllAsync(g => g.IdRole == idRole);
            if (!roleUsers.Any())
            {
                return null;
            }
            return _mapper.Map<IEnumerable<RoleUserResponse>>(roleUsers);
        }

        public async Task<bool> AddUserInRole(int idRole, List<int> listIdUsers, List<int> listUserInRoleIds)
        {
            List<int> usersToAdd = listIdUsers.Except(listUserInRoleIds).ToList(); //Những user trong listIdUser nhưng ko có trong listUserInRole

            foreach (int idUser in usersToAdd)
            {
                bool result2 = await _repository.Users.AnyAsync(x => x.Id == idUser);
                if (!result2) return false;

                if (await _validationRoleUser.IsRowNotExist(idRole, idUser))
                {
                    RoleUser roleUser = new()
                    {
                        IdRole = idRole,
                        IdUser = idUser,
                    };
                    _repository.RoleUser.Create(roleUser);
                }

            }
            return true;
        }


        public async Task<bool> DeleteUserInRole(IEnumerable<RoleUser> listUserInRole, List<int> listIdUsers, List<int> listUserInRoleIds)
        {
            List<int> usersToRemove = listUserInRoleIds.Except(listIdUsers).ToList();  // Những user trong listUserInRole nhưng không có trong listIdUser

            foreach (int idUser in usersToRemove)
            {
                bool result2 = await _repository.Users.AnyAsync(x => x.Id == idUser);
                if (!result2) return false;

                RoleUser? roleUser = listUserInRole.FirstOrDefault(r => r.IdUser == idUser);
                if (roleUser != null)
                {
                    _repository.RoleUser.Delete(roleUser);
                }
            }
            return true;
        }

        //Thêm/update nhóm user vào 1 role
        public async Task<(bool Success, string ErrorMessage)> AddListUserToRole(int idRole, List<int> listIdUsers)
        {
            bool result = await _repository.Role.AnyAsync(x => x.IdRole == idRole);
            if (!result) return (false, "IdRole not found");

            IEnumerable<RoleUser> listUserInRole = await _repository.RoleUser.GetAllAsync(x => x.IdRole == idRole);
            if (listUserInRole.Any())
            {
                List<int> listUserInRoleIds = listUserInRole.Select(r => r.IdUser).ToList();

                //Những user trong listIdUser nhưng ko có trong listUserInRole --> thêm dòng (idrole, iduser) vào bảng RoleUser
                bool result2 = await AddUserInRole(idRole, listIdUsers, listUserInRoleIds);
                if (!result2) return (false, "IdUser not found");

                // Những user trong listUserInRole nhưng không có trong listIdUser--> xóa dòng (idrole, iduser) ra khỏi bảng RoleUser
                bool result3 = await DeleteUserInRole(listUserInRole, listIdUsers, listUserInRoleIds);
                if (!result3) return (false, "IdUser not found");

            }
            else //Idrole chưa tồn tại --> thêm dòng mới
            {

                foreach (int idUser in listIdUsers)
                {
                    bool result2 = await _repository.Users.AnyAsync(x => x.Id == idUser);
                    if (!result2) return (false, "IdUser not found");

                    if (await _validationRoleUser.IsRowNotExist(idRole, idUser))
                    {
                        RoleUser roleUser = new()
                        {
                            IdRole = idRole,
                            IdUser = idUser,
                        };
                        _repository.RoleUser.Create(roleUser);
                    }

                }
            }

            await _repository.SaveChangeAsync();
            return (true, null);
        }

    }
}
