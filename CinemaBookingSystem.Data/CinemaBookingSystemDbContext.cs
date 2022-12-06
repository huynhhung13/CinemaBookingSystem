﻿using CinemaBookingSystem.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Data
{
    public class CinemaBookingSystemDbContext : DbContext
    {
        public CinemaBookingSystemDbContext() {}
        public CinemaBookingSystemDbContext(DbContextOptions<CinemaBookingSystemDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=PTS;Integrated Security=True");
            }
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Carousel> Carousels { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Screening> Screenings { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<SupportOnline> SupportOnlines { get; set; }
        public DbSet<Theatre> Theatres { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VisitorStatistic> VisitorStatistics { get; set; }
    }
}
