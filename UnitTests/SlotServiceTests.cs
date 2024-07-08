using System;
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

namespace Application.Tests
{
    public class SlotServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
        private readonly Mock<ISpinResultRepository> _mockSpinResultRepository = new Mock<ISpinResultRepository>();
        private readonly Mock<ITransactionRepository> _mockTransactionRepository = new Mock<ITransactionRepository>();

        [Fact]
        public async Task Spin_NonPositiveBetAmount_ReturnsError()
        {
            var slotService = new SlotService(
                _mockUserRepository.Object,
                _mockSpinResultRepository.Object,
                _mockTransactionRepository.Object);

            var result = await slotService.Spin("userId", 0m);

            Assert.Equal(ErrorCode.NonPositiveBetAmount, result.ErrorCode);
        }

        [Fact]
        public async Task Spin_UserNotFound_ReturnsError()
        {
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<string>()))
                               .ReturnsAsync((Domain.Entities.User)null);
            var slotService = new SlotService(
                _mockUserRepository.Object,
                _mockSpinResultRepository.Object,
                _mockTransactionRepository.Object);

            var result = await slotService.Spin("nonExistentUserId", 10m);

            Assert.Equal(ErrorCode.UserNotFound, result.ErrorCode);
        }

        [Fact]
        public async Task Spin_InsufficientBalance_ReturnsError()
        {
            var user = new Domain.Entities.User { Balance = 5m };
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<string>()))
                               .ReturnsAsync(user);
            var slotService = new SlotService(
                _mockUserRepository.Object,
                _mockSpinResultRepository.Object,
                _mockTransactionRepository.Object);

            var result = await slotService.Spin("userId", 10m);

            Assert.Equal(ErrorCode.InsufficientBalance, result.ErrorCode);
        }
    }
}
