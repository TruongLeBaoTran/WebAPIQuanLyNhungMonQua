using FluentValidation;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;

namespace WebAPI_GiftManagement_BaoTran.Validators
{
    public class RoleUserValidator : AbstractValidator<RoleUserRequest>
    {
        private readonly IRepositoryWrapper _repository;
        public RoleUserValidator(IRepositoryWrapper repository)
        {
            _repository = repository;

            RuleFor(RoleUser => RoleUser.IdRole)
                .NotEmpty().WithMessage("IdRole is required.");

            RuleFor(RoleUser => RoleUser.IdUser)
                .NotEmpty().WithMessage("IdUser is required.");

        }

        public async Task<bool> IsRowNotExist(int idRole, int idUser)
        {
            if (await _repository.RoleUser.AnyAsync(u => u.IdRole == idRole && u.IdUser == idUser))
                return false;
            return true;
        }

    }
}
