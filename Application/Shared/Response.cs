using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared
{
    public class Response<T>
    {
        public bool Success { get; set; } = true;
        public T? Data { get; set; }
        public ErrorCode ErrorCode { get; set; }
        public string? Error { get; set; }
    }
}
