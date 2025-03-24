using FluentValidation;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;

namespace WebAPI_GiftManagement_BaoTran.Validators
{
    public class CategoryValidator : AbstractValidator<CategoryRequest>
    {
        private readonly IRepositoryWrapper _repository;

        public CategoryValidator(IRepositoryWrapper repositoryWrapper)
        {
            _repository = repositoryWrapper;

            RuleFor(gift => gift.Name)
             .NotEmpty().WithMessage("Name is required.");


        }


    }
}
