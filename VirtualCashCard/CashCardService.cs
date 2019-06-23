using System;
using System.Collections.Generic;
using System.Text;
using VirtualCashCard.AccountBalance;
using VirtualCashCard.Repositories;

namespace VirtualCashCard
{
    public class CashCardService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IAccountBalanceManager accountBalanceManager;

        public CashCardService(IAccountRepository accountRepository, IAccountBalanceManager accountBalanceManager)
        {
            this.accountRepository = accountRepository;
            this.accountBalanceManager = accountBalanceManager;
        }

        public WithdrawalCommandResponse Withdraw(WithdrawalCommandRequest request)
        {
            var account = accountRepository.GetByCardNumber(request.CardNumber);

            if (!account.IsPinValid(request.Pin))
            {
                return new WithdrawalCommandResponse(false, "Invalid PIN");
            }

            if (!accountBalanceManager.IsBalanceAvailable(account.Id, request.Amount))
            {
                return new WithdrawalCommandResponse(false, "Balance unavailable");
            }

            try
            {
                var txn = accountBalanceManager.CheckAndReserveAmount(account.Id, request.Amount);
                accountBalanceManager.ProcessTransaction(txn);
                return new WithdrawalCommandResponse(true);
            }
            catch (Exception ex)
            {
                return new WithdrawalCommandResponse(false, ex.Message);
            }
        }

        public TopupCommandResponse Topup(TopupCommandRequest request)
        {
            var account = accountRepository.GetByCardNumber(request.CardNumber);

            if (!account.IsPinValid(request.Pin))
            {
                return new TopupCommandResponse(false, "Invalid PIN");
            }

            try
            {
                accountBalanceManager.AddAmount(account.Id, request.Amount);
                return new TopupCommandResponse(true);
            }
            catch (Exception ex)
            {
                return new TopupCommandResponse(false, ex.Message);
            }
        }
    }
}
