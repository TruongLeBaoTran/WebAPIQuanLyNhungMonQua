using FluentValidation;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;

namespace WebAPI_GiftManagement_BaoTran.Validators
{
    public class PermissionValidator : AbstractValidator<PermissionRequest>
    {
        private readonly IRepositoryWrapper _repository;
        public PermissionValidator(IRepositoryWrapper repository)
        {
            _repository = repository;

            RuleFor(permission => permission.Name)
                .NotEmpty().WithMessage("Name permission is required.");

            RuleFor(permission => permission.Code)
                .NotEmpty().WithMessage("Code permission is required.");

        }

    }
}
