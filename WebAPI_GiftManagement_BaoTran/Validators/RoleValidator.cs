using FluentValidation;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;

namespace WebAPI_GiftManagement_BaoTran.Validators
{
    public class RoleValidator : AbstractValidator<RoleRequest>
    {
        private readonly IRepositoryWrapper _repository;
        public RoleValidator(IRepositoryWrapper repository)
        {
            _repository = repository;

            RuleFor(role => role.Name)
                .NotEmpty().WithMessage("Name role is required.");

            RuleFor(role => role.Code)
                .NotEmpty().WithMessage("Code role is required.");

        }

    }
}
