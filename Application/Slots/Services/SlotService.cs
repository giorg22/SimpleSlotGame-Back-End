using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Shared;
using System.Diagnostics.Contracts;
using static Application.Shared.ReponseExtensions;
using Mapster;
using Application.Slots.Interfaces;
using Application.Slots.Models;
using Application.Slots.Responses;

namespace Application.Slots.Services
{
    public class SlotService : ISlotService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISpinResultRepository _spinResultRepository;
        private readonly ITransactionRepository _transactionRepository;

        public SlotService(
            IUserRepository userRepository,
            ISpinResultRepository spinResultRepository,
            ITransactionRepository transactionRepository)
        {
            _userRepository = userRepository;
            _spinResultRepository = spinResultRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<Response<SpinResultResponse>> Spin(string userId, decimal betAmount)
        {
            if (betAmount <= 0)
            {
                return Fail<SpinResultResponse>(ErrorCode.NonPositiveBetAmount, "Bet amount should be positive");
            }

            var user = await _userRepository.GetById(userId);

            if (user == null)
            {
                return Fail<SpinResultResponse>(ErrorCode.UserNotFound, "User not found");
            }

            if (user.Balance < betAmount)
            {
                return Fail<SpinResultResponse>(ErrorCode.InsufficientBalance, "Insufficient balance");
            }

            user.Balance -= betAmount;
            await _userRepository.Update(user);

            var betTransaction = new Transaction
            {
                UserId = userId,
                Amount = betAmount,
                Date = DateTime.UtcNow.AddHours(-4),
                Type = "Bet"
            };
            await _transactionRepository.Add(betTransaction);

            var result = GenerateSpinResult();
            var winAmount = CalculateWinAmount(result, betAmount);

            if (winAmount > 0)
            {
                user.Balance += winAmount;
                await _userRepository.Update(user);

                var transaction = new Transaction
                {
                    UserId = userId,
                    Amount = winAmount,
                    Date = DateTime.UtcNow.AddHours(-4),
                    Type = "Win"
                };
                await _transactionRepository.Add(transaction);
            }

            var spinResult = new SpinResult
            {
                UserId = userId,
                Result = result,
                WinAmount = winAmount
            };
            await _spinResultRepository.Add(spinResult);

            var spinResultResponse = spinResult.Adapt<SpinResultResponse>();
            spinResultResponse.CurrentBalance = user.Balance;

            return Ok(spinResultResponse);
        }

        public string GenerateSpinResult()
        {
            var random = new Random();
            var reels = new List<string[]>
            {
                new[] { "Cherry", "Lemon", "Orange", "Bell", "Seven" },
                new[] { "Cherry", "Lemon", "Orange", "Bell", "Seven" },
                new[] { "Cherry", "Lemon", "Orange", "Bell", "Seven" }
            };

            var spinResult = new StringBuilder();
            foreach (var reel in reels)
            {
                var symbol = reel[random.Next(reel.Length)];
                spinResult.Append(symbol + " ");
            }

            return spinResult.ToString().TrimEnd();
        }

        public decimal CalculateWinAmount(string result, decimal betAmount)
        {
            var paylines = new List<Payline>
            {
                new Payline { Symbols = new List<Symbol> { Symbol.Cherry, Symbol.Cherry, Symbol.Cherry }, Multiplier = 10m },
                new Payline { Symbols = new List<Symbol> { Symbol.Seven, Symbol.Seven, Symbol.Seven }, Multiplier = 50m },
                new Payline { Symbols = new List<Symbol> { Symbol.Bell, Symbol.Bell, Symbol.Bell }, Multiplier = 20m }
            };

            var symbols = result.Split(' ').Select(s => Enum.Parse<Symbol>(s)).ToList();

            foreach (var payline in paylines)
            {
                if (symbols.SequenceEqual(payline.Symbols))
                {
                    return betAmount * payline.Multiplier;
                }
            }

            return 0m;
        }
    }
}
