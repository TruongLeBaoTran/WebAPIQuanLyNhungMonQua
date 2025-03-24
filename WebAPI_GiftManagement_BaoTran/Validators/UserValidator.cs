using FluentValidation;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;

namespace WebAPI_GiftManagement_BaoTran.Validators
{
    public class UserValidator : AbstractValidator<UserRequest>
    {
        private readonly IRepositoryWrapper _repository;
        public UserValidator(IRepositoryWrapper repositoryWrapper)
        {
            _repository = repositoryWrapper;

            RuleFor(user => user.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Matches(@"^(?=.*[a-zA-Z])(?=.*\d).{8,}$").WithMessage("Username must be at least 8 characters long and contain both letters and numbers.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,30}$").WithMessage("Password must be between 8 and 30 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(user => user.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^(?:\+84|0)\d{9,10}$").WithMessage("Invalid phone number format. Phone number must be 10 or 11 digits and start with 0 or +84.");
        }

        public async Task<bool> IsHaveNotAccess(int idUser, string usernameClaim)
        {
            User existingUser = await _repository.Users.GetSingleAsync(g => g.Id == idUser);
            if (existingUser.Username != usernameClaim)
                return true;
            return false;
        }


        public async Task<int> IsEnoughCoin(IEnumerable<Cart> listGiftInCart, int idUser)
        {
            int totalCoinSum = 0;
            foreach (Cart cart in listGiftInCart)
            {
                Gift gift = await _repository.Gifts.GetSingleAsync(x => x.IdGift == cart.IdMainGift);
                totalCoinSum += (int)gift.Coin;
            }

            User users = await _repository.Users.GetSingleAsync(x => x.Id == idUser);
            if (users.Coin >= totalCoinSum)
            {
                return totalCoinSum;
            }
            return 0;
        }

        public async Task<bool> IsNameNotExist(string userName)
        {
            if (await _repository.Users.AnyAsync(u => u.Username == userName))
                return false;
            return true;
        }

        public async Task<bool> IsEmailNotExist(string email)
        {
            if (await _repository.Users.AnyAsync(u => u.Username == email))
                return false;
            return true;
        }

        public async Task<bool> IsPhoneNotExist(string phone)
        {
            if (await _repository.Users.AnyAsync(u => u.Username == phone))
                return false;
            return true;
        }
    }
}
