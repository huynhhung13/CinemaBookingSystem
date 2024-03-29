﻿using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;
using System.Net.WebSockets;

namespace CinemaBookingSystem.Service
{
    public interface IUserService
    {
        void Add(User user);

        void Update(User user);

        void Delete(int id);

        IEnumerable<User> GetAll();

        IEnumerable<User> GetByRole(int roleId);

        User GetByUsername(string username);

        User GetById(int id);

        bool Login(string username, string password);

        bool Signup(User user);

        bool ChangePassword(User user, string oldPassword, string newPassword);

        void SaveChanges();
    }

    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(User user)
        {
            _userRepository.Add(user);
        }

        public bool ChangePassword(User user, string oldPassword, string newPassword)
        {
            bool isValid = _userRepository.PasswordHashing(oldPassword) == user.Password;
            if (isValid)
            {
                user.Password = _userRepository.PasswordHashing(newPassword);
                _userRepository.Update(user);
                return true;
            }
            return false;
        }

        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetById(int id)
        {
            return _userRepository.GetSingleById(id);
        }

        public IEnumerable<User> GetByRole(int roleId)
        {
            return _userRepository.GetByRole(roleId);
        }

        public User GetByUsername(string username)
        {
            return _userRepository.GetByUsername(username);
        }

        public bool Login(string username, string password)
        {
            return _userRepository.Login(username, password);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public bool Signup(User user)
        {
            bool isValidUser = _userRepository.UsernameCheck(user.Username);
            if (isValidUser)
            {
                user.Password = _userRepository.PasswordHashing(user.Password);
                _userRepository.Add(user);
                SaveChanges();
                return isValidUser;
            }
            return isValidUser;
        }

        public void Update(User user)
        {
            _userRepository.Update(user);
        }
    }
}