using AutoMapper;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;
using WebAPI_GiftManagement_BaoTran.Validators;

namespace WebAPI_GiftManagement_BaoTran.Services
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionResponse>> GetAllPermission();
        Task<PermissionResponse> GetSinglePermission(int idPermission);
        Task<(bool Success, string ErrorMessage)> PostPermission(PermissionRequest permissionNew);
        Task<(bool Success, string ErrorMessage)> DeletePermission(int id);
        Task<(bool Success, string ErrorMessage)> PutPermission(int id, PermissionRequest permissionUpdate);
    }
    public class PermissionService : IPermissionService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly PermissionValidator _validatiorPermission;
        public PermissionService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PermissionResponse>> GetAllPermission()
        {
            IEnumerable<Permission> permissions = await _repository.Permission.GetAllAsync();
            return _mapper.Map<IEnumerable<PermissionResponse>>(permissions);
        }

        public async Task<PermissionResponse> GetSinglePermission(int idPermission)
        {
            Permission permission = await _repository.Permission.GetSingleAsync(g => g.IdPermission == idPermission);
            if (permission == null)
            {
                return null;
            }
            return _mapper.Map<PermissionResponse>(permission);
        }

        public async Task<(bool Success, string ErrorMessage)> PostPermission(PermissionRequest permissionNew)
        {
            if (await _repository.Permission.AnyAsync(g => g.Name == permissionNew.Name))
                return (false, "Name is already taken");

            FluentValidation.Results.ValidationResult validationResult = await _validatiorPermission.ValidateAsync(permissionNew);
            if (!validationResult.IsValid)
                return (false, validationResult.Errors.First().ErrorMessage);

            Permission permission = _mapper.Map<Permission>(permissionNew);

            _repository.Permission.Create(permission);
            await _repository.SaveChangeAsync();

            return (true, null);

        }

        public async Task<(bool Success, string ErrorMessage)> DeletePermission(int id)
        {
            Permission permission = await _repository.Permission.GetSingleAsync(u => u.IdPermission == id);
            if (permission == null)
                return (false, "Permission not found.");

            _repository.Permission.Delete(permission);
            await _repository.SaveChangeAsync();

            return (true, null);
        }

        public async Task<(bool Success, string ErrorMessage)> PutPermission(int id, PermissionRequest permissionUpdate)
        {
            Permission existingPermission = await _repository.Permission.GetSingleAsync(g => g.IdPermission == id);
            if (existingPermission == null)
                return (false, "Permission not found.");

            if (await _repository.Permission.AnyAsync(g => g.Name == permissionUpdate.Name && g.IdPermission != id))
                return (false, "Name is already taken");

            FluentValidation.Results.ValidationResult validationResult = await _validatiorPermission.ValidateAsync(permissionUpdate);
            if (!validationResult.IsValid)
                return (false, validationResult.Errors.First().ErrorMessage);

            _mapper.Map(permissionUpdate, existingPermission);

            _repository.Permission.Update(existingPermission);
            await _repository.SaveChangeAsync();

            return (true, null);
        }
    }
}
