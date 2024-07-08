using Application.Shared;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Shared.ReponseExtensions;
using Domain.Entities;
using Application.Users.Interfaces;

namespace Application.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Response<string>> Login(string username)
        {
            var user = await _userRepository.GetByUsername(username);
            if (user != null)
            {
                return Ok(user.Id);
            }

            var newUser = new Domain.Entities.User()
            {
                Username = username,
                Balance = 10000
            };
            var id = await _userRepository.Add(newUser);

            return Ok(id);
        }
    }
}
