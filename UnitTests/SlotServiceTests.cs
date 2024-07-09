﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Slots.Models;
using Application.Slots.Services;
using Application.Shared;
using Domain.Entities;
using Domain.Repositories;
using Moq;
using Xunit;
using Application.Slots.Interfaces;

namespace Application.Tests
{
    public class SlotServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ISpinResultRepository> _mockSpinResultRepository;
        private readonly Mock<ITransactionRepository> _mockTransactionRepository;
        private readonly SlotService _slotService;

        public SlotServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockSpinResultRepository = new Mock<ISpinResultRepository>();
            _mockTransactionRepository = new Mock<ITransactionRepository>();

            _slotService = new SlotService(
                _mockUserRepository.Object,
                _mockSpinResultRepository.Object,
                _mockTransactionRepository.Object);
        }

        [Fact]
        public async Task Spin_NonPositiveBetAmount_ReturnsError()
        {
            var result = await _slotService.Spin("userId", 0m);

            Assert.Equal(ErrorCode.NonPositiveBetAmount, result.ErrorCode);
        }

        [Fact]
        public async Task Spin_UserNotFound_ReturnsError()
        {
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<string>()))
                               .ReturnsAsync((Domain.Entities.User)null);

            var result = await _slotService.Spin("nonExistentUserId", 10m);

            Assert.Equal(ErrorCode.UserNotFound, result.ErrorCode);
        }

        [Fact]
        public async Task Spin_InsufficientBalance_ReturnsError()
        {
            var user = new Domain.Entities.User { Balance = 5m };
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<string>()))
                               .ReturnsAsync(user);

            var result = await _slotService.Spin("userId", 10m);

            Assert.Equal(ErrorCode.InsufficientBalance, result.ErrorCode);
        }

        [Fact]
        public async Task Spin_WithValidBet_UpdatesUserBalanceAndCreatesTransactions()
        {
            var userId = "testUser";
            var betAmount = 10m;
            var user = new User { Id = userId, Balance = 20m };

            _mockUserRepository.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.Update(It.IsAny<User>())).Returns(Task.CompletedTask);
            _mockTransactionRepository.Setup(repo => repo.Add(It.IsAny<Transaction>())).Returns(Task.CompletedTask);
            _mockSpinResultRepository.Setup(repo => repo.Add(It.IsAny<SpinResult>())).Returns(Task.CompletedTask);

            var result = await _slotService.Spin(userId, betAmount);

            Assert.True(result.Success);
            _mockUserRepository.Verify(repo => repo.Update(It.Is<User>(u => u.Balance == 10m)), Times.Once);
            _mockTransactionRepository.Verify(repo => repo.Add(It.IsAny<Transaction>()), Times.AtLeastOnce);
            _mockSpinResultRepository.Verify(repo => repo.Add(It.IsAny<SpinResult>()), Times.Once);
        }
    }
}
