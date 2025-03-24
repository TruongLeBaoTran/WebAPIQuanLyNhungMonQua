using FluentValidation;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;

namespace WebAPI_GiftManagement_BaoTran.Validators
{
    public class RolePermissionValidator : AbstractValidator<RolePermissionRequest>
    {
        private readonly IRepositoryWrapper _repository;
        public RolePermissionValidator(IRepositoryWrapper repository)
        {
            _repository = repository;

            RuleFor(permission => permission.IdRole)
                .NotEmpty().WithMessage("Name permission is required.");

            RuleFor(permission => permission.IdPermission)
                .NotEmpty().WithMessage("Code permission is required.");

        }

        public async Task<bool> IsRowNotExist(int idRole, int idPermission)
        {
            if (await _repository.RolePermission.AnyAsync(u => u.IdRole == idRole && u.IdPermission == idPermission))
                return false;
            return true;
        }

    }
}
