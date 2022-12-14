using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface IScreeningRepository : IRepository<Screening>
    {

    }
    public class ScreeningRepository : RepositoryBase<Screening>, IScreeningRepository
    {
        public ScreeningRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
