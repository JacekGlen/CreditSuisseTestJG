using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCashCard.AccountBalance
{
    public interface IAccountBalanceManager
    {
        bool IsBalanceAvailable(int accountId, decimal amount);
        AccountBalanceTransaction CheckAndReserveAmount(int accountId, decimal amount);
        void ProcessTransaction(AccountBalanceTransaction transaction);
        void AddAmount(int accountId, decimal amount);
    }
}
