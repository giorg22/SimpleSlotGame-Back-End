using Application.Shared;
using Application.Slots.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Slots.Interfaces
{
    public interface ISlotService
    {
        Task<Response<SpinResultResponse>> Spin(string userId, decimal betAmount);
        public string GenerateSpinResult();
        public decimal CalculateWinAmount(string result, decimal betAmount);
    }
}
