﻿namespace CinemaBookingSystem.ViewModels
{
    public class MovieViewModel
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public string Director { get; set; }
        public string Cast { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genres { get; set; }
        public int RunningTime { get; set; }
        public string Rated { get; set; }
        public string? TrailerURL { get; set; }
        public string? ThumpnailImg { get; set; }
        public string? BannerImg { get; set; }
        public string Description { get; set; }
        public IEnumerable<ScreeningViewModel>? Screenings { get; set; }
        public IEnumerable<CommentViewModel>? Comments { get; set; }
    }
}