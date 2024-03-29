﻿using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface ICinemaRepository : IRepository<Cinema>
    {
        
    }
    public class CinemaRepository : RepositoryBase<Cinema>, ICinemaRepository
    {
        public CinemaRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
