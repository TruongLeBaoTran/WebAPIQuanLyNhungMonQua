using AutoMapper;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;
using WebAPI_GiftManagement_BaoTran.Validators;

namespace WebAPI_GiftManagement_BaoTran.Services
{
    public interface ICartService
    {
        Task<(bool Success, string ErrorMessage, IEnumerable<CartResponse>)> GetAllGiftInCart(string usernameClaim, int idUser);
        Task<(bool Success, string ErrorMesage)> AddGiftToCart(string usernameClaim, CartRequest cartRequest);
        Task<(bool Success, string ErrorMesage)> UpdateQuantityGiftToCart(string usernameClaim, int idCart, int QuantityRequest);
        Task<(bool Success, string ErrorMesage)> IncreaseOneGiftInCart(string usernameClaim, int idCartGift);
        Task<(bool Success, string ErrorMesage)> DecreaseOneGiftInCart(string usernameClaim, int idCartGift);
        Task<(bool Success, string ErrorMesage)> DeleteOneGiftInCart(string usernameClaim, int idCartGift);
        Task<(bool Success, string ErrorMesage)> DeleteAllGiftInCart(string usernameClaim, int idUser);

    }
    public class CartService : ICartService
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly GiftValidator _validationGift;
        private readonly UserValidator _validationUser;

        public CartService(MyDbContext dbContext, IMapper mapper, IRepositoryWrapper repositoryWrapper, GiftValidator validationGift, UserValidator validationUser)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _repository = repositoryWrapper;
            _validationGift = validationGift;
            _validationUser = validationUser;
        }

        /*-------------------------------------------------------------------------------------
       ------Xem tất cả quà trong giỏ hàng-----------------------------------------------------
       --------------------------------------------------------------------------------------*/
        public async Task<(bool Success, string ErrorMessage, IEnumerable<CartResponse>)> GetAllGiftInCart(string usernameClaim, int idUser)
        {
            if (await _validationUser.IsHaveNotAccess(idUser, usernameClaim)) return (false, "You have not access", null);

            // Lấy ra danh sách quà chính trong giỏ hàng của người dùng
            IEnumerable<Cart> mainGiftInCarts = await _repository.Carts.GetAllAsync(x => x.IdUser == idUser);

            List<CartResponse> cartViewModels = new();

            foreach (Cart mainGiftInCart in mainGiftInCarts)
            {
                // Lấy ra danh sách quà khuyến mãi của 1 quà chính
                IEnumerable<Promotion> promotionGift = await _repository.Promotions.GetAllAsync(x => x.IdMainGift == mainGiftInCart.IdMainGift);

                List<PromoGiftResponse> promoGiftResponses = new();

                // Cập nhật số lượng quà khuyến mãi hiển thị trong giỏ
                foreach (Promotion promotion in promotionGift)
                {
                    Gift promoGift = await _repository.Gifts.GetSingleAsync(x => x.IdGift == promotion.IdPromoGift); // lấy quà km đó từ kho 

                    int adjustedQuantity = promotion.Quantity * mainGiftInCart.Quantity > promoGift.RemainingQuantity
                        ? promoGift.RemainingQuantity
                        : promotion.Quantity * mainGiftInCart.Quantity;

                    promoGiftResponses.Add(new PromoGiftResponse
                    {
                        IdPromoGift = promotion.IdPromoGift,
                        GiftName = promoGift.Name,
                        GiftImage = promoGift.Image,
                        Quantity = adjustedQuantity
                    });
                }

                Gift? mainGift = await _repository.Gifts.GetSingleAsync(g => g.IdGift == mainGiftInCart.IdMainGift);
                CartResponse response = new()
                {
                    IdMainGift = mainGiftInCart.IdMainGift,
                    GiftName = mainGift.Name,
                    GiftImage = mainGift.Image,
                    GiftCoin = mainGift.Coin,
                    Quantity = mainGiftInCart.Quantity,
                    ListPromoGift = promoGiftResponses
                };

                cartViewModels.Add(response);
            }

            return (true, "Success", cartViewModels);
        }


        /*-------------------------------------------------------------------------------------
        ------Thêm quà vào giỏ hàng------------------------------------------------------------
        --------------------------------------------------------------------------------------*/
        public async Task<bool> AddGiftToCart(CartRequest cartRequest, bool isUpdate)
        {

            if (await _validationGift.IsGiftQuantityAvailable(cartRequest.IdMainGift, cartRequest.Quantity))
            {
                Cart cartGift = _mapper.Map<Cart>(cartRequest);

                //Kiểm tra quà chính tồn tại trong giỏ chưa
                if (!await _validationGift.IsGiftExistInCart(cartRequest.IdUser, cartRequest.IdMainGift))
                {
                    _repository.Carts.Create(cartGift);
                }
                else
                {
                    //Cập nhật số lượng của quà đó trong giỏ
                    Cart giftInCart = await _repository.Carts.GetSingleAsync(wg => wg.IdUser == cartRequest.IdUser && wg.IdMainGift == cartRequest.IdMainGift);
                    if (isUpdate)
                    {
                        giftInCart.Quantity = cartRequest.Quantity;
                    }
                    else
                    {
                        giftInCart.Quantity += cartRequest.Quantity;
                    }

                    _repository.Carts.Update(giftInCart);
                }

                await _repository.SaveChangeAsync();
                return true;
            }
            return false;
        }


        public async Task<(bool Success, string ErrorMesage)> AddGiftToCart(string usernameClaim, CartRequest cartRequest)
        {
            if (await _validationUser.IsHaveNotAccess(cartRequest.IdUser, usernameClaim)) return (false, "You have not access");

            if (!await AddGiftToCart(cartRequest, false))
                return (false, "Add failure");
            return (true, "Add success");
        }

        /*-------------------------------------------------------------------------------------
         ------Sửa số lượng quà trong giỏ hàng-------------------------------------------------
         --------------------------------------------------------------------------------------*/
        public async Task<(bool Success, string ErrorMesage)> UpdateQuantityGiftToCart(string usernameClaim, int idCart, int QuantityRequest)
        {
            Cart giftInCart = await _repository.Carts.GetSingleAsync(x => x.IdCart == idCart);

            if (await _validationUser.IsHaveNotAccess(giftInCart.IdUser, usernameClaim)) return (false, "You have not access");

            if (await _validationGift.IsGiftQuantityAvailable(giftInCart.IdMainGift, QuantityRequest))
            {
                CartRequest cartGift = _mapper.Map<CartRequest>(giftInCart);
                cartGift.Quantity = QuantityRequest;
                await AddGiftToCart(cartGift, true); //true= update

                await _repository.SaveChangeAsync();
                return (true, "Success");
            }
            return (false, "Failure");
        }

        //*-------------------------------------------------------------------------------------
        //------Tăng 1 quà trong giỏ hàng-Click dấu +--------------------------------------------
        //--------------------------------------------------------------------------------------*/
        public async Task<(bool Success, string ErrorMesage)> IncreaseOneGiftInCart(string usernameClaim, int idCartGift)
        {


            Cart giftInCart = await _repository.Carts.GetSingleAsync(wg => wg.IdCart == idCartGift);

            if (await _validationUser.IsHaveNotAccess(giftInCart.IdUser, usernameClaim)) return (false, "You have not access");

            if (giftInCart != null && await _validationGift.IsGiftQuantityAvailable(giftInCart.IdMainGift, giftInCart.Quantity + 1))
            {
                Cart cartGift = new()
                {
                    IdCart = idCartGift,
                    IdMainGift = giftInCart.IdMainGift,
                    IdUser = giftInCart.IdUser,
                    Quantity = 1
                };
                CartRequest cartRequest = _mapper.Map<CartRequest>(cartGift);
                await AddGiftToCart(cartRequest, false);

                await _repository.SaveChangeAsync();
                return (true, "Increate failure");
            }
            return (false, "Increate success");
        }

        // /*-------------------------------------------------------------------------------------
        //------Giảm 1 quà trong giỏ hàng-Click dấu +---------------------------------------------
        //--------------------------------------------------------------------------------------*/
        public async Task<(bool Success, string ErrorMesage)> DecreaseOneGiftInCart(string usernameClaim, int idCartGift)
        {
            Cart giftInCart = await _repository.Carts.GetSingleAsync(wg => wg.IdCart == idCartGift);

            if (await _validationUser.IsHaveNotAccess(giftInCart.IdUser, usernameClaim)) return (false, "You have not access");

            if (giftInCart != null && await _validationGift.IsGiftQuantityAvailable(giftInCart.IdMainGift, giftInCart.Quantity - 1))
            {
                if (giftInCart.Quantity > 1)
                {
                    Cart cartGift = new()
                    {
                        IdCart = idCartGift,
                        IdMainGift = giftInCart.IdMainGift,
                        IdUser = giftInCart.IdUser,
                        Quantity = -1
                    };
                    CartRequest cartRequest = _mapper.Map<CartRequest>(cartGift);
                    await AddGiftToCart(cartRequest, false);

                }
                else
                {
                    await DeleteOneGiftInCart(giftInCart);
                }
                await _repository.SaveChangeAsync();
                return (true, "Decrease success");

            }
            return (false, "Decrease failure");
        }


        // /*------------------------------------------------------------------------------------
        // ------Xóa 1 quà trong giỏ hàng---------------------------------------------------------
        // --------------------------------------------------------------------------------------*/

        public async Task<(bool Success, string ErrorMesage)> DeleteOneGiftInCart(string usernameClaim, int idCartGift)
        {

            Cart giftInCart = await _repository.Carts.GetSingleAsync(wg => wg.IdCart == idCartGift);

            if (await _validationUser.IsHaveNotAccess(giftInCart.IdUser, usernameClaim)) return (false, "You have not access");

            if (giftInCart != null && await DeleteOneGiftInCart(giftInCart))
            {
                return (true, "Delete success");
            }
            return (false, "Delete failure");

        }

        public async Task<bool> DeleteOneGiftInCart(Cart giftInCart)
        {
            //Kiểm tra quà chính tồn tại trong giỏ ko
            if (await _validationGift.IsGiftExistInCart(giftInCart.IdUser, giftInCart.IdMainGift))
            {
                _repository.Carts.Delete(giftInCart);
                await _repository.SaveChangeAsync();
                return true;
            }
            return false;

        }

        // /*------------------------------------------------------------------------------------
        // ------Xóa tất cả quà trong giỏ hàng---------------------------------------------------
        // --------------------------------------------------------------------------------------*/
        public async Task<(bool Success, string ErrorMesage)> DeleteAllGiftInCart(string usernameClaim, int idUser)
        {
            if (await _validationUser.IsHaveNotAccess(idUser, usernameClaim)) return (false, "You have not access");

            IEnumerable<Cart> listGiftInCart = await _repository.Carts.GetAllAsync(x => x.IdUser == idUser);
            if (listGiftInCart != null)
            {
                foreach (Cart cartGift in listGiftInCart)
                {
                    if (await DeleteOneGiftInCart(cartGift) == false)
                    {
                        return (false, "Delete failure");
                    }
                }
                await _repository.SaveChangeAsync();
                return (true, "Deleted successffuly");
            }
            return (false, "Cart have not gift");

        }

    }
}
