using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;

namespace WebAPI_GiftManagement_BaoTran.Validators
{
    public class AuthValidator : AbstractValidator<Token>
    {
        private readonly JwtSecurityTokenHandler _jwtTokenHandler;
        private readonly IConfiguration _configuration;
        private readonly MyDbContext _db; // Giả sử bạn có một DbContext để tương tác với cơ sở dữ liệu
        private readonly IRepositoryWrapper _repository;


        public AuthValidator(JwtSecurityTokenHandler jwtTokenHandler, IConfiguration configuration, MyDbContext db, IRepositoryWrapper repository)
        {
            _jwtTokenHandler = jwtTokenHandler;
            _configuration = configuration;
            _repository = repository;
            _db = db;

            RuleFor(token => token.AccessToken)
                .Must(IsAccessTokenFormatValid).WithMessage("Invalid access token")
                .Must(IsTokenAlgorithmValid).WithMessage("Invalid token algorithm")
                .Must(IsAccessTokenExpired).WithMessage("Access token has not yet expired");

            RuleFor(token => token.RefreshToken)
                .Must(DoesRefreshTokenExist).WithMessage("Refresh token does not exist")
                .Must(IsRefreshTokenUsedOrRevoked).WithMessage("Refresh token has been used or revoked");

            RuleFor(token => token)
                .Must(IsJwtIdMatching).WithMessage("Token doesn't match");

        }


        //Check 1: Kiểm tra định dạng của access token
        private bool IsAccessTokenFormatValid(string accessToken)
        {
            IConfigurationSection jwtSettings = _configuration.GetSection("AppSettings");
            byte[] key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

            TokenValidationParameters tokenValidParam = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };

            try
            {
                _jwtTokenHandler.ValidateToken(accessToken, tokenValidParam, out SecurityToken? validatedToken);
                return true; // Token is valid
            }
            catch
            {
                return false; // Token is invalid
            }
        }



        //Check 2: Kiểm tra thuật toán mã hóa của token khớp với thuật toán mà server đã sử dụng để ký token ko
        private bool IsTokenAlgorithmValid(string accessToken)
        {
            IConfigurationSection jwtSettings = _configuration.GetSection("AppSettings");
            byte[] key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

            TokenValidationParameters tokenValidParam = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(key), // Sử dụng khóa ký từ cài đặt JWT.
                ValidateLifetime = false
            };

            System.Security.Claims.ClaimsPrincipal tokenInVerification = _jwtTokenHandler.ValidateToken(accessToken, tokenValidParam, out SecurityToken? validatedToken);

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                return jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
            }

            return false; // Token không hợp lệ

        }


        //Check 3: Kiểm tra xem access token đã hết hạn chưa
        private bool IsAccessTokenExpired(string accessToken)
        {
            IConfigurationSection jwtSettings = _configuration.GetSection("AppSettings");
            byte[] key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

            TokenValidationParameters tokenValidParam = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(key), // Sử dụng khóa ký từ cài đặt JWT.
                ValidateLifetime = false
            };

            System.Security.Claims.ClaimsPrincipal tokenInVerification = _jwtTokenHandler.ValidateToken(accessToken, tokenValidParam, out SecurityToken? validatedToken);


            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                System.Security.Claims.Claim? utcExpireDateClaim = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp);
                if (utcExpireDateClaim != null)
                {
                    long utcExpireDate = long.Parse(jwtSecurityToken.Claims.FirstOrDefault(x =>
                                x.Type == JwtRegisteredClaimNames.Exp)?.Value ?? "0");

                    DateTime expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                    return expireDate <= DateTime.UtcNow;
                }
            }

            return true; // Nếu không có claim Exp thì coi như token đã hết hạn
        }

        private DateTime ConvertUnixTimeToDateTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
        }

        //Check 4: Kiểm tra sự tồn tại của refresh token trong cơ sở dữ liệu
        private bool DoesRefreshTokenExist(string refreshToken)
        {
            RefreshToken? storedToken = _db.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken);
            //RefreshToken? storedToken = _repository.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken);
            return storedToken != null; // Trả về true nếu refresh token tồn tại
        }

        //Check 5: Kiểm tra trạng thái của refresh token (đã được sử dụng hoặc đã bị thu hồi) 
        private bool IsRefreshTokenUsedOrRevoked(string refreshToken)
        {
            RefreshToken? storedToken = _db.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken);
            return !storedToken.IsUsed || !storedToken.IsRevoked; // Trả về true nếu đã được sử dụng hoặc đã bị thu hồi
        }


        ////Check 6: Kiểm tra JwtId của access token có khớp với JwtId trong refresh token không
        private bool IsJwtIdMatching(Token token)
        {
            IConfigurationSection jwtSettings = _configuration.GetSection("AppSettings");
            byte[] key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

            TokenValidationParameters tokenValidParam = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(key), // Sử dụng khóa ký từ cài đặt JWT.
                ValidateLifetime = false
            };

            System.Security.Claims.ClaimsPrincipal tokenInVerification = _jwtTokenHandler.ValidateToken(token.AccessToken, tokenValidParam, out SecurityToken? validatedToken);
            string? jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

            RefreshToken? storedToken = _db.RefreshTokens.FirstOrDefault(x => x.Token == token.RefreshToken);
            if (storedToken == null)
            {
                return false; // Refresh token không tồn tại
            }

            return storedToken.JwtId == jti; // Trả về true nếu JwtId khớp
        }


        //Kiểm tra đúng role mới cho đăng nhập
        //public bool IsUserRole(int role)
        //{
        //    if(role == 1)
        //    {
        //        return true;
        //    }    
        //    return false;
        //}
    }
}
