using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IReviewRepository : IRepositoryBase<Review>
    {
        Task<IEnumerable<Review>> GetAllWithUserAsync();
        Task<bool> HasUserReviewedAsync(int userId);
    }
}
