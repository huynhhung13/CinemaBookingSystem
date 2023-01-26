﻿namespace CinemaBookingSystem.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int RoleId { get; set; }
        public virtual RoleViewModel? Role { get; set; }
        public IEnumerable<CommentViewModel>? Comments { get; set; }
        public IEnumerable<BookingViewModel>? Bookings { get; set; }
    }
}