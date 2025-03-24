using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;
using WebAPI_GiftManagement_BaoTran.Validators;

namespace WebAPI_GiftManagement_BaoTran.Services
{
    public interface IGiftService
    {
        Task<IEnumerable<GiftResponse>> GetAllGifts();
        Task<GiftResponse> GetSingleGift(int idGift);
        Task<(bool Success, string ErrorMessage)> PostGift(GiftRequest giftNew);
        Task<(bool Success, string ErrorMessage)> DeleteGift(int id);
        Task<(bool Success, string ErrorMessage)> PutGift(int id, GiftRequest giftUpdate);
        Task<string> SaveImage(IFormFile imageFile);

    }

    public class GiftService : IGiftService
    {
        private readonly IMapper _mapper;
        private readonly GiftValidator _validationGift;
        private readonly IRepositoryWrapper _repository;

        public GiftService(IMapper mapper, GiftValidator validationRules, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _validationGift = validationRules;
            _repository = repository;
        }

        public async Task<IEnumerable<GiftResponse>> GetAllGifts()
        {
            IEnumerable<Gift> gifts = await _repository.Gifts.GetAllAsync();
            return _mapper.Map<IEnumerable<GiftResponse>>(gifts);
        }

        public async Task<GiftResponse> GetSingleGift(int idGift)
        {
            Gift gift = await _repository.Gifts.GetSingleAsync(g => g.IdGift == idGift);
            if (gift == null) return null;

            return _mapper.Map<GiftResponse>(gift);
        }

        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string[] allowedMimeTypes = new[] { "image/png", "image/jpeg", "image/gif" };

            if (imageFile != null && imageFile.Length > 0)
            {
                if (imageFile.Length > 10000000) return "File too large"; // Kích thước tối đa là 10MB

                if (!allowedMimeTypes.Contains(imageFile.ContentType)) return "Invalid file type";

                // Lưu hình ảnh vào thư mục
                string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                // Kiểm tra nếu thư mục không tồn tại thì tạo mới
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imageFile.FileName);
                string extension = Path.GetExtension(imageFile.FileName);

                string newFileName = $"{fileNameWithoutExtension}-{DateTime.UtcNow.Ticks}{extension}";
                string dest = Path.Combine(uploads, newFileName);

                using (FileStream stream = new(dest, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return $"/images/{newFileName}";
            }
            else
            {
                return "";
            }
        }


        public async Task<(bool Success, string ErrorMessage)> PostGift(GiftRequest giftNew)
        {

            if (await _repository.Gifts.AnyAsync(g => g.Name == giftNew.Name))
                return (false, "Name is already taken");

            FluentValidation.Results.ValidationResult validationResult = await _validationGift.ValidateAsync(giftNew);
            if (!validationResult.IsValid)
                return (false, validationResult.Errors.First().ErrorMessage);

            Gift gift = _mapper.Map<Gift>(giftNew);

            gift.Image = await SaveImage(giftNew.Image);

            _repository.Gifts.Create(gift);
            await _repository.SaveChangeAsync();

            return (true, null);

        }

        public async Task<(bool Success, string ErrorMessage)> DeleteGift(int id)
        {
            Gift? gift = await _repository.Gifts.GetSingleAsync(u => u.IdGift == id);
            if (gift == null)
                return (false, "Gift not found.");

            _repository.Gifts.Delete(gift);
            await _repository.SaveChangeAsync();

            return (true, null);
        }

        public async Task<(bool Success, string ErrorMessage)> PutGift(int id, GiftRequest giftUpdate)
        {
            Gift existingGift = await _repository.Gifts.GetSingleAsync(g => g.IdGift == id);
            if (existingGift == null)
                return (false, "Gift not found.");

            if (await _repository.Gifts.AnyAsync(g => g.Name == giftUpdate.Name && g.IdGift != id))
                return (false, "Name is already taken");

            FluentValidation.Results.ValidationResult validationResult = await _validationGift.ValidateAsync(giftUpdate);
            if (!validationResult.IsValid)
                return (false, validationResult.Errors.First().ErrorMessage);

            _mapper.Map(giftUpdate, existingGift);

            if (giftUpdate.Image != null && giftUpdate.Image.Length > 0)
                existingGift.Image = await SaveImage(giftUpdate.Image) ?? existingGift.Image;

            _repository.Gifts.Update(existingGift);
            await _repository.SaveChangeAsync();

            return (true, null);
        }

    }
}
