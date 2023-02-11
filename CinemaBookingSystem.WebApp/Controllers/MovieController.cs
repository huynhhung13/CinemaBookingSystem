﻿using AspNetCoreHero.ToastNotification.Abstractions;
using CinemaBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CinemaBookingSystem.WebApp.Controllers
{
    public class MovieController : Controller
    {
        private Uri _baseUrl = new Uri("https://localhost:44322/api/");
        private HttpClient _client;
        private const string APIKEY = "movienew";
        private readonly INotyfService _notyf;

        public MovieController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("CBSToken", APIKEY);
            _notyf = notyf;
        }
        public IActionResult Index()
        {
            IEnumerable<MovieViewModel> movies = GetMovieListRequest();
            return View(movies);
        }
        public IActionResult ComingSoon()
        {
            IEnumerable<MovieViewModel> movies = GetMovieListRequest();
            return View(movies.Where(x => x.ReleaseDate >= DateTime.UtcNow).ToList());
        }
        public IActionResult NowShowing()
        {
            IEnumerable<MovieViewModel> movies = GetMovieListRequest();
            return View(movies.Where(x => x.ReleaseDate <= DateTime.UtcNow).ToList());
        }
        public IActionResult TrailerWatch(int id)
        {
            MovieViewModel movie = GetMovieDetailsRequest(id);
            return View(movie);
        }
        public IActionResult Details(int id)
        {
            MovieViewModel movie = GetMovieDetailsRequest(id);
            IEnumerable<MovieViewModel> movies = GetMovieListRequest();
            movies = movies.Where(x => x.MovieId != movie.MovieId);
            ViewBag.Movies = movies;
            IEnumerable<CommentViewModel> comments = GetCommentListRequest(id);
            movie.Comments = comments;
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Comment(string content, int movieId)
        {
            if (String.IsNullOrEmpty(content))
            {
                _notyf.Information("Hãy điền bình luận của bạn vào nhé", 4);
            }
            else
            {
                CommentViewModel comment = new CommentViewModel()
                {
                    CommentedAt = DateTime.Now,
                    CommentedBy = (int)HttpContext.Session.GetInt32("_clientid"),
                    Content = content,
                    MovieId = movieId,
                };
                HttpResponseMessage response = CreateCommentRequest(comment);
                if (response.IsSuccessStatusCode)
                {
                    _notyf.Success($"Bình luận thành công", 3);
                    return RedirectToAction("Details", "Movie", new { id = movieId });
                }
                else
                {
                    _notyf.Error("Không thể thực hiện do lỗi server", 4);
                    Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            return RedirectToAction("Details", "Movie", new { id = movieId });
        }

        private MovieViewModel GetMovieDetailsRequest(int id)
        {
            MovieViewModel movie = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"movie/getsingle/{id}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);
            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                movie = JsonConvert.DeserializeObject<MovieViewModel>(body);
            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return movie;
        }

        private IEnumerable<MovieViewModel> GetMovieListRequest()
        {
            IEnumerable<MovieViewModel> list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "movie/getall");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);
            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<MovieViewModel>>(body);

            }
            else
            {
                _notyf.Error("Không thể lấy thông tin do lỗi server");
                _notyf.Error($"Status code: {(int)response.StatusCode}, Message: {response.ReasonPhrase}");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return list;
        }
        public HttpResponseMessage CreateCommentRequest(CommentViewModel comment)
        {
            string data = JsonConvert.SerializeObject(comment);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + "comment/create");
            request.Method = HttpMethod.Post;
            request.Headers.Add("CBSToken", APIKEY);
            request.Content = content;

            return _client.SendAsync(request).Result;
        }

        private IEnumerable<CommentViewModel> GetCommentListRequest(int id)
        {
            IEnumerable<CommentViewModel> list = null;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseUrl + $"comment/getall/{id}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("CBSToken", APIKEY);
            HttpResponseMessage response = _client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<CommentViewModel>>(body);

            }
            else
            {
                _notyf.Error("Không thể lấy thông tin bình luận do lỗi server");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return list.OrderBy(x => x.CommentedAt);
        }
    }
}