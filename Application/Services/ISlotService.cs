using Application.Models;
using Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ISlotService
    {
        Task<Response<SpinResultResponse>> Spin(string userId, decimal betAmount);
        public string GenerateSpinResult();
        public decimal CalculateWinAmount(string result, decimal betAmount);
    }
}
