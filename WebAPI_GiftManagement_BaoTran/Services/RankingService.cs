using AutoMapper;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Repository;

namespace WebAPI_GiftManagement_BaoTran.Services
{
    public interface IRankingService
    {
        Task<(bool Success, string ErrorMessage)> Ranking(RankingRequest rankingNew);
        Task<(bool Success, string ErrorMessage)> Approved(int idRanking);
        Task<(string Message, IEnumerable<RankingUserResponse>)> GetRankingByTime(int month, int year, int pageIndex);
    }

    public class RankingService : IRankingService
    {
        public readonly IMapper mapper;
        public readonly IRepositoryWrapper repository;

        public RankingService(IRepositoryWrapper repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }


        int pageSize = 2;
        public async Task<(string Message, IEnumerable<RankingUserResponse>)> GetRankingByTime(int month, int year, int pageIndex)
        {
            Ranking ranking = await repository.Rankings.GetSingleAsync(x => x.Month == month && x.Year == year);
            if (ranking == null) return ("Ranking not exist", null);

            IEnumerable<RankingUser> rankingUsers = await repository.RankingUser.GetAllAsync(x => x.IdRanking == ranking.IdRanking);

            // Lấy danh sách giao dịch của người dùng trong bảng Transaction
            IEnumerable<int> userIds = rankingUsers.Select(ru => ru.IdUser).Distinct();
            IEnumerable<Transaction> transactions = await repository.Transactions
                .GetAllAsync(t => userIds.Contains(t.IdUser));

            // Sắp xếp danh sách theo Point giảm dần, nếu Point bằng nhau thì xét TransactionTime gần nhất
            IEnumerable<RankingUser> sortedRankingUsers = rankingUsers
                .Select(ru => new
                {
                    RankingUser = ru,
                    LastTransactionTime = transactions
                        .Where(t => t.IdUser == ru.IdUser)
                        .OrderByDescending(t => t.TransactionTime)
                        .FirstOrDefault()?.TransactionTime
                })
                .OrderByDescending(ru => ru.RankingUser.Point)
                .ThenBy(ru => ru.LastTransactionTime)
                .Select(ru => ru.RankingUser);

            //Phân trang
            IEnumerable<RankingUser> pagedRankingUsers = sortedRankingUsers
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return ("Success", mapper.Map<IEnumerable<RankingUserResponse>>(pagedRankingUsers));
        }


        public async Task<(bool Success, string ErrorMessage)> Ranking(RankingRequest rankingNew)
        {
            Ranking? existingRanking = await repository.Rankings.GetSingleAsync(g => g.Month == rankingNew.Month && g.Year == rankingNew.Year);
            RankingUser existingRankingUser = null;

            //TH: Đã có bxh tháng mới
            if (existingRanking != null)
            {
                existingRankingUser = await repository.RankingUser.GetSingleAsync(g => g.IdRanking == existingRanking.IdRanking && g.IdUser == rankingNew.IdUser);

                if (existingRankingUser != null) //User đã có trong bxh
                {
                    existingRankingUser.Point += rankingNew.Point;
                    repository.RankingUser.Update(existingRankingUser);
                }
                else
                {
                    RankingUser rankUserNew = new() //User chưa có trong bxh
                    {
                        IdRanking = existingRanking.IdRanking,
                        IdUser = rankingNew.IdUser,
                        Point = rankingNew.Point,
                    };
                    repository.RankingUser.Create(rankUserNew);
                }
                await repository.SaveChangeAsync();
            }
            else
            {
                //TH: Chưa có bxh tháng mới
                Ranking ranking = new()
                {
                    Month = rankingNew.Month,
                    Year = rankingNew.Year,
                };

                repository.Rankings.Create(ranking);
                await repository.SaveChangeAsync();

                RankingUser rankingUser = new()
                {
                    IdRanking = ranking.IdRanking,
                    IdUser = rankingNew.IdUser,
                    Point = rankingNew.Point,
                };

                repository.RankingUser.Create(rankingUser);
                await repository.SaveChangeAsync();
            }
            return (true, null);
        }



        public async Task<(bool Success, string ErrorMessage)> Approved(int idRanking)
        {

            Ranking? existingRanking = await repository.Rankings.GetSingleAsync(g => g.IdRanking == idRanking);
            if (existingRanking == null)
                return (true, "Ranking not existing");

            existingRanking.ApprovedDate = DateTime.Now;
            existingRanking.IsApproved = true;

            repository.Rankings.Update(existingRanking);
            await repository.SaveChangeAsync();

            return (true, null);

        }
    }

}
