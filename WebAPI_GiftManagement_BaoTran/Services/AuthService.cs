using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;
using WebAPI_GiftManagement_BaoTran.Validators;

namespace WebAPI_GiftManagement_BaoTran.Services
{
    public interface IAuthService
    {
        Task<Token> GenerateToken(User user);
        Task<(bool Success, string ErrorMessage, Token Token)> RenewToken(Token token);
        public string GenerateRefreshToken();
        Task<(bool Success, string ErrorMessage, Token Token)> LoginUser(Login model);
        Task<(bool Success, string ErrorMessage, Token Token)> LoginAdmin(Login model);
        Task<(bool Success, string ErrorMessage)> Register(UserRequest userNew);
        Task<(bool Success, string ErrorMessage)> CreateAdmin(UserRequest userNew);
    }

    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly AuthValidator _validator;
        private readonly UserValidator _userValidator;
        private readonly IRepositoryWrapper _repository;

        public AuthService(IMapper mapper, IConfiguration config, AuthValidator authValidator, IRepositoryWrapper repository, UserValidator validationRules)
        {
            _mapper = mapper;
            _config = config;
            _validator = authValidator;
            _repository = repository;
            _userValidator = validationRules;
        }

        //Đăng nhập cho user
        public async Task<(bool Success, string ErrorMessage, Token Token)> LoginUser(Login model)
        {

            User user = await _repository.Users.GetSingleAsync(p => p.Username == model.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                return (false, "Invalid username/password.", null);

            // Kiểm tra vai trò của người dùng
            bool isAdmin = await _repository.RoleUser.AnyAsync(r => r.IdUser == user.Id);
            if (isAdmin)
                return (false, "Not have permission to access.", null);

            // Cấp token
            Token token = await GenerateToken(user);
            return (true, null, token);
        }



        //Đăng nhập cho admin
        public async Task<(bool Success, string ErrorMessage, Token Token)> LoginAdmin(Login model)
        {

            User user = await _repository.Users.GetSingleAsync(p => p.Username == model.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                return (false, "Invalid username/password.", null);

            // Kiểm tra vai trò của admin
            bool isAdmin = await _repository.RoleUser.AnyAsync(r => r.IdUser == user.Id);
            if (!isAdmin)
                return (false, "User does not have admin privileges.", null);

            // Cấp token
            Token token = await GenerateToken(user);
            return (true, null, token);

        }



        //Đăng ký của user
        public async Task<(bool Success, string ErrorMessage)> Register(UserRequest userNew)
        {
            if (!await _userValidator.IsNameNotExist(userNew.Username)) return (false, "Name already exists");

            if (!await _userValidator.IsEmailNotExist(userNew.Email)) return (false, "Email already exists");

            if (!await _userValidator.IsEmailNotExist(userNew.Phone)) return (false, "Phone already exists");

            FluentValidation.Results.ValidationResult validationResult = await _userValidator.ValidateAsync(userNew);
            if (!validationResult.IsValid)
                return (false, validationResult.Errors.First().ErrorMessage);

            User? user = _mapper.Map<User>(userNew);

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            user.Image = await SaveImage(userNew.Image);

            _repository.Users.Create(user);
            await _repository.SaveChangeAsync();

            return (true, null);
        }

        //Tạo ra 1 admin
        public async Task<(bool Success, string ErrorMessage)> CreateAdmin(UserRequest userNew)
        {

            if (!await _userValidator.IsNameNotExist(userNew.Username)) return (false, "Name already exists");

            if (!await _userValidator.IsEmailNotExist(userNew.Email)) return (false, "Email already exists");

            if (!await _userValidator.IsEmailNotExist(userNew.Phone)) return (false, "Phone already exists");

            FluentValidation.Results.ValidationResult validationResult = await _userValidator.ValidateAsync(userNew);
            if (!validationResult.IsValid)
                return (false, validationResult.Errors.First().ErrorMessage);

            User user = _mapper.Map<User>(userNew);

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            user.Image = await SaveImage(userNew.Image);

            _repository.Users.Create(user);
            await _repository.SaveChangeAsync();

            return (true, null);
        }

        public async Task<string> SaveImage(IFormFile imageFile)
        {
            // Ràng buộc định dạng hình ảnh
            string[] allowedMimeTypes = new[] { "image/png", "image/jpeg", "image/gif" };

            if (imageFile != null && imageFile.Length > 0)
            {

                if (imageFile.Length > 10000000) return "File too large"; // Kích thước tối đa là 10MB

                if (!allowedMimeTypes.Contains(imageFile.ContentType)) return "Invalid file type";

                // Lưu hình ảnh vào thư mục
                string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imageFile.FileName);
                string extension = Path.GetExtension(imageFile.FileName);
                string dest = Path.Combine(uploads, imageFile.FileName);
                int i = 1;

                // Kiểm tra trùng lặp tên file
                while (File.Exists(dest))
                {
                    dest = Path.Combine(uploads, $"{fileNameWithoutExtension}-{i}{extension}");
                    i++;
                }

                // Di chuyển file đến thư mục đích
                using (FileStream stream = new(dest, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return $"/images/{Path.GetFileName(dest)}";
            }
            else
            {
                return "";
            }
        }

        public async Task<Token> GenerateToken(User user)
        {
            JwtSecurityTokenHandler jwtTokenHandler = new();

            IConfigurationSection appSetting = _config.GetSection("AppSettings");

            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(appSetting["SecretKey"]);

            SecurityTokenDescriptor tokenDescription = new()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes),
                                                                SecurityAlgorithms.HmacSha256Signature)

            };

            SecurityToken token = jwtTokenHandler.CreateToken(tokenDescription);
            string accessToken = jwtTokenHandler.WriteToken(token);
            string refreshToken = GenerateRefreshToken();

            //Save database
            RefreshToken reefreshTokenEntity = new()
            {
                IdUser = user.Id,
                Token = refreshToken,
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1),
            };

            _repository.RefreshTokens.Create(reefreshTokenEntity);
            await _repository.SaveChangeAsync();

            return new Token
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public string GenerateRefreshToken()
        {
            byte[] random = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }

        public async Task<(bool Success, string ErrorMessage, Token Token)> RenewToken(Token token)
        {
            // Validate the token
            FluentValidation.Results.ValidationResult validationResult = await _validator.ValidateAsync(token);
            if (!validationResult.IsValid)
            {
                return (false, validationResult.Errors.First().ErrorMessage, null);
            }

            // Get the stored refresh token
            RefreshToken? storedToken = await _repository.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token.RefreshToken);
            if (storedToken == null)
            {
                return (false, "Invalid refresh token.", null);
            }

            // Update the refresh token status
            storedToken.IsUsed = true;
            storedToken.IsRevoked = true;
            _repository.RefreshTokens.Update(storedToken);
            await _repository.SaveChangeAsync();

            // Generate a new access token and refresh token
            User? user = await _repository.Users.GetSingleAsync(u => u.Id == storedToken.IdUser);
            if (user == null)
            {
                return (false, "User not found.", null);
            }

            Token newToken = await GenerateToken(user);

            return (true, null, new Token
            {
                AccessToken = newToken.AccessToken,
                RefreshToken = newToken.RefreshToken,
            });
        }

    }


}
