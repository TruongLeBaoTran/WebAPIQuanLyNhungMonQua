using AutoMapper;
using Hangfire;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;
using WebAPI_GiftManagement_BaoTran.Validators;

namespace WebAPI_GiftManagement_BaoTran.Services
{
    public interface ITransactionService
    {
        Task<(bool Success, string ErrorMessage, IEnumerable<TransactionResponse>)> GetPurchaseHistory(string usernameClaim, int idUser);
        Task<(bool Success, string ErrorMessage, IEnumerable<IEnumerable<TransactionResponse>>)> GetWinningGiftHistory(int idUser = 0, int page = 1);
        Task<(bool Success, string ErrorMessage)> Purchase(string usernameClaim, int idUser);
        Task<(bool Success, string ErrorMessage)> AddGiftWhenPromoGiftAvailability(TransactionDetailRequest giftToGive);
        Task<(bool Success, string ErrorMessage)> AddGiftWhenPromoGiftRunOut(TransactionDetailRequest giftToGive);
        Task AutoGiftDistribution(int idMainGift, int quantity, bool optionWhenPromoGiftAvailability);
        string ScheduleGiftDistribution(int idGift, int quantity, DateTime distributionTime, bool optionWhenPromoGiftAvailability);
        bool CancelScheduledGiftDistribution(string jobId);
        Task<int> CreateTransaction(int totalCoinSum, int idUser);
        Task<bool> AddMainGift(int idTransaction, TransactionDetailRequest mainGift);
        Task<bool> AddPromoGift(int idTransaction, TransactionDetailRequest mainGift);

    }
    public class TransactionService : ITransactionService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly GiftValidator _validatorGift;
        private readonly UserValidator _validatorUser;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRankingService _rankingService;

        public static int PAGESIZE { get; set; } = 2;

        public TransactionService(IRepositoryWrapper repository, GiftValidator validatorGift, UserValidator validatorUser, ICartService cartService, IMapper mapper, IBackgroundJobClient backgroundJobClient, IRankingService rankingService)
        {
            _repository = repository;
            _validatorGift = validatorGift;
            _validatorUser = validatorUser;
            _cartService = cartService;
            _mapper = mapper;
            _backgroundJobClient = backgroundJobClient;
            _rankingService = rankingService;
        }

        /*-----------------------------------------------------------------------
        ----Admin phát quà chính khi còn quà khuyến mãi (phát quà chính + km)----
        -------------------------------------------------------------------------*/
        //Trường hợp trong list quà KM của quà chính này có 1 quà bị hết số lượng thì quà chính cũng không được phát --> Chỉ phát khi đủ số lượng như quy định
        public async Task<(bool Success, string ErrorMessage)> AddGiftWhenPromoGiftAvailability(TransactionDetailRequest giftToGive)
        {
            //Tất cả quà khuyến mãi của quà chính đó còn hàng và số lượng của quà chính vẫn còn trong kho
            if (await _validatorGift.IsQuantityPromoGiftNotAvailable(giftToGive.IdMainGift, giftToGive.Quantity))
                return (false, "Gifts are not available");

            int idTransaction = await CreateTransaction(0, giftToGive.IdUser);
            await AddMainGift(idTransaction, giftToGive);
            await AddPromoGift(idTransaction, giftToGive);

            await _repository.SaveChangeAsync();

            return (true, "Success");
        }


        /*------------------------------------------------------------------------
        ------Admin phát quà (khi hết quà khuyến mãi)-----------------------------
        -------------------------------------------------------------------------*/
        public async Task<(bool Success, string ErrorMessage)> AddGiftWhenPromoGiftRunOut(TransactionDetailRequest giftToGive)
        {
            //Kiểm tra quà KM thật sự hết và số lượng của quà chính còn đủ như số lượng muốn phát không
            if (!await _validatorGift.IsPromoGiftRunOut(giftToGive.IdMainGift))
                return (false, "Main gift still has promotional gifts");

            int idTransaction = await CreateTransaction(0, giftToGive.IdUser);
            await AddMainGift(idTransaction, giftToGive);

            await _repository.SaveChangeAsync();
            return (true, "Success");
        }


        /*-------------------------------------------------------------------------------------
        ------Tự động phát quà theo ngày được chỉ định sẵn, phát đến khi hết quà thì dừng------
        --------------------------------------------------------------------------------------*/

        public async Task AutoGiftDistribution(int idMainGift, int quantity, bool optionWhenPromoGiftAvailability)
        {

            IEnumerable<User> users = await _repository.Users.GetAllAsync();

            foreach (User user in users)
            {
                TransactionDetailRequest GiftToGive = new()
                {
                    IdMainGift = idMainGift,
                    IdUser = user.Id,
                    Quantity = quantity // Số lượng quà phát, tùy chỉnh theo yêu cầu
                };

                if (optionWhenPromoGiftAvailability)
                    // Phát quà khi còn quà khuyến mãi
                    await AddGiftWhenPromoGiftAvailability(GiftToGive);
                else
                    // Phát quà khi hết quà khuyến mãi
                    await AddGiftWhenPromoGiftRunOut(GiftToGive);

            }
        }

        public string ScheduleGiftDistribution(int idGift, int quantity, DateTime distributionTime, bool optionWhenPromoGiftAvailability)
        {
            string jobId = _backgroundJobClient.Schedule(() => AutoGiftDistribution(idGift, quantity, optionWhenPromoGiftAvailability), distributionTime);
            return jobId;
        }

        public bool CancelScheduledGiftDistribution(string jobId)
        {
            bool deleted = _backgroundJobClient.Delete(jobId);
            return deleted;
        }

        //Thêm vào bảng Transaction
        public async Task<int> CreateTransaction(int totalCoinSum, int idUser)
        {
            Transaction tran = new()
            {
                IdUser = idUser,
                CoinTotal = totalCoinSum,
                TransactionTime = DateTime.Now,
                AccumulatedPoints = null,
            };
            int idTransaction = _repository.Transactions.CreateTransaction(tran);
            return idTransaction;
        }

        //Thêm quà chính vào bảng TransactionDetail
        public async Task<bool> AddMainGift(int idTransaction, TransactionDetailRequest mainGift)
        {
            if (!await _validatorGift.IsGiftQuantityAvailable(mainGift.IdMainGift, mainGift.Quantity)
                    || await _validatorGift.IsGiftExpired(mainGift.IdMainGift))
            {
                return false;
            }

            TransactionDetail mainGiftTransaction = new()
            {
                IdTransaction = idTransaction,
                IdGift = mainGift.IdMainGift,
                MainGift = null,
                Quantity = mainGift.Quantity,
            };
            _repository.TransactionDetails.Create(mainGiftTransaction);

            Gift mainGiftInStock = await _repository.Gifts.GetSingleAsync(wg => wg.IdGift == mainGift.IdMainGift);
            mainGiftInStock.RemainingQuantity -= mainGift.Quantity;
            _repository.Gifts.Update(mainGiftInStock);
            return true;
        }

        //Thêm quà khuyến mãi vào bảng TransactionDetail
        public async Task<bool> AddPromoGift(int idTransaction, TransactionDetailRequest mainGift)
        {
            IEnumerable<Promotion> promoGifts = await _repository.Promotions.GetAllAsync(x => x.IdMainGift == mainGift.IdMainGift);
            foreach (Promotion promoGift in promoGifts)
            {
                if (!await _validatorGift.IsGiftQuantityAvailable(promoGift.IdPromoGift, promoGift.Quantity * mainGift.Quantity)
                    || await _validatorGift.IsGiftExpired(promoGift.IdPromoGift))
                {
                    return false;
                }

                TransactionDetail promoGiftTransaction = new()
                {
                    IdTransaction = idTransaction,
                    IdGift = promoGift.IdPromoGift,
                    MainGift = promoGift.IdMainGift,
                    Quantity = promoGift.Quantity * mainGift.Quantity,
                };
                _repository.TransactionDetails.Create(promoGiftTransaction);

                Gift promoGiftInStock = await _repository.Gifts.GetSingleAsync(wg => wg.IdGift == promoGift.IdPromoGift);
                promoGiftInStock.RemainingQuantity -= promoGift.Quantity * mainGift.Quantity;
                _repository.Gifts.Update(promoGiftInStock);
            }
            return true;
        }


        /*------------------------------------------------------------------------------------
        ------ Mua hàng ----------------------------------------------------------------------
        -------------------------------------------------------------------------------------*/
        private static readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        public async Task<(bool Success, string ErrorMessage)> Purchase(string usernameClaim, int idUser)
        {
            if (await _validatorUser.IsHaveNotAccess(idUser, usernameClaim))
                return (false, "You have not access");

            IEnumerable<Cart> listGiftInCart = await _repository.Carts.GetAllAsync(x => x.IdUser == idUser);
            if (!listGiftInCart.Any())
                return (false, "There are no products in the cart");

            int totalCoinSum = await _validatorUser.IsEnoughCoin(listGiftInCart, idUser);
            if (totalCoinSum == 0)
                return (false, "You don't have enough coins");

            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await _repository.BeginTransactionAsync())
            {
                await _semaphoreSlim.WaitAsync();
                try
                {
                    int idTransaction = await CreateTransaction(totalCoinSum, idUser);
                    int? accumulatedPoints = 0;

                    // Mua quà
                    foreach (Cart mainGift in listGiftInCart)
                    {
                        TransactionDetailRequest detail = new()
                        {
                            IdUser = idUser,
                            IdMainGift = mainGift.IdMainGift,
                            Quantity = mainGift.Quantity,
                        };

                        if (!await AddMainGift(idTransaction, detail))
                        {
                            await transaction.RollbackAsync();
                            return (false, "Failed to add main gift");
                        }


                        if (!await AddPromoGift(idTransaction, detail))
                        {
                            await transaction.RollbackAsync();
                            return (false, "Failed to add main gift");
                        }

                        //Tính điểm tích lũy
                        Gift gift = await _repository.Gifts.GetSingleAsync(x => x.IdGift == mainGift.IdMainGift);
                        if (gift.AccumulatedPoints != null)
                            accumulatedPoints += gift.AccumulatedPoints * mainGift.Quantity;

                    }

                    // Xóa tất cả quà trong giỏ hàng
                    await _cartService.DeleteAllGiftInCart(usernameClaim, idUser);

                    // Cập nhật lại tiền của user
                    User user = await _repository.Users.GetSingleAsync(wg => wg.Id == idUser);
                    user.Coin -= totalCoinSum;
                    _repository.Users.Update(user);

                    //Cập nhật điểm tích lũy
                    Transaction tran = await _repository.Transactions.GetSingleAsync(x => x.IdTransaction == idTransaction);
                    tran.AccumulatedPoints = accumulatedPoints;

                    //Xếp hạng
                    RankingRequest rankingRequest = new()
                    {
                        Month = tran.TransactionTime.Month,
                        Year = tran.TransactionTime.Year,
                        IdUser = idUser,
                        Point = accumulatedPoints ?? 0
                    };
                    await _rankingService.Ranking(rankingRequest);

                    await _repository.SaveChangeAsync();

                    await transaction.CommitAsync();
                    return (true, "Successful purchase");
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            }
        }


        /*-------------------------------------------------------------------------------------
        ------Xem lịch sử mua quà-------------------------------------------------------------
        --------------------------------------------------------------------------------------*/

        public async Task<(bool Success, string ErrorMessage, IEnumerable<TransactionResponse>)> GetPurchaseHistory(string usernameClaim, int idUser)
        {
            if (await _validatorUser.IsHaveNotAccess(idUser, usernameClaim))
                return (false, "You have not access", null);

            IEnumerable<Transaction> transactions = await _repository.Transactions.GetAllAsync(t => t.IdUser == idUser);
            List<TransactionResponse> historyResponses = new();

            foreach (Transaction transaction in transactions)
            {

                if (transaction.CoinTotal > 0)
                {
                    IEnumerable<TransactionDetail> transactionDetails = await _repository.TransactionDetails.GetAllAsync(td => td.IdTransaction == transaction.IdTransaction);

                    foreach (TransactionDetail detail in transactionDetails)
                    {
                        Gift? gift = await _repository.Gifts.GetSingleAsync(g => g.IdGift == detail.IdGift);

                        historyResponses.Add(new TransactionResponse
                        {
                            TransactionTime = transaction.TransactionTime,
                            CoinTotal = transaction.CoinTotal,
                            IdGift = detail.IdGift,
                            MainGift = detail.MainGift,
                            GiftName = gift?.Name,
                            GiftImage = gift?.Image,
                            GiftCoin = gift?.Coin,
                            Quantity = detail.Quantity
                        });
                    }
                }
            }

            return (true, "Success", historyResponses);
        }

        /*-------------------------------------------------------------------------------------
       ------Xem lịch sử trúng quà-------------------------------------------------------------
       --------------------------------------------------------------------------------------*/
        public async Task<(bool Success, string ErrorMessage, IEnumerable<IEnumerable<TransactionResponse>>)> GetWinningGiftHistory(int idUser = 0, int page = 1)
        {
            IEnumerable<Transaction> transactions;
            if (idUser == 0)
            {
                transactions = await _repository.Transactions.GetAllAsync();
            }
            else
            {
                transactions = await _repository.Transactions.GetAllAsync(t => t.IdUser == idUser);
            }

            List<TransactionResponse> historyResponses = new();

            foreach (Transaction transaction in transactions)
            {

                if (transaction.CoinTotal == 0)
                {
                    IEnumerable<TransactionDetail> transactionDetails = await _repository.TransactionDetails.GetAllAsync(td => td.IdTransaction == transaction.IdTransaction);

                    foreach (TransactionDetail detail in transactionDetails)
                    {
                        Gift? gift = await _repository.Gifts.GetSingleAsync(g => g.IdGift == detail.IdGift);

                        historyResponses.Add(new TransactionResponse
                        {
                            TransactionTime = transaction.TransactionTime,
                            CoinTotal = transaction.CoinTotal,
                            IdGift = detail.IdGift,
                            MainGift = detail.MainGift,
                            GiftName = gift?.Name,
                            GiftImage = gift?.Image,
                            GiftCoin = gift?.Coin,
                            Quantity = detail.Quantity
                        });
                    }
                }
            }

            List<List<TransactionResponse>> groupedResponses = historyResponses
                .GroupBy(r => r.TransactionTime)
                .Select(g => g.ToList())
                .ToList();

            IEnumerable<IEnumerable<TransactionResponse>> pagedResponses = groupedResponses
                .Skip((page - 1) * PAGESIZE)
                .Take(PAGESIZE);

            return (true, "Success", pagedResponses);
        }

    }
}
