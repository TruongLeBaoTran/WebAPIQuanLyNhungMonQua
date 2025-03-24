using AutoMapper;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;
using WebAPI_GiftManagement_BaoTran.Validators;

namespace WebAPI_GiftManagement_BaoTran.Services
{
    public interface IPromotionService
    {
        Task<(bool Success, string ErrorMessage)> AddPromoGiftToGift(PromotionRequest promotionRequest);
        Task<IEnumerable<PromotionResponse>> GetPromoGiftInGift(int idMainGift);
        Task<(bool Success, string ErrorMessage)> DeletePromoGiftInGift(int idPromotion);
        Task<(bool Success, string ErrorMessage)> PutQuantityPromoGiftInGift(PromotionRequest promotionRequest);

    }
    public class PromotionService : IPromotionService
    {
        public readonly IMapper mapper;
        public readonly IRepositoryWrapper repository;
        public readonly PromotionValidator validation;

        public PromotionService(IMapper mapper, IRepositoryWrapper repository, PromotionValidator validation)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.validation = validation;
        }

        public async Task<(bool Success, string ErrorMessage)> AddPromoGiftToGift(PromotionRequest promotionRequest)
        {
            if (await repository.Promotions.AnyAsync(g => g.IdMainGift == promotionRequest.IdMainGift && g.IdPromoGift == promotionRequest.IdPromoGift))
                return (false, "Data is already taken");

            FluentValidation.Results.ValidationResult validationResult = await validation.ValidateAsync(promotionRequest);
            if (!validationResult.IsValid)
                return (false, validationResult.Errors.First().ErrorMessage);

            if (await validation.IsGiftNotExist(promotionRequest.IdMainGift)) return (false, "Main gift is not exist");

            if (await validation.IsGiftNotExist(promotionRequest.IdPromoGift)) return (false, "Promotion gift is not exist");

            Promotion gift = mapper.Map<Promotion>(promotionRequest);

            repository.Promotions.Create(gift);
            await repository.SaveChangeAsync();

            return (true, null);

        }

        public async Task<IEnumerable<PromotionResponse>> GetPromoGiftInGift(int idMainGift)
        {
            IEnumerable<Promotion> listPromoGifts = await repository.Promotions.GetAllAsync(g => g.IdMainGift == idMainGift);
            if (!listPromoGifts.Any())
            {
                return null;
            }
            Gift mainGift = await repository.Gifts.GetSingleAsync(x => x.IdGift == idMainGift);
            foreach (Promotion promo in listPromoGifts)
            {
                Gift promoGift = await repository.Gifts.GetSingleAsync(x => x.IdGift == promo.IdPromoGift);
                PromotionResponse promotionResponse = new()
                {
                    IdMainGift = idMainGift,
                    MainGiftName = mainGift.Name,
                    MainGiftImage = mainGift.Image,
                    IdPromoGift = promo.IdPromoGift,
                    PromoGiftName = promoGift.Name,
                    PromoGiftImage = promoGift.Image
                };
            }


            return mapper.Map<IEnumerable<PromotionResponse>>(listPromoGifts);
        }

        public async Task<(bool Success, string ErrorMessage)> DeletePromoGiftInGift(int idPromotion)
        {
            Promotion? gift = await repository.Promotions.GetSingleAsync(u => u.IdPromotion == idPromotion);
            if (gift == null)
                return (false, "Gift not found.");

            repository.Promotions.Delete(gift);
            await repository.SaveChangeAsync();

            return (true, null);
        }

        public async Task<(bool Success, string ErrorMessage)> PutQuantityPromoGiftInGift(PromotionRequest promotionRequest)
        {

            if (await validation.IsGiftNotExist(promotionRequest.IdMainGift))
                return (false, "Main Gift not found.");

            if (await validation.IsGiftNotExist(promotionRequest.IdPromoGift))
                return (false, "Promotion Gift not found.");

            Promotion existingPromoGiftInGift = await repository.Promotions.GetSingleAsync(g => g.IdPromoGift == promotionRequest.IdPromoGift && g.IdMainGift == promotionRequest.IdMainGift);
            if (existingPromoGiftInGift == null)
                return (false, "Promo Gift In Gift not found.");

            FluentValidation.Results.ValidationResult validationResult = await validation.ValidateAsync(promotionRequest);
            if (!validationResult.IsValid)
                return (false, validationResult.Errors.First().ErrorMessage);

            mapper.Map(promotionRequest, existingPromoGiftInGift);

            repository.Promotions.Update(existingPromoGiftInGift);
            await repository.SaveChangeAsync();

            return (true, null);
        }
    }
}

