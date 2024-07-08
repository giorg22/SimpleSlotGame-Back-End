using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared
{
    public static class ReponseExtensions
    {
        public static Response<T> Ok<T>()
        {
            return new Response<T>()
            {
                Success = true
            };
        }
        public static Response<T> Ok<T>(T t)
        {
            return new Response<T>()
            {
                Success = true,
                Data = t
            };
        }
        public static Response<T> Fail<T>()
        {
            return new Response<T>()
            {
                Success = false,
            };
        }
        public static Response<T> Fail<T>(ErrorCode errorCode)
        {
            return new Response<T>()
            {
                Success = false,
                ErrorCode = errorCode
            };
        }
        public static Response<T> Fail<T>(ErrorCode errorCode, string error)
        {
            return new Response<T>()
            {
                Success = false,
                ErrorCode = errorCode,
                Error = error
            };
        }
    }
}
