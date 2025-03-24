using FluentValidation;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;

namespace WebAPI_GiftManagement_BaoTran.Validators
{
    public class GiftValidator : AbstractValidator<GiftRequest>
    {
        private readonly IRepositoryWrapper _repository;

        public GiftValidator(IRepositoryWrapper repositoryWrapper)
        {
            _repository = repositoryWrapper;

            RuleFor(gift => gift.Name)
             .NotEmpty().WithMessage("Name is required.");

            RuleFor(gift => gift.Image)
                .NotEmpty().WithMessage("Image is required.");

            RuleFor(gift => gift.RemainingQuantity)
                .NotEmpty().WithMessage("RemainingQuantity is required.")
                .GreaterThanOrEqualTo(0).WithMessage("RemainingQuantity must be greater than or equal to 0.");

            RuleFor(gift => gift.Coin)
                .NotEmpty().WithMessage("Coin is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Coin must be greater than or equal to 0.");

            RuleFor(gift => gift.StartDate)
                .NotEmpty().WithMessage("StartDate is required.")
                .LessThan(gift => gift.EndDate).WithMessage("StartDate must be less than EndDate.");

            RuleFor(gift => gift.EndDate)
                .NotEmpty().WithMessage("EndDate is required.");

        }

        //Kiểm tra thời hạn của quà 
        public async Task<bool> IsGiftExpired(int IdGift)
        {
            return await _repository.Gifts.AnyAsync(x => x.IdGift == IdGift && x.EndDate < DateTime.Now);
        }

        //Kiểm tra các quà khuyến mãi có đủ số lượng không(1 quà chính 2 quà km thì 2 quà chính là 4 km) --> đủ km mới phát
        public async Task<bool> IsQuantityPromoGiftNotAvailable(int idGift, int quantityRequested)
        {
            IEnumerable<Promotion> listPromoOfGift = await _repository.Promotions.GetAllAsync(gp => gp.IdMainGift == idGift);
            if (!listPromoOfGift.Any()) //quà chính này ko có quà km nào kèm theo
            {
                return true;
            }

            foreach (Promotion promo in listPromoOfGift)
            {
                Promotion promoInStock = await _repository.Promotions.GetSingleAsync(x => x.IdPromoGift == promo.IdPromoGift);
                if (promo.Quantity * quantityRequested > promoInStock.Quantity)
                {
                    return true; //quà chính này có quà km kèm theo nhưng 1 trong số quà km này số lượng không đủ
                }
            }
            return false;

        }


        //Kiểm tra quà chính có quà khuyến mãi nào không
        public async Task<bool> IsPromoGiftRunOut(int idGift)
        {
            IEnumerable<Promotion> listPromoOfGift = await _repository.Promotions.GetAllAsync(gp => gp.IdMainGift == idGift);
            if (!listPromoOfGift.Any()) //quà chính này ko có quà km nào kèm theo
            {
                return true;
            }
            foreach (Promotion promo in listPromoOfGift)
            {
                Gift promoInStock = await _repository.Gifts.GetSingleAsync(x => x.IdGift == promo.IdPromoGift);
                if (promoInStock.RemainingQuantity != 0) //còn quà km
                {
                    return false; //quà chính có quà km kèm theo và ít nhất 1 quà km còn hàng
                }
            }
            return true; //tất cả = 0

        }


        //Kiểm tra số lượng của quà chính trong kho
        public async Task<bool> IsGiftQuantityAvailable(int idGift, int quantityRequested)
        {
            return await _repository.Gifts.AnyAsync(x => x.IdGift == idGift && x.RemainingQuantity >= quantityRequested);// trả về true khi đủ sl yêu cầu 
        }


        ////Kiểm tra quà chính tồn tại trong tủ chưa
        //public async Task<bool> IsGiftExistInWardrobe(int IdUser, int IdGift)
        //{
        //    return await _repository.WardrobeGift.AnyAsync(g => g.IdUser == IdUser && g.IdGift == IdGift);
        //}

        //Kiểm tra quà chính tồn tại trong giỏ hàng chưa
        public async Task<bool> IsGiftExistInCart(int IdUser, int IdGift)
        {
            return await _repository.Carts.AnyAsync(g => g.IdUser == IdUser && g.IdMainGift == IdGift);
        }

    }
}
