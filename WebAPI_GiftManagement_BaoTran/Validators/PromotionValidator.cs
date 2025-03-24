using FluentValidation;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;

namespace WebAPI_GiftManagement_BaoTran.Validators
{
    public class PromotionValidator : AbstractValidator<PromotionRequest>
    {
        private readonly IRepositoryWrapper _repository;

        public PromotionValidator(IRepositoryWrapper repositoryWrapper)
        {
            _repository = repositoryWrapper;

            RuleFor(gift => gift.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0.");

            RuleFor(gift => gift.IdPromoGift)
                .NotEmpty().WithMessage("IdPromoGift is required.");

            RuleFor(gift => gift.IdMainGift)
                .NotEmpty().WithMessage("IdMainGift is required.");

        }


        public async Task<bool> IsGiftNotExist(int idGift)
        {
            if (await _repository.Gifts.AnyAsync(u => u.IdGift == idGift))
                return false;
            return true;
        }


    }
}
