using Microsoft.AspNetCore.SignalR;
using Domain.Entities;
using Application.Services;
using Application.Shared;
using Application.Models;

namespace API
{
    public class SlotHub : Hub
    {
        private readonly ISlotService _slotService;

        public SlotHub(ISlotService slotService)
        {
            _slotService = slotService;
        }

        public async Task<Response<SpinResultResponse>> Spin(string userId, decimal betAmount)
        {
            var result = await _slotService.Spin(userId, betAmount);
            await Clients.Caller.SendAsync("ReceiveSpinResult", result);
            return result;
        }
    }
}
