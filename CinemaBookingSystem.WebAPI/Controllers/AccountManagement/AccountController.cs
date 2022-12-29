﻿using AutoMapper;
using CinemaBookingSystem.Model.Models;
using CinemaBookingSystem.Service;
using CinemaBookingSystem.WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.WebAPI.Controllers.AccountManagement
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public AccountController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("login")]
        public ActionResult Login([FromHeader, Required] string CinemaBookingSystemToken,string username, string password)
        {
            bool IsValid = _userService.Login(username, password);
            if (!IsValid) return BadRequest("Username or Password is incorrect!");
            else
            {
                return Ok("Account verified!");
            }
        }
        [HttpPost]
        [Route("signin")]
        public ActionResult Signin([FromHeader, Required] string CinemaBookingSystemToken, [FromBody] UserViewModel userViewModel)
        {
            var user = _mapper.Map<User>(userViewModel);
            bool IsSuccess = _userService.Signup(user);
            if (!IsSuccess) return BadRequest("Sign-in failed");
            else
            {
                _userService.SaveChanges();
                return Ok("Sign-in successful!");
            }

        }
    }
}