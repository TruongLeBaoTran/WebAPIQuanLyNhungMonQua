using AutoMapper;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;
using WebAPI_GiftManagement_BaoTran.Validators;

namespace WebAPI_GiftManagement_BaoTran.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResponse>> GetAllRoles();
        Task<RoleResponse> GetSingleRole(int idRole);
        Task<(bool Success, string ErrorMessage)> PostRole(RoleRequest roleNew);
        Task<(bool Success, string ErrorMessage)> DeleteRole(int id);
        Task<(bool Success, string ErrorMessage)> PutRole(int id, RoleRequest roleUpdate);

    }

    public class RoleService : IRoleService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly RoleValidator _validatorRole;

        public RoleService(IRepositoryWrapper repository, IMapper mapper, RoleValidator validatorRole)
        {
            _repository = repository;
            _mapper = mapper;
            _validatorRole = validatorRole;
        }

        public async Task<IEnumerable<RoleResponse>> GetAllRoles()
        {
            IEnumerable<Role> roles = await _repository.Role.GetAllAsync();
            return _mapper.Map<IEnumerable<RoleResponse>>(roles);
        }

        public async Task<RoleResponse> GetSingleRole(int idRole)
        {
            Role role = await _repository.Role.GetSingleAsync(g => g.IdRole == idRole);
            if (role == null)
            {
                return null;
            }
            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<(bool Success, string ErrorMessage)> PostRole(RoleRequest roleNew)
        {
            if (await _repository.Role.AnyAsync(g => g.Name == roleNew.Name))
                return (false, "Name is already taken");

            FluentValidation.Results.ValidationResult validationResult = await _validatorRole.ValidateAsync(roleNew);
            if (!validationResult.IsValid)
                return (false, validationResult.Errors.First().ErrorMessage);

            Role role = _mapper.Map<Role>(roleNew);

            _repository.Role.Create(role);
            await _repository.SaveChangeAsync();

            return (true, null);
        }


        public async Task<(bool Success, string ErrorMessage)> DeleteRole(int id)
        {
            Role role = await _repository.Role.GetSingleAsync(u => u.IdRole == id);
            if (role == null)
                return (false, "Role not found");

            _repository.Role.Delete(role);
            await _repository.SaveChangeAsync();

            return new(true, null);
        }

        public async Task<(bool Success, string ErrorMessage)> PutRole(int id, RoleRequest roleUpdate)
        {
            Role existingRole = await _repository.Role.GetSingleAsync(g => g.IdRole == id);
            if (existingRole == null)
                return new(false, "Role not found");

            if (await _repository.Role.AnyAsync(g => g.Name == roleUpdate.Name && g.IdRole != id))
                return (false, "Name is already taken");

            FluentValidation.Results.ValidationResult validationResult = await _validatorRole.ValidateAsync(roleUpdate);
            if (!validationResult.IsValid)
                return new(false, validationResult.Errors.First().ErrorMessage);

            _mapper.Map(roleUpdate, existingRole);

            _repository.Role.Update(existingRole);
            await _repository.SaveChangeAsync();

            return new(true, null);
        }

    }
}
