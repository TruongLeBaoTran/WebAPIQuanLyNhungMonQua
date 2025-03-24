using AutoMapper;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;
using WebAPI_GiftManagement_BaoTran.Validators;

namespace WebAPI_GiftManagement_BaoTran.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllUsers();
        Task<(bool Success, string ErrorMessage, UserResponse)> GetSingleUser(string usernameClaim, string userName);
        Task<(bool Success, string ErrorMessage)> PutUser(string usernameClaim, int id, UserRequest userUpdate);
        Task<(bool Success, string ErrorMessage)> DeleteUser(string usernameClaim, int id);
        Task<(bool Success, string ErrorMessage)> GiveCoins(int idUser, int Coins);

    }

    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserValidator _userValidator;
        private readonly IRepositoryWrapper _repository;



        public UserService(IMapper mapper, UserValidator userValidator, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _userValidator = userValidator;
            _repository = repository;
        }

        public async Task<(bool Success, string ErrorMessage)> GiveCoins(int idUser, int coins)
        {
            User existingUser = await _repository.Users.GetSingleAsync(g => g.Id == idUser);
            if (existingUser == null)
                return (false, "User not found.");

            existingUser.Coin += coins;

            _repository.Users.Update(existingUser);
            await _repository.SaveChangeAsync();

            return (true, null);
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsers()
        {
            IEnumerable<User> users = await _repository.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        public async Task<(bool Success, string ErrorMessage, UserResponse)> GetSingleUser(string usernameClaim, string userName)
        {
            User user = await _repository.Users.GetSingleAsync(x => x.Username == userName);
            if (user == null)
            {
                return (false, "User not found", null);
            }
            if (user.Username != usernameClaim)
                return (false, "You do not have the right to edit other people's accounts.", null);

            return (true, "Success", _mapper.Map<UserResponse>(user));
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

        public async Task<(bool Success, string ErrorMessage)> PutUser(string usernameClaim, int id, UserRequest userUpdate)
        {

            User existingUser = await _repository.Users.GetSingleAsync(g => g.Id == id);
            if (existingUser == null)
                return (false, "PromoGift not found.");

            if (existingUser.Username != usernameClaim)
                return (false, "You do not have the right to edit other people's accounts.");

            if (!await _userValidator.IsNameNotExist(userUpdate.Username)) return (false, "Name already exists");

            if (!await _userValidator.IsEmailNotExist(userUpdate.Email)) return (false, "Email already exists");

            if (!await _userValidator.IsEmailNotExist(userUpdate.Phone)) return (false, "Phone already exists");

            FluentValidation.Results.ValidationResult validationResult = await _userValidator.ValidateAsync(userUpdate);
            if (!validationResult.IsValid)
                return (false, validationResult.Errors.First().ErrorMessage);

            _mapper.Map(userUpdate, existingUser);

            _repository.Users.Update(existingUser);
            await _repository.SaveChangeAsync();

            return (true, null);
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteUser(string usernameClaim, int id)
        {

            User? existingUser = await _repository.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (existingUser == null)
                return (false, "User not found.");

            if (existingUser.Username != usernameClaim)
                return (false, "You do not have the right to edit other people's accounts.");

            _repository.Users.Delete(existingUser);
            await _repository.SaveChangeAsync();

            return (true, null);
        }


    }
}
