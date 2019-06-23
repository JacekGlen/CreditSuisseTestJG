using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Moq;
using VirtualCashCard;
using VirtualCashCard.AccountBalance;
using VirtualCashCard.Models;
using VirtualCashCard.Repositories;

namespace VirtualCashCard.Tests
{
    public class CashCardServiceTest
    {
        Mock<IAccountRepository> _accountRepositoryMock;
        Mock<IAccountBalanceManager> _accountBalanceManagerMock;

        CashCardService sut;

        [SetUp]
        public void Setup()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _accountRepositoryMock
                .Setup(ar => ar.GetByCardNumber("123"))
                .Returns(new Account()
                {
                    Id = 1,
                    CardNumber = "123",
                    Pin = "8888",
                });

            _accountBalanceManagerMock = new Mock<IAccountBalanceManager>();
            _accountBalanceManagerMock
                .Setup(abm => abm.IsBalanceAvailable(1, It.IsAny<decimal>()))
                .Returns(true);
            _accountBalanceManagerMock
                .Setup(abm => abm.CheckAndReserveAmount(1, It.IsAny<decimal>()))
                .Returns(new AccountBalanceTransaction() { Id = 567, });

            sut = new CashCardService(_accountRepositoryMock.Object, _accountBalanceManagerMock.Object);
        }

        [Test]
        public void CompletesValidTopupTransaction()
        {
            var request = new TopupCommandRequest()
            {
                CardNumber = "123",
                Amount = 100,
                Pin = "8888",
            };

            var result = sut.Topup(request);

            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public void FailsTopupIfPinInvalid()
        {
            var request = new TopupCommandRequest()
            {
                CardNumber = "123",
                Amount = 100,
                Pin = "7777",
            };

            var result = sut.Topup(request);

            Assert.IsFalse(result.IsSuccessful);
        }

        [Test]
        public void FailsTopupOnAccountBalanceManagerError()
        {
            var request = new TopupCommandRequest()
            {
                CardNumber = "123",
                Amount = 100,
                Pin = "8888",
            };

            _accountBalanceManagerMock
                .Setup(abm => abm.AddAmount(1, It.IsAny<decimal>()))
                .Throws(new Exception("Connection failed"));

            var result = sut.Topup(request);

            Assert.IsFalse(result.IsSuccessful);
        }

        [Test]
        public void CompletesValidWithdrawalTransaction()
        {
            var request = new WithdrawalCommandRequest()
            {
                CardNumber = "123",
                Amount = 100,
                Pin = "8888",
            };

            var result = sut.Withdraw(request);

            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public void FailsWithdrawalIfPinInvalid()
        {
            var request = new WithdrawalCommandRequest()
            {
                CardNumber = "123",
                Amount = 100,
                Pin = "7777",
            };

            var result = sut.Withdraw(request);

            Assert.IsFalse(result.IsSuccessful);
        }

        [Test]
        public void FailsWithdrawalIfInsufficientFunds()
        {
            var request = new WithdrawalCommandRequest()
            {
                CardNumber = "123",
                Amount = 100,
                Pin = "8888",
            };
            _accountBalanceManagerMock
                .Setup(abm => abm.IsBalanceAvailable(1, 100))
                .Returns(false);

            var result = sut.Withdraw(request);

            Assert.IsFalse(result.IsSuccessful);
        }

        [Test]
        public void FailsWithdrawalICannotReserveFunds()
        {
            var request = new WithdrawalCommandRequest()
            {
                CardNumber = "123",
                Amount = 100,
                Pin = "8888",
            };
            _accountBalanceManagerMock
                .Setup(abm => abm.CheckAndReserveAmount(1, 100))
                .Throws(new Exception("Connection error"));

            var result = sut.Withdraw(request);

            Assert.IsFalse(result.IsSuccessful);
        }

        [Test]
        public void FailsWithdrawalICannotCompleteTransaction()
        {
            var request = new WithdrawalCommandRequest()
            {
                CardNumber = "123",
                Amount = 100,
                Pin = "8888",
            };
            _accountBalanceManagerMock
                .Setup(abm => abm.ProcessTransaction(It.IsAny<AccountBalanceTransaction>()))
                .Throws(new Exception("Connection error"));

            var result = sut.Withdraw(request);

            Assert.IsFalse(result.IsSuccessful);
        }
    }
}
