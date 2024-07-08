using Microsoft.AspNetCore.SignalR;
using Domain.Entities;
using Application.Shared;
using Application.Slots.Interfaces;
using Application.Slots.Responses;

namespace API.SignalR
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
