using Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User
{
    public interface IUserService
    {
        Task<Response<string>> Login(string username);
    }
}
